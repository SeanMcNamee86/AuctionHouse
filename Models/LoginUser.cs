#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
namespace AuctionHouse.Models;
public class LoginUser
{
    
    [Key] // Primary Key

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Email Address")]
    [EmailAddress]
    public string LoginEmail { get; set; }
    [DataType(DataType.Password)]
    public string LoginPassword { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}