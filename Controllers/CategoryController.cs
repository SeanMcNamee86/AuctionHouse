using AuctionHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoryController : Controller
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
    public CategoryController(AuctionHouseContext context)
    {
        db = context;
    }

    [HttpGet("/categories/new")]
    public IActionResult New()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        return View("New");
    }

    [HttpPost("/categories/create")]
    public IActionResult Create(Category newCategory)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index", "User");
        }

        if (!ModelState.IsValid )
        {
            return New();
        }


        Console.WriteLine(newCategory.CategoryId);

        db.Categories.Add(newCategory);

        db.SaveChanges();
        Console.WriteLine(newCategory.CategoryId);

        return RedirectToAction("All");


    }

    [HttpGet("/categories")]
    public IActionResult All()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }


        List<Category> allCategories = db.Categories.ToList();
        ViewBag.name = HttpContext.Session.GetString("Name");

        return View("All", allCategories);
    }

    [HttpGet("/categories/{oneCategoryId}")]
    public IActionResult GetOneCategory(int oneCategoryId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        Category? category = db.Categories
            .FirstOrDefault(v => v.CategoryId == oneCategoryId);
       

        if (category == null)
        {
            return RedirectToAction("All");
        }

        return View("Details", category);
    }

    [HttpPost("/categories/{deletedCategoryId}/delete")]
    public IActionResult DeleteCategory(int deletedCategoryId)
    {
        if (!loggedIn && HttpContext.Session.GetString("Admin") != "true")
        {
            return RedirectToAction("Index", "User");
        }
        Category? category = db.Categories.FirstOrDefault(p => p.CategoryId == deletedCategoryId);

        if (category != null)
        {
            db.Categories.Remove(category);
            db.SaveChanges();
        }
        return RedirectToAction("All");
    }

    [HttpGet("/Categories/{categoryId}/edit")]
    public IActionResult Edit(int categoryId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        Category? category = db.Categories.FirstOrDefault(p => p.CategoryId == categoryId);

        if (category == null || HttpContext.Session.GetString("Admin") != "true")
        {
            return RedirectToAction("All");
        }

        return View("Edit", category);
    }

    [HttpPost("/categories/{categoryId}/update")]
    public IActionResult Update(Category editedCategory, int categoryId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index", "User");
        }
        if (ModelState.IsValid == false)
        {
            return Edit(categoryId);
        }

        Category? dbCategory = db.Categories.FirstOrDefault(category => category.CategoryId == categoryId);

        if (dbCategory == null || HttpContext.Session.GetString("Admin") != "true")
        {
            return RedirectToAction("All");
        }

        dbCategory.Name = editedCategory.Name;
        dbCategory.UpdatedAt = DateTime.Now;

        db.Categories.Update(dbCategory);
        db.SaveChanges();

        return RedirectToAction("GetOneCategory", new { CategoryId = dbCategory.CategoryId });
    }


}