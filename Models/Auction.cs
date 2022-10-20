#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
namespace AuctionHouse.Models;
public class Auction
{

    [Key] // Primary Key
    public int AuctionId { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Name")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string Name { get; set; }
    [Display(Name = "Buy it now price")]
    [Range(0, Int32.MaxValue, ErrorMessage = " must be a positive value!")]
    public int BuyNow { get; set; }
    [Display(Name = "Minimum Bid amount")]
    public int MinBid { get; set; } = 0;

    [Display(Name = "Current highest bid")]
    public int HighBid { get; set; } = 0;

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }
    [MinLength(20, ErrorMessage = "Your Auction Description must be 20 characters")]
    public string Description { get; set; }
    public int UserId { get; set; }

    public string? ImgURL {get; set;}
    public bool isFinished {get; set;} = false;
    public User? Creator { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Bid> Bids { get; set; } = new List<Bid>();
    public List<AuctionCategory> AuctionCategories { get; set; } = new List<AuctionCategory>();

    public string StringDate()
    {
        return (this.EndDate).ToString("MMM dd, yyyy HH:mm:ss");
    } 

    public void RetireAuction()
    {
        //logic to end auction
    }
}