using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreSenderrequest
{
    public decimal Requestid { get; set; }

    public string? Recipientname { get; set; }

    public string? Recipientaddress { get; set; }

    public string? Requeststatus { get; set; }

    public DateTime? Requestdate { get; set; }

    public decimal? Senderid { get; set; }

    public decimal? Giftid { get; set; }

    public virtual GiftstoreGift? Gift { get; set; }

    public virtual GiftstoreUser? Sender { get; set; }
}
