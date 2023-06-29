using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreTestimonial
{
    public decimal Testimonialid { get; set; }

    public string? Testimonialcontent { get; set; }

    public DateTime? Testimonialdate { get; set; }

    public string? Testimonialstatus { get; set; }

    public decimal? Userid { get; set; }

    public virtual GiftstoreUser? User { get; set; }
}
