﻿using AuctionHouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
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
    public HomeController(AuctionHouseContext context)
    {
        db = context;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Theme") == null)
        {
            HttpContext.Session.SetString("Theme", "light");
            HttpContext.Session.SetString("ThemeOpposite", "dark");
        }

        List<Auction> allAuctions = db.Auctions
        .Include(a => a.AuctionCategories)
        .ThenInclude(ac => ac.Category)
        .ToList();

        ViewBag.AllCategories = db.Categories.ToList();

        return View("Index", allAuctions);
    }

    [HttpPost("/dark")]
    public IActionResult SetDark()
    {
        HttpContext.Session.SetString("Theme", "dark");
        HttpContext.Session.SetString("ThemeOpposite", "light");
        return Redirect("/");
    }

    [HttpPost("/light")]
    public IActionResult SetLight()
    {
        HttpContext.Session.SetString("Theme", "light");
        HttpContext.Session.SetString("ThemeOpposite", "dark");
        return Redirect("/");
    }
}