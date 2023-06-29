using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStoreMVC.Models;

public partial class GiftstoreUser
{
    public decimal Userid { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? Approvalstatus { get; set; }

    public string? Phonenumber { get; set; }

    public string? Imagepath { get; set; }

    public decimal? Categoryid { get; set; }

    public decimal? Roleid { get; set; }

    public decimal? Profits { get; set; }

    [NotMapped]
    public virtual IFormFile? UserImage { get; set; }

    [NotMapped]
    public virtual string? RoleName { get; set; }

    public virtual GiftstoreCategory? Category { get; set; }

    public virtual ICollection<GiftstoreGift> GiftstoreGifts { get; set; } = new List<GiftstoreGift>();

    public virtual ICollection<GiftstoreMessage> GiftstoreMessageReseivers { get; set; } = new List<GiftstoreMessage>();

    public virtual ICollection<GiftstoreMessage> GiftstoreMessageSenders { get; set; } = new List<GiftstoreMessage>();

    public virtual ICollection<GiftstoreNotification> GiftstoreNotifications { get; set; } = new List<GiftstoreNotification>();

    public virtual ICollection<GiftstorePage> GiftstorePages { get; set; } = new List<GiftstorePage>();

    public virtual ICollection<GiftstorePaymentrecored> GiftstorePaymentrecoreds { get; set; } = new List<GiftstorePaymentrecored>();

    public virtual ICollection<GiftstoreSenderrequest> GiftstoreSenderrequests { get; set; } = new List<GiftstoreSenderrequest>();

    public virtual ICollection<GiftstoreTestimonial> GiftstoreTestimonials { get; set; } = new List<GiftstoreTestimonial>();

    public virtual GiftstoreRole? Role { get; set; }
}
