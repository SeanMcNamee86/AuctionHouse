#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;
namespace AuctionHouse.Models;

public class AuctionHouseContext : DbContext 
{ 
    public AuctionHouseContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; } 
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<AuctionCategory> AuctionCategories { get; set; }
}
