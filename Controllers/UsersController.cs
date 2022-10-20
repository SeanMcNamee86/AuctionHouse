using AuctionHouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UsersController : Controller
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
    public UsersController(AuctionHouseContext context)
    {
        db = context;
    }

    [HttpGet("/users")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Theme") == null)
        {
            HttpContext.Session.SetString("Theme", "light");
            HttpContext.Session.SetString("ThemeOpposite", "dark");
        }
        if (loggedIn)
        {
            return RedirectToAction("Index", "Home");
        }
        return View("Index");
    }



    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            if (db.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email is taken");
            }
        }

        if (!ModelState.IsValid)
        {
            return Index();
        }

        PasswordHasher<User> hasedPW = new PasswordHasher<User>();
        newUser.Password = hasedPW.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (ModelState.IsValid == false)
        {
            return Index();
        }

        User? dbUser = db.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);

        if (dbUser == null)
        {
            ModelState.AddModelError("LoginEmail", "Invalid Credentials");
            return Index();
        }

        PasswordHasher<LoginUser> hashedPW = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hashedPW.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LoginEmail", "Invalid Credentials");
            return Index();
        }

        if (dbUser.isSuper == true)
        {
            HttpContext.Session.SetString("Admin", "true");
        }

        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/users/dashboard")]
    public IActionResult Dashboard()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "Home");
        }


        List<Auction> allAuctions = db.Auctions

            .Include(v => v.Creator)
            .Include(v => v.Bids)
            .ThenInclude(b => b.User)
            .ToList();

        return View("Dashboard", allAuctions);
    }

    [HttpGet("/users/{oneUserId}")]
    public IActionResult GetOneUser(int oneUserId)
    {
        ViewBag.CreatorInfo = db.Users.FirstOrDefault(u => u.UserId == oneUserId);

        List<Auction> allAuctions = db.Auctions

            .Include(a => a.Creator)
            .Include(a => a.Bids)
            .ThenInclude(b => b.User)
            .Where(u => u.UserId == oneUserId)
            .ToList();

        if (allAuctions == null)
        {
            return RedirectToAction("All");
        }

        if (HttpContext.Session.GetInt32("UUID") == oneUserId)
        {
            return RedirectToAction("Dashboard");
        }
        return View("OneUser", allAuctions);
    }

    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}