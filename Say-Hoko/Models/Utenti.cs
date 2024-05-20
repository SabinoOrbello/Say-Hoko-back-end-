using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Say_Hoko.Models;

namespace Say_Hoko.Models;

public partial class Utenti
{
    public int UserId { get; set; }

    public string? Nome { get; set; }

    public string? Cognome { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Telefono { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<MetodiConsegna> MetodiConsegnas { get; set; } = new List<MetodiConsegna>();

    public virtual ICollection<Ordini> Ordinis { get; set; } = new List<Ordini>();

    public virtual ICollection<Pagamenti> Pagamentis { get; set; } = new List<Pagamenti>();
}

