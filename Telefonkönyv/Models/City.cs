using System;
using System.Collections.Generic;

namespace Telefonkönyv.Models;

public partial class City
{
    public int CityId { get; set; }

    public string? CityName { get; set; }

    public string? Irsz { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
