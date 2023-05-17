using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace User.Controllers;
using BeltExam.Models;
using Microsoft.AspNetCore.Mvc.Filters;


public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext _context;
    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("/")]
    [Route("/home")]
    public IActionResult HomePage()
    {
        return View("HomePage");
    }

    [HttpPost]
    [Route("/logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("HomePage");
    }

    [HttpPost]
    [Route("/users/create")]
    public IActionResult CreateUser(User newUser)
    {
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();   
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password); 
                _context.Add(newUser);
                _context.SaveChanges();
                Console.WriteLine("New User sent to database");
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                HttpContext.Session.SetString("UserName", newUser.FirstName);
                return RedirectToAction("AllHabits", "Habit");
            }
            else
            {
                return View("HomePage");
            }
        }
    }

    [HttpPost]
    [Route("/login")]
    public IActionResult LoginUser(ReturningUser ReturningUser)
    {
        if (!ModelState.IsValid)
        {
            return View("HomePage");
        }
        User? OneUser = _context.Users.FirstOrDefault(a => a.Email == ReturningUser.ReturningEmail);
        if (OneUser.Email == null)
        {
            ModelState.AddModelError("ReturningEmail", "Please check your credentials.");
            return View("HomePage");
        }
        PasswordHasher<ReturningUser> hasher = new PasswordHasher<ReturningUser>();                    
        var result = hasher.VerifyHashedPassword(ReturningUser, OneUser.Password, ReturningUser.ReturningPassword);
        if ((int)result == 1)
        {
            HttpContext.Session.SetInt32("UserId", OneUser.UserId);
            HttpContext.Session.SetString("UserName", OneUser.FirstName);
            return RedirectToAction("AllHabits", "Habit");
        } else {
            ModelState.AddModelError("ReturningEmail", "Please check your credentials.");
            return View("HomePage");
        }
    }
}

public class MustBeLoggedInAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? UserId = context.HttpContext.Session.GetInt32("UserId");
        // Check to see if we got back null
        if (UserId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("LoginUser", "User", null);
        }
    }
}