using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStoreMVC.Models;

public partial class GiftstoreGift
{
    public decimal Giftid { get; set; }

    public string? Giftname { get; set; }

    public decimal? Giftprice { get; set; }

    public string? Imagepath { get; set; }

    public int? Giftavailability { get; set; }

    public string? Giftdescription { get; set; }

    public decimal? Categoryid { get; set; }

    public decimal? Orderid { get; set; }

    public decimal? Userid { get; set; }

    [NotMapped]
    public virtual IFormFile? GiftImage { get; set; }
    public virtual GiftstoreCategory? Category { get; set; }

    public virtual ICollection<GiftstoreSenderrequest> GiftstoreSenderrequests { get; set; } = new List<GiftstoreSenderrequest>();

    public virtual GiftstoreOrder? Order { get; set; }

    public virtual GiftstoreUser? User { get; set; }
}
