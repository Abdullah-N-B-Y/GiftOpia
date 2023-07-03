using GiftStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace GiftStoreMVC.Controllers;

public class AuthController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private GiftstoreNotification _notification;
    public AuthController(ModelContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult SignIn() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SignIn([Bind("Username,Password")] GiftstoreUser user)
    {
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Username.Equals(user.Username) && obj.Password.Equals(user.Password)).SingleOrDefault();//a13df10
        if (currentUser != null)
        {
            if (currentUser.Approvalstatus.Equals("Accepted"))
            {
                HttpContext.Session.SetString("Username", currentUser.Username);
                HttpContext.Session.SetString("Password", currentUser.Password);
                HttpContext.Session.SetInt32("UserId", (int)currentUser.Userid);
                switch (currentUser.Roleid)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        return RedirectToAction("Index", "Maker");
                    case 3:
                        return RedirectToAction("Index", "Sender");
                    case 4:
                        //Driver
                        return RedirectToAction("Index", "");
                }
            }
            else if (currentUser.Approvalstatus.Equals("Pending"))
            { 
                    
            }
        }
        TempData["SignIn"] = "UserName or Password isVailed ";
        return View();
    }


    public IActionResult SignUp()
    {
        ViewData["categories"] = _context.GiftstoreCategories.ToList();
        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(GiftstoreUser? user, string? confirmPassword, decimal? userCategoryId)
    {
        if (ModelState.IsValid)
        {
            if(user.Password.Equals(confirmPassword))
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName;

                if (user.UserImage != null)
                {
                    fileName = Guid.NewGuid().ToString() + user.UserImage.FileName;
                    string? path = Path.Combine(wwwRootPath, "UsersImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.UserImage.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    fileName = "default-profile-image.png";
                    string? path = Path.Combine(wwwRootPath, "UsersImages", fileName);
                }
                user.Imagepath = fileName;

                if (user.RoleName == "Sender")
                {
                    user.Roleid = 3;
                    user.RoleName = "Sender";
                    user.Approvalstatus = "Accepted";
                }
                else
                {
                    if (user.RoleName == "Maker")
                    {
                        user.Roleid = 2;
                        user.RoleName = "Maker";
                        user.Categoryid = userCategoryId;
                    }
                    else
                    {
                        user.Roleid = 4;
                        user.RoleName = "Driver";
                    }
                    user.Approvalstatus = "Pending";

                    _notification = new()
                    {
                        Notificationcontent = "Accept my registration for the system",
                        Notificationdate = DateTime.Now,
                        Email = user.Email,
                        Isread = false
                    };
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                if (_notification != null)
                {
                    _context.Add(_notification);
                    await _context.SaveChangesAsync();
                }
                return user.RoleName.Equals("Sender") ? RedirectToAction("SignIn", "Auth") : RedirectToAction("Index", "Home");
            }
        }
        ViewData["categories"] = _context.GiftstoreCategories.ToList();
        return View(user);
    }
}