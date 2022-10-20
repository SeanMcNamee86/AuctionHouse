#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Models;

public class Bid 
{
    [Key]
    public int BidId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public float Amount {get; set;}
    public bool WinningBid {get; set; } = false;
    public int UserId { get; set; }
    public User? User { get; set; }
    public int AuctionId { get; set; }
    public Auction? Auction { get; set; }
}