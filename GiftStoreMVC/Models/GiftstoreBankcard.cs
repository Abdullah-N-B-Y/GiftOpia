using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreBankcard
{
    public string Cardnumber { get; set; } = null!;

    public string? Cardholdername { get; set; }

    public DateTime? Expirationdate { get; set; }

    public string? Cvv { get; set; }

    public decimal? Totalamount { get; set; }

    public virtual ICollection<GiftstoreBank> GiftstoreBanks { get; set; } = new List<GiftstoreBank>();
}
