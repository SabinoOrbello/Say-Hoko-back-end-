using System;
using System.Collections.Generic;

namespace Say_Hoko.Models;

public partial class Categorie
{
    public int CategorieId { get; set; }

    public string? NomeCategoria { get; set; }

    public virtual ICollection<Prodotti> Prodottis { get; set; } = new List<Prodotti>();
}
