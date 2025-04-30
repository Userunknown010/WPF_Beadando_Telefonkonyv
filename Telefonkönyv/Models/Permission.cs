using System;
using System.Collections.Generic;

namespace Telefonkönyv.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
