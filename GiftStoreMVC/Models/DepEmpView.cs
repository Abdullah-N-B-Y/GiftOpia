using System;
using System.Collections.Generic;

namespace GiftStoreMVC.Models;

public partial class DepEmpView
{
    public decimal Depid { get; set; }

    public string Depaddress { get; set; } = null!;

    public decimal Empid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public decimal? Salary { get; set; }

    public string? Phonenumber { get; set; }

    public string? Empaddress { get; set; }

    public decimal? EDepid { get; set; }

    public decimal? Age { get; set; }
}
