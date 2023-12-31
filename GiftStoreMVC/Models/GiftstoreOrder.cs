﻿using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreOrder
{
    public decimal Orderid { get; set; }

    public DateTime? Orderdate { get; set; }

    public string? Orderstatus { get; set; }

    public string? Recipientaddress { get; set; }

    public DateTime? Arrivaldate { get; set; }

    public decimal? Finalprice { get; set; }

    public decimal? Requestid { get; set; }

    public virtual GiftstoreSenderrequest? Request { get; set; }
}
