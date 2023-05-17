#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BeltExam.Models;
public class Habit
{
    [Key]
    public int HabitId { get; set; }
    [Required(ErrorMessage = "is required.")]
    public string Name {get;set;}
    [Required(ErrorMessage = "is required.")]
    public string Description {get;set;}
    [Required(ErrorMessage = "is required.")]
    public string Category {get;set;}
    public int UserId {get;set;}
    public User? user { get; set; }
    public List<Cheer> Cheers { get; set; } = new List<Cheer>();
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}