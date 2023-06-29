using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstorePage
{
    public decimal Pageid { get; set; }

    public string? Pagetitle { get; set; }

    public string? Pagecontent { get; set; }

    public decimal? Adminid { get; set; }

    public virtual GiftstoreUser? Admin { get; set; }
}
