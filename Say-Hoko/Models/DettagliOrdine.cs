using System;
using System.Collections.Generic;

namespace Say_Hoko.Models;

public partial class DettagliOrdine
{
    public int DettagliOrdineId { get; set; }

    public int? OrdiniId { get; set; }

    public int? ProdottoId { get; set; }

    public int? Quantità { get; set; }

    public decimal? Prezzo { get; set; }

    public virtual Ordini? Ordini { get; set; }

    public virtual Prodotti? Prodotto { get; set; }
}
