using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GiftStoreMVC.Controllers;

public class HomeController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private GiftstoreNotification notification;
    public HomeController(ModelContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {

        var testimonials = _context.GiftstoreTestimonials.ToList();
        var homePage = _context.GiftstorePages.Where(obj => obj.Pageid == 1).SingleOrDefault();

        ViewData["HomePageContent"] = homePage.Pagecontent.ToString();

        return View(testimonials);
    }

    public IActionResult About() 
    {
        var categories = _context.GiftstoreCategories.ToList();
        var aboutPage = _context.GiftstorePages.Where(obj=>obj.Pageid == 2).SingleOrDefault();

        ViewData["AboutPageContent"] = aboutPage.Pagecontent.ToString();
        return View(categories);
    }

    public IActionResult Categories()
    {
        var categories = _context.GiftstoreCategories.ToList();
        return View(categories);
    }

    public IActionResult Gifts(decimal? Categoryid)
    {
        var modelContext = _context.GiftstoreGifts.Where(obj => obj.Categoryid == Categoryid).ToList();

        return View(modelContext);
    }

    public IActionResult Privacy() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}