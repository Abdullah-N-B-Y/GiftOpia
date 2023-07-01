using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStoreMVC.Models;

public partial class GiftstoreCategory
{
    public decimal Categoryid { get; set; }

    public string? Categoryname { get; set; }

    public string? Imagepath { get; set; }

    public string? Categorydescription { get; set; }

    [NotMapped]
    public virtual IFormFile? CategoryImage { get; set; }
    public virtual ICollection<GiftstoreGift> GiftstoreGifts { get; set; } = new List<GiftstoreGift>();

    public virtual ICollection<GiftstoreUser> GiftstoreUsers { get; set; } = new List<GiftstoreUser>();
}
