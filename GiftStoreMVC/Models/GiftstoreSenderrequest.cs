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

    public decimal? Makerid { get; set; }

    public string? Sendername { get; set; }

    public string? Giftname { get; set; }

    public decimal? Giftprice { get; set; }

    public virtual ICollection<GiftstoreOrder> GiftstoreOrders { get; set; } = new List<GiftstoreOrder>();

    public virtual GiftstoreUser? Maker { get; set; }

    public virtual GiftstoreUser? Sender { get; set; }
}
