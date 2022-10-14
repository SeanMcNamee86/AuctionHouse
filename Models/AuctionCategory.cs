#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Models;

public class AuctionCategory 
{
    [Key]
    public int AuctionCategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int AuctionId { get; set; }
    public Auction? Auction { get; set; }
}