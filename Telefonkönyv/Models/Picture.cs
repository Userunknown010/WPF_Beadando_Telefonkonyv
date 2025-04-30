using System;
using System.Collections.Generic;

namespace Telefonkönyv.Models;

public partial class Picture
{
    public int Id { get; set; }

    public byte[]? Picture1 { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
