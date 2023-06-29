using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreMessage
{
    public decimal Messagelid { get; set; }

    public string? Messagecontent { get; set; }

    public DateTime? Messagedate { get; set; }

    public decimal? Senderid { get; set; }

    public decimal? Reseiverid { get; set; }

    public virtual GiftstoreUser? Reseiver { get; set; }

    public virtual GiftstoreUser? Sender { get; set; }
}
