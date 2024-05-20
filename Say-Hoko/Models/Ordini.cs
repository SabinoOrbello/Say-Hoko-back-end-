using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Say_Hoko.Models;

namespace Say_Hoko.Models;

public partial class Ordini
{
    public int OrdiniId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DataOra { get; set; }

    public string? Stato { get; set; }

    public decimal? Totale { get; set; }
    public string? Indirizzo { get; set; }
    public string? Citta { get; set; }
    public string? NumeroTelefono { get; set; }
    public string? Note { get; set; }

    public virtual ICollection<DettagliOrdine> DettagliOrdines { get; set; } = new List<DettagliOrdine>();

    public virtual ICollection<Pagamenti> Pagamentis { get; set; } = new List<Pagamenti>();

    public virtual Utenti? User { get; set; }
}


