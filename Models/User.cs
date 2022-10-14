#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHouse.Models;
public class User
{
    
    [Key] // Primary Key
    public int UserId { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "First Name")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Last Name")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string LastName { get; set; }
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email {get; set;}    [Required(ErrorMessage = "is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "must be at least 8 characters")]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Password fields must match")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPW { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Auction> CreatedAuctions { get; set; } = new List<Auction>();
    public List<Bid> Bidding { get; set; } = new List<Bid>();
}