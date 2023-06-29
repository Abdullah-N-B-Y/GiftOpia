using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class GiftstoreRole
{
    public decimal Roleid { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<GiftstoreUser> GiftstoreUsers { get; set; } = new List<GiftstoreUser>();
}
