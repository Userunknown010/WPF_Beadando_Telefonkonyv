using System;
using System.Collections.Generic;

namespace Telefonkönyv.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public int PermissionId { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual Permission Permission { get; set; } = null!;
}
