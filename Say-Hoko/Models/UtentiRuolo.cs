using System;
using System.Collections.Generic;

namespace Say_Hoko.Models;

public partial class UtentiRuolo
{
    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Utenti? User { get; set; }
}
