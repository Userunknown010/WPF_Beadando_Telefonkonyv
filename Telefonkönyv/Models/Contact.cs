using System;
using System.Collections.Generic;

namespace Telefonkönyv.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string? Nickname { get; set; }

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public int? CityId { get; set; }

    public int? UploaderId { get; set; }

    public int? PictureId { get; set; }

    public virtual City? City { get; set; }

    public virtual Picture? Picture { get; set; }

    public virtual User? Uploader { get; set; }
}
