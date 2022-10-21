#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
namespace AuctionHouse.Models;
public class Auction
{

    [Key] // Primary Key
    public int AuctionId { get; set; }

    [Required(ErrorMessage = "Auction Name is required")]
    [Display(Name = "Name")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Buy Now Price is required")]
    [Display(Name = "Buy it now price")]
    [Range(0, Int32.MaxValue, ErrorMessage = "Buy Now Price must be a positive value!")]
    public float BuyNow { get; set; }

    [Required(ErrorMessage = "Minimum Bid Amount is required")]
    [Display(Name = "Minimum Bid amount")]
    public float MinBid { get; set; } = 0;

    [Display(Name = "Current highest bid")]
    public float HighBid { get; set; } = 0;

    [Required(ErrorMessage = "End Date is required")]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [MinLength(20, ErrorMessage = "Auction Description must be at least 20 characters")]
    public string Description { get; set; }
    public int UserId { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    public string? ImgURL { get; set; }
    public bool isFinished { get; set; } = false;
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

    public TimeSpan TimeRemaining()
    {
        if ((this.EndDate - DateTime.Now) <= (DateTime.Now - DateTime.Now))
        {
            return DateTime.Now - DateTime.Now;
        }
        else
        {
            return this.EndDate - DateTime.Now;
        }
    }
}