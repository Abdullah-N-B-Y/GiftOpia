using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreBank
{
    public decimal Bankid { get; set; }

    public string? Bankname { get; set; }

    public string? Cardnumber { get; set; }

    public virtual GiftstoreBankcard? CardnumberNavigation { get; set; }
}
