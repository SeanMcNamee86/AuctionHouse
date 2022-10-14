#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
namespace AuctionHouse.Models;
public class Category
{
    
    [Key] // Primary Key
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Name")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<AuctionCategory> CategoryAuctions { get; set; } = new List<AuctionCategory>();

}