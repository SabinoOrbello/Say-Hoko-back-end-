using System;
using System.Collections.Generic;

namespace Say_Hoko.Models;

public partial class Pagamenti
{
    public int PagamentoId { get; set; }

    public int? UserId { get; set; }

    public int? OrdiniId { get; set; }

    public string? Metodo { get; set; }

    public decimal? Importo { get; set; }

    public string? Stato { get; set; }

    public DateTime? DataOra { get; set; }

    public virtual Ordini? Ordini { get; set; }

    public virtual Utenti? User { get; set; }
}
