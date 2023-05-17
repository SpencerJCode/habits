#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BeltExam.Models;
public class Cheer
{
    [Key]
    public int CheerId { get; set; }
    public int UserId { get; set; }
    public int HabitId { get; set; }
    public User? User { get; set; }
    public Habit? Habit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}