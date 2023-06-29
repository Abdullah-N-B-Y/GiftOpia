using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreNotification
{
    public decimal Notificationlid { get; set; }

    public string? Notificationcontent { get; set; }

    public DateTime? Notificationdate { get; set; }

    public string? Email { get; set; }

    public bool? Isread { get; set; }

    public decimal? Userid { get; set; }

    public virtual GiftstoreUser? User { get; set; }
}
