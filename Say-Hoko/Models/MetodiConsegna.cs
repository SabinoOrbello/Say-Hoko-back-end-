using System;
using System.Collections.Generic;

namespace Say_Hoko.Models;

public partial class MetodiConsegna
{
    public int MetodoId { get; set; }

    public int? UserId { get; set; }

    public string? Tipo { get; set; }

    public string? Indirizzo { get; set; }

    public string? Citta { get; set; }

    public string? Cap { get; set; }

    public virtual Utenti? User { get; set; }
}
