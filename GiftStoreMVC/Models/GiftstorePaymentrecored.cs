using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstorePaymentrecored
{
    public decimal Paymentid { get; set; }

    public string? Cardnumber { get; set; }

    public decimal? Paymentamount { get; set; }

    public DateTime? Paymentdate { get; set; }

    public string? Paymentstatus { get; set; }

    public decimal? Userid { get; set; }

    public virtual GiftstoreUser? User { get; set; }
}
