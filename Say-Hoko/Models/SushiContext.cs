using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Say_Hoko.Models;

public partial class SushiContext : DbContext
{
    public SushiContext()
    {
    }

    public SushiContext(DbContextOptions<SushiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<DettagliOrdine> DettagliOrdines { get; set; }

    public virtual DbSet<MetodiConsegna> MetodiConsegnas { get; set; }

    public virtual DbSet<Ordini> Ordinis { get; set; }

    public virtual DbSet<Pagamenti> Pagamentis { get; set; }

    public virtual DbSet<Prodotti> Prodottis { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Utenti> Utentis { get; set; }

    public virtual DbSet<UtentiRuolo> UtentiRuolos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=SABINOORBELLO\\SQLEXPRESS;Database=Sushi;Trusted_Connection=true;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.CategorieId).HasName("PK__Categori__F643AD86DC6C7253");

            entity.ToTable("Categorie");

            entity.Property(e => e.CategorieId).HasColumnName("CategorieID");
            entity.Property(e => e.NomeCategoria).HasMaxLength(50);
        });

        modelBuilder.Entity<DettagliOrdine>(entity =>
        {
            entity.HasKey(e => e.DettagliOrdineId).HasName("PK__Dettagli__404742920D0202CE");

            entity.ToTable("DettagliOrdine");

            entity.Property(e => e.DettagliOrdineId).HasColumnName("DettagliOrdineID");
            entity.Property(e => e.OrdiniId).HasColumnName("OrdiniID");
            entity.Property(e => e.Prezzo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProdottoId).HasColumnName("ProdottoID");

            entity.HasOne(d => d.Ordini).WithMany(p => p.DettagliOrdines)
                .HasForeignKey(d => d.OrdiniId)
                .HasConstraintName("FK__DettagliO__Ordin__412EB0B6");

            entity.HasOne(d => d.Prodotto).WithMany(p => p.DettagliOrdines)
                .HasForeignKey(d => d.ProdottoId)
                .HasConstraintName("FK__DettagliO__Prodo__4222D4EF");
        });

        modelBuilder.Entity<MetodiConsegna>(entity =>
        {
            entity.HasKey(e => e.MetodoId).HasName("PK__MetodiCo__5C1E3E310B3CD614");

            entity.ToTable("MetodiConsegna");

            entity.Property(e => e.MetodoId).HasColumnName("MetodoID");
            entity.Property(e => e.Cap)
                .HasMaxLength(10)
                .HasColumnName("CAP");
            entity.Property(e => e.Citta).HasMaxLength(100);
            entity.Property(e => e.Indirizzo).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.MetodiConsegnas)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__MetodiCon__UserI__49C3F6B7");
        });

        modelBuilder.Entity<Ordini>(entity =>
        {
            entity.HasKey(e => e.OrdiniId).HasName("PK__Ordini__FE9A1F650C9ED873");

            entity.ToTable("Ordini");

            entity.Property(e => e.OrdiniId).HasColumnName("OrdiniID");
            entity.Property(e => e.DataOra).HasColumnType("datetime");
            entity.Property(e => e.Stato).HasMaxLength(50);
            entity.Property(e => e.Totale).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Ordinis)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ordini__UserID__3E52440B");
        });

        modelBuilder.Entity<Pagamenti>(entity =>
        {
            entity.HasKey(e => e.PagamentoId).HasName("PK__Pagament__977DE7D3CF5F9C99");

            entity.ToTable("Pagamenti");

            entity.Property(e => e.PagamentoId).HasColumnName("PagamentoID");
            entity.Property(e => e.DataOra).HasColumnType("datetime");
            entity.Property(e => e.Importo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Metodo).HasMaxLength(50);
            entity.Property(e => e.OrdiniId).HasColumnName("OrdiniID");
            entity.Property(e => e.Stato).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Ordini).WithMany(p => p.Pagamentis)
                .HasForeignKey(d => d.OrdiniId)
                .HasConstraintName("FK__Pagamenti__Ordin__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.Pagamentis)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Pagamenti__UserI__4CA06362");
        });

        modelBuilder.Entity<Prodotti>(entity =>
        {
            entity.HasKey(e => e.ProdottoId).HasName("PK__Prodotti__9BE523B8E56CD97B");

            entity.ToTable("Prodotti");

            entity.Property(e => e.ProdottoId).HasColumnName("ProdottoID");
            entity.Property(e => e.CategorieId).HasColumnName("CategorieID");
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.Prezzo).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categorie).WithMany(p => p.Prodottis)
                .HasForeignKey(d => d.CategorieId)
                .HasConstraintName("FK__Prodotti__Catego__3B75D760");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A0F3274FB");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(30);
        });

        modelBuilder.Entity<Utenti>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Utenti__1788CCAC03F5C83E");

            entity.ToTable("Utenti");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Cognome).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Telefono).HasMaxLength(100);
        });

        modelBuilder.Entity<UtentiRuolo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UtentiRuolo");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UtentiRuo__RoleI__46E78A0C");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UtentiRuo__UserI__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
