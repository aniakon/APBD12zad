using System;
using System.Collections.Generic;
using APBDcw12.Models;
using Microsoft.EntityFrameworkCore;

namespace APBDcw12.Data;

public partial class Apbd12Context : DbContext
{
    public Apbd12Context(DbContextOptions<Apbd12Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_pk");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

            entity.ToTable("Client_Trip");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Client");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Trip");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                        j.ToTable("Country_Trip");
                    });
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

            entity.ToTable("Trip");

            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Client>().HasData(new List<Client>
        {
            new Client()
            {
                IdClient = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jkowalski@gmail.com",
                Telephone = "1111111", Pesel = "11111111"
            },
            new Client()
            {
                IdClient = 2, FirstName = "Julia", LastName = "Nowak", Email = "jnowak@gmail.com", Telephone = "222222",
                Pesel = "22222222"
            },
        });

        modelBuilder.Entity<Trip>().HasData(new List<Trip>
        {
            new Trip()
            {
                IdTrip = 1, Name = "wyc1", Description = "desc", DateFrom = DateTime.Parse("02-02-2022"),
                DateTo = DateTime.Parse("03-03-2022"), MaxPeople = 20
            },
            new Trip()
            {
                IdTrip = 2, Name = "wyc2", Description = "desc", DateFrom = DateTime.Parse("02-02-2022"),
                DateTo = DateTime.Parse("02-04-2022"), MaxPeople = 30
            }
        });

        modelBuilder.Entity<Country>().HasData(new List<Country>
        {
            new Country() { IdCountry = 1, Name = "Poland" },
            new Country() { IdCountry = 2, Name = "Spain" },
        });
        
        modelBuilder.Entity<Trip>()
            .HasMany(t => t.IdCountries)
            .WithMany(c => c.IdTrips)
            .UsingEntity<Dictionary<string, object>>(
                "Country_Trip",
                r => r.HasData(
                    new { IdCountry = 1, IdTrip = 1 },
                    new { IdCountry = 2, IdTrip = 2 }
                )
            );

        modelBuilder.Entity<ClientTrip>().HasData(new List<ClientTrip>{
            new ClientTrip()
            {
                IdClient = 1,
                IdTrip = 1,
                RegisteredAt = DateTime.Parse("02-02-2022"),
                PaymentDate = null
            },
            new ClientTrip()
            {
                IdClient = 2,
                IdTrip = 1,
                RegisteredAt = DateTime.Parse("02-02-2022"),
                PaymentDate = null
            },
            new ClientTrip()
            {
                IdClient = 2,
                IdTrip = 2,
                RegisteredAt = DateTime.Parse("02-02-2022"),
                PaymentDate = null
            }}
        );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
