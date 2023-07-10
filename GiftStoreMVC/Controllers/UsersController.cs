using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStoreMVC.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Hosting;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace GiftStoreMVC.Controllers;

public class UsersController : Controller
{
    private readonly ModelContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UsersController(ModelContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        decimal? id = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        IQueryable<GiftstoreUser> modelContext = _context.GiftstoreUsers.Where(obj=> obj.Approvalstatus.Equals("Accepted"));
        return View(await modelContext.ToListAsync());
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        if (id == null || _context.GiftstoreUsers == null)
        {
            return NotFound();
        }

        GiftstoreUser? giftstoreUser = await _context.GiftstoreUsers
            .Include(g => g.Category)
            .Include(g => g.Role)
            .FirstOrDefaultAsync(m => m.Userid == id);
        if (giftstoreUser == null)
        {
            return NotFound();
        }

        return View(giftstoreUser);
    }

    public async Task<IActionResult> Edit(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        if (id == null || _context.GiftstoreUsers == null)
        {
            return NotFound();
        }

        GiftstoreUser? giftstoreUser = await _context.GiftstoreUsers.FindAsync(id);
        if (giftstoreUser == null)
        {
            return NotFound();
        }
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        return View(giftstoreUser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(decimal id, [Bind("Userid,Username,Password,Email,Name,Approvalstatus,Phonenumber,Imagepath,Categoryid,Roleid,Profits")] GiftstoreUser giftstoreUser)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        if (id != giftstoreUser.Userid)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(giftstoreUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiftstoreUserExists(giftstoreUser.Userid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Categoryid"] = new SelectList(_context.GiftstoreCategories, "Categoryid", "Categoryid", giftstoreUser.Categoryid);
        ViewData["Roleid"] = new SelectList(_context.GiftstoreRoles, "Roleid", "Roleid", giftstoreUser.Roleid);
        return View(giftstoreUser);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(decimal? id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;
        if (id == null || _context.GiftstoreUsers == null)
        {
            return NotFound();
        }

        GiftstoreUser? giftstoreUser = await _context.GiftstoreUsers
            .FirstOrDefaultAsync(obj => obj.Userid == id);
        if (giftstoreUser == null)
        {
            return NotFound();
        }

        return View(giftstoreUser);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(decimal id)
    {
        decimal? id2 = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentUser = _context.GiftstoreUsers.Where(obj => obj.Userid == id2).SingleOrDefault();
        ViewData["Username"] = currentUser.Username;
        ViewData["Name"] = currentUser.Name;
        ViewData["Password"] = currentUser.Password;
        ViewData["Email"] = currentUser.Email;
        ViewData["UserId"] = id;
        ViewData["RoleId"] = currentUser.Roleid;
        ViewData["ImagePath"] = currentUser.Imagepath;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        if (_context.GiftstoreUsers == null)
        {
            return Problem("Entity set 'ModelContext.GiftstoreUsers'  is null.");
        }
        GiftstoreUser? giftstoreUser = await _context.GiftstoreUsers
            .FirstOrDefaultAsync(obj => obj.Userid == id);
        if (giftstoreUser != null)
        {
            _context.GiftstoreUsers.Remove(giftstoreUser);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

	public IActionResult Profile()
	{
        int? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? user2 = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();
        ViewData["Username"] = user2.Username;
        ViewData["Name"] = user2.Name;
        ViewData["Password"] = user2.Password;
        ViewData["Email"] = user2.Email;
        ViewData["UserId"] = userId;
        ViewData["RoleId"] = user2.Roleid;
        ViewData["ImagePath"] = user2.Imagepath;
        ViewData["PhoneNumber"] = user2.Phonenumber;

        ViewData["NumberOfNotification"] = _context.GiftstoreNotifications.Count(obj => obj.Isread == false);

        GiftstoreUser? currentLoggedUser = _context.GiftstoreUsers.Where(obj=> obj.Userid == userId).FirstOrDefault();
		return View(currentLoggedUser);
	}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserImage(IFormFile userImage)
    {
        decimal? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentLoggedUser = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).FirstOrDefault();
        if (ModelState.IsValid)
        {
            try
            {
                if (userId != currentLoggedUser.Userid)
                {
                    return NotFound();
                }
                if (userImage != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + userImage.FileName;
                    string path = Path.Combine(wwwRootPath, "UsersImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await userImage.CopyToAsync(fileStream);
                    }
                    currentLoggedUser.Imagepath = fileName;

                    _context.Update(currentLoggedUser);
                    await _context.SaveChangesAsync();

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));
                    email.To.Add(MailboxAddress.Parse(currentLoggedUser.Email));

                    email.Subject = "Update Image Profile";
                    email.Body = new TextPart(TextFormat.Html) { Text = "Hi" + " " + currentLoggedUser.Name + " your image updated successfully" };


                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        return RedirectToAction("Profile","Users",currentLoggedUser);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPersonalData(string? userName, string? userPhoneNumber, string? password)
    {
        decimal? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentLoggedUser = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).FirstOrDefault();
        if (ModelState.IsValid)
        {
            try
            {
                if (password != null && password.Equals(currentLoggedUser?.Password))
                {
                    if (userId != currentLoggedUser.Userid)
                    {
                        return NotFound();
                    }
                    if (userName != null)
                    {
                        currentLoggedUser.Name = userName;
                    }
                    if (userPhoneNumber != null)
                    {
                        currentLoggedUser.Phonenumber = userPhoneNumber;
                    }
                    _context.Update(currentLoggedUser);
                    await _context.SaveChangesAsync();

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));
                    email.To.Add(MailboxAddress.Parse(currentLoggedUser.Email));

                    email.Subject = "Update User Personal Data Profile";
                    email.Body = new TextPart(TextFormat.Html) { Text = "Hi" + " " + currentLoggedUser.Name + " your Personal Data updated successfully" };


                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        return RedirectToAction("Profile", "Users", currentLoggedUser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPassword(string? oldPassword, string? newPassword, string? confirmPassword)
    {
        decimal? userId = HttpContext.Session.GetInt32("UserId");
        GiftstoreUser? currentLoggedUser = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).FirstOrDefault();
        if (ModelState.IsValid)
        {
            try
            {
                if (oldPassword != null && oldPassword.Equals(currentLoggedUser?.Password))
                {
                    if (newPassword != null && confirmPassword != null && newPassword.Equals(confirmPassword))
                    {
                        currentLoggedUser.Password = newPassword;

                        _context.Update(currentLoggedUser);
                        await _context.SaveChangesAsync();

                        var email = new MimeMessage();
                        email.From.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));
                        email.To.Add(MailboxAddress.Parse(currentLoggedUser.Email));

                        email.Subject = "Update Profile GifOpia Password";
                        email.Body = new TextPart(TextFormat.Html) { Text = "Hi" + " " + currentLoggedUser.Name + " your password updated successfully" };


                        using var smtp = new SmtpClient();
                        smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
                        smtp.Send(email);
                        smtp.Disconnect(true);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        return RedirectToAction("Profile", "Users", currentLoggedUser);
    }



    public IActionResult ContactUs()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ContactUs(string? fname, string? lname, string? email1, string? text)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(email1));
            email.To.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));

            email.Subject = "Contact us";
            email.Body = new TextPart(TextFormat.Html) { Text = "Greetings, this is my contact for your site GiftOpia." };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        catch
        {
            return View();
        }
        return View();
    }


    public IActionResult Feedback()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Feedback(string? fname, string? lname, string? email1, string? message)
    {
        try
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            GiftstoreUser? user = _context.GiftstoreUsers.Where(obj => obj.Userid == userId).SingleOrDefault();

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("GiftOpiaSite@outlook.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));

            email.Subject = "Feedback";
            email.Body = new TextPart(TextFormat.Html) { Text = "Hi" + " " + user.Name + " thanks for your feedback and be sure that we always want to read from you." };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("GiftOpiaSite@outlook.com", "Abdullah2000$!");
            smtp.Send(email);
            smtp.Disconnect(true);


            GiftstoreTestimonial testimonial = new()
            {
                Testimonialcontent = message,
                Testimonialdate = DateTime.UtcNow,
                Testimonialstatus = "Not read"
            };
            _context.Add(testimonial);
            _context.SaveChanges();
        }
        catch
        {
            return View();
        }
        return RedirectToAction("Index", "Sender");
    }


    public IActionResult Ban(decimal Userid)
    {
        var bannedUser = _context.GiftstoreUsers.Where(obj=>obj.Userid == Userid).SingleOrDefault();

        bannedUser.Approvalstatus = "Banned";
        _context.Update(bannedUser);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }



    private bool GiftstoreUserExists(decimal id) => (_context.GiftstoreUsers?.Any(e => e.Userid == id)).GetValueOrDefault();
}