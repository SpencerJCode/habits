using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Habit.Controllers;
using BeltExam.Models;
using Microsoft.AspNetCore.Mvc.Filters;


[MustBeLoggedIn]
public class HabitController : Controller
{
    private readonly ILogger<HabitController> _logger;
    private MyContext _context;
    public HabitController(ILogger<HabitController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("/habits")]
    public IActionResult AllHabits()
    {
        List<Habit> AllHabits = _context.Habits
            .Include(habit => habit.Cheers)
                .ThenInclude(cheer => cheer.User)
            .Include(habit => habit.user)
            .ToList();
        return View("AllHabits", AllHabits);
    }

    [HttpGet]
    [Route("/habits/new")]
    public IActionResult NewHabit()
    {
        return View("NewHabit");
    }

    [HttpPost]
    [Route("/habits/create")]
    public IActionResult CreateHabit(Habit newHabit)
    {
        if (!ModelState.IsValid)
        {
            return View("NewHabit");
        } else {
            _context.Add(newHabit);
            _context.SaveChanges();
            return RedirectToAction("AllHabits");
        }
    }

    [HttpGet]
    [Route("/habits/{HabitId}/edit")]
    public IActionResult EditHabit(int HabitId)
    {
        Habit? editHabit = _context.Habits.SingleOrDefault(habit => habit.HabitId == HabitId);
        return View("EditHabit", editHabit);
    }

    [HttpPost]
    [Route("/habits/update")]
    public IActionResult UpdateHabit(Habit updatedHabit)
    {
            Habit? oldHabit = _context.Habits.SingleOrDefault(habit => habit.HabitId == updatedHabit.HabitId);
        if (ModelState.IsValid)
        {
            oldHabit.Name = updatedHabit.Name;
            oldHabit.Description = updatedHabit.Description;
            oldHabit.Category = updatedHabit.Category;
            oldHabit.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("AllHabits");
        } else {
            return View("EditHabit", oldHabit);
        }
    }

    [HttpPost]
    [Route("/habits/{HabitId}/delete")]
    public IActionResult DeleteHabit(int HabitId)
    {
        _context.Habits.Remove(_context.Habits.SingleOrDefault(i => i.HabitId == HabitId));
        _context.SaveChanges();
        return RedirectToAction("AllHabits");
    }

    [HttpPost]
    [Route("/cheers/{HabitId}/Create")]
    public IActionResult CreateCheer(int HabitId)
    {
        Cheer newCheer = new Cheer();
        newCheer.HabitId = HabitId;
        newCheer.UserId = (int)HttpContext.Session.GetInt32("UserId");
        _context.Cheers.Add(newCheer);
        _context.SaveChanges();
        return RedirectToAction("AllHabits");
    }

    [HttpGet]
    [Route("/habits/{UserId}/view")]
    public IActionResult ViewUser(int UserId)
    {
        List<Habit> UserHabits = _context.Habits.Where(habit => habit.UserId == UserId)
            .Include(habit => habit.Cheers)
                .ThenInclude(cheer => cheer.User)
            .ToList();
        User? user = _context.Users.FirstOrDefault(user => user.UserId == UserId);
        ViewBag.UserName = user.FirstName;
        return View("UserHabits", UserHabits);
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
            context.Result = new RedirectToActionResult("HomePage", "User", null);
        }
    }
}