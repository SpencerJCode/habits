#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BeltExam.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "is required.")]
    [MinLength(2,  ErrorMessage = "must have at least 2 characters")]    
    public string FirstName {get;set;}
    [Required(ErrorMessage = "is required.")]
    [MinLength(2,  ErrorMessage = "must have at least 2 characters")]    
    public string LastName {get;set;}
    [Required(ErrorMessage = "is required.")]
    [UniqueEmail]
    [EmailAddress]
    public string Email {get;set;}
    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Password)]
    [StrongPassword]
    public string Password {get;set;}
    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Password)]
    [NotMapped]
    [Compare("Password")]
    public string ConfirmPassword {get;set;}
    List<Cheer> Cheers { get; set; } = new List<Cheer>();    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            return new ValidationResult("is required!");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("is already in use.");
        } else {
            return ValidationResult.Success;
        }
    }
}

public class StrongPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) {
            return new ValidationResult("is required.");
        }
        if(!value.ToString().Contains("!") 
            &&
            !value.ToString().Contains("@")
            &&
            !value.ToString().Contains("#")
            &&
            !value.ToString().Contains("$")
            &&
            !value.ToString().Contains("%")
            &&
            !value.ToString().Contains("^")
            &&
            !value.ToString().Contains("&")
            &&
            !value.ToString().Contains("*")
            &&
            !value.ToString().Contains("?"))
        {
            return new ValidationResult("must contain a special character.");
        } else if (!value.ToString().Contains("1") 
            &&
            !value.ToString().Contains("2")
            &&
            !value.ToString().Contains("3")
            &&
            !value.ToString().Contains("4")
            &&
            !value.ToString().Contains("5")
            &&
            !value.ToString().Contains("6")
            &&
            !value.ToString().Contains("7")
            &&
            !value.ToString().Contains("8")
            &&
            !value.ToString().Contains("9")
            &&
            !value.ToString().Contains("0"))
            {
                return new ValidationResult("must contain a number.");
            } else if (value.ToString().Length < 8) {
                return new ValidationResult("must be at least 8 characters.");
            }
        
        return ValidationResult.Success;
    }
}