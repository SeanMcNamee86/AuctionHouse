using AuctionHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AuctionController : Controller
{
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }

    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }

    private AuctionHouseContext db;
    public AuctionController(AuctionHouseContext context)
    {
        db = context;
    }

    [HttpGet("/auctions/new")]
    public IActionResult New()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        ViewBag.AllCategories = db.Categories.ToList();
        return View("New");
    }

    [HttpPost("/auctions/create")]
    public IActionResult Create(Auction newAuction, int catId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "Users");
        }
        newAuction.isFinished = false;
        newAuction.HighBid = (float)(newAuction.MinBid * 1.025);
        if (newAuction.EndDate < DateTime.Now)
        {
            ModelState.AddModelError("EndDate", "must be in the future");
        }
        
        if (!ModelState.IsValid)
        {
            return New();
        }


        newAuction.UserId = (int)uid;

        Console.WriteLine(newAuction.AuctionId);

        db.Auctions.Add(newAuction);

        db.SaveChanges();
        AuctionCategory newAC = new AuctionCategory()
        {
            CategoryId = catId,
            AuctionId = newAuction.AuctionId
        }
        db.AuctionCategories.Add(newAC);
        db,SaveChanges();
        Console.WriteLine(newAuction.AuctionId);

        return RedirectToAction("All");


    }

    [HttpGet("/auctions")]
    public IActionResult All()
    {


        List<Auction> allAuctions = db.Auctions

            .Include(v => v.Creator)
            .Include(v => v.Bids)
            .ToList();
        ViewBag.name = HttpContext.Session.GetString("Name");

        return View("All", allAuctions);
    }

    [HttpGet("/auctions/{oneAuctionId}")]
    public IActionResult GetOneAuction(int oneAuctionId)
    {

        Auction? auction = db.Auctions
            .Include(w => w.Creator)
            .Include(w => w.Bids)
            .ThenInclude(a => a.User)
            .FirstOrDefault(v => v.AuctionId == oneAuctionId);


        if (auction == null)
        {
            return RedirectToAction("All");
        }
        return View("Details", auction);
    }

    [HttpPost("/auctions/{deletedAuctionId}/delete")]
    public IActionResult DeleteAuction(int deletedAuctionId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        Auction? auction = db.Auctions.FirstOrDefault(p => p.AuctionId == deletedAuctionId);

        if (auction != null && auction.UserId == uid)
        {
            db.Auctions.Remove(auction);
            db.SaveChanges();
        }
        return RedirectToAction("All");
    }

    [HttpGet("/auctions/{auctionId}/edit")]
    public IActionResult Edit(int auctionId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        Auction? auction = db.Auctions.FirstOrDefault(p => p.AuctionId == auctionId);

        if (auction == null || auction.UserId != uid)
        {
            return RedirectToAction("All");
        }

        return View("Edit", auction);
    }

    [HttpPost("/auctions/{auctionId}/update")]
    public IActionResult Update(Auction editedAuction, int auctionId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Users");
        }
        if (ModelState.IsValid == false)
        {
            return Edit(auctionId);
        }

        Auction? dbAuction = db.Auctions.FirstOrDefault(auction => auction.AuctionId == auctionId);

        if (dbAuction == null || dbAuction.UserId != uid)
        {
            return RedirectToAction("All");
        }

        dbAuction.Name = editedAuction.Name;
        dbAuction.EndDate = editedAuction.EndDate;
        dbAuction.Description = editedAuction.Description;
        dbAuction.UpdatedAt = DateTime.Now;

        db.Auctions.Update(dbAuction);
        db.SaveChanges();

        return RedirectToAction("GetOneAuction", new { AuctionId = dbAuction.AuctionId });
    }

    [HttpPost("/auctions/{auctionId}/bid")]
    public IActionResult Bid(float amount, int auctionId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "Users");
        }
        Auction? dbAuction = db.Auctions.FirstOrDefault(auction => auction.AuctionId == auctionId);
        if (dbAuction != null)
        {
            if (amount < dbAuction.HighBid)
            {
                ModelState.AddModelError("Amount", "Must be greater than the current highest bid!");
                return RedirectToAction("GetOneAuction", new { AuctionId = dbAuction.AuctionId });
            }
        }
        Bid newBid = new Bid()
        {
            UserId = (int)uid,
            AuctionId = auctionId,
            Amount = amount
        };

        db.Bids.Add(newBid);
        if (dbAuction != null)
        {
            dbAuction.HighBid = newBid.Amount;
            db.Auctions.Update(dbAuction);
        }




        db.SaveChanges();
        return RedirectToAction("GetOneAuction", new { oneAuctionId = auctionId });
    }
}