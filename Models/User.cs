#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHouse.Models;
public class User
{

    [Key] // Primary Key
    public int UserId { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^([\w.-]+)@([\w-]+)((.(\w){2,3})+)$", ErrorMessage = "Must be a valid Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "must be at least 8 characters")]
    public string Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Password fields must match")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPW { get; set; }
    public bool isSuper {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Auction> CreatedAuctions { get; set; } = new List<Auction>();
    public List<Bid> Bidding { get; set; } = new List<Bid>();
}