using System;
using System.Collections.Generic;

namespace Say_Hoko.Models;

public partial class Prodotti
{
    public int ProdottoId { get; set; }

    public int? CategorieId { get; set; }

    public string? Nome { get; set; }

    public string? Ingredienti { get; set; }

    public string? Descrizione { get; set; }

    public string? Immagine { get; set; }

    public string? Allergeni1 { get; set; }

    public string? Allergeni2 { get; set; }

    public string? Allergeni3 { get; set; }

    public decimal? Prezzo { get; set; }

    public bool? Disponibilità { get; set; }

    public virtual Categorie? Categorie { get; set; }

    public virtual ICollection<DettagliOrdine> DettagliOrdines { get; set; } = new List<DettagliOrdine>();
}
