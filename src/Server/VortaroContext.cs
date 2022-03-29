using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


public class VortaroContext : DbContext
{
    public DbSet<Vorto> Vortoj { get; set; } = null!;
    public DbSet<Fonto> Fontoj { get; set; } = null!;
    public DbSet<Lingvo> Lingvoj { get; set; } = null!;
    public DbSet<Enhavo> Enhavoj { get; set; } = null!;
    public DbSet<Ekzemplo> Ekzemploj { get; set; } = null!;
    public DbSet<Difino> Difinoj { get; set; } = null!;
    public DbSet<Traduko> Tradukoj { get; set; } = null!;
    public DbSet<Uzanto> Uzantoj { get; set; } = null!;
    public DbSet<Voĉdono> Voĉdonoj { get; set; } = null!;

    public string DbPath {get;}

    public VortaroContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "vortaro.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //options.UseSqlite($"Data Source={DbPath}");        
        options.UseMySql(
            Environment.GetEnvironmentVariable("ConnectionStrings:UVD",EnvironmentVariableTarget.User), 
            new MySqlServerVersion(new Version(5, 7, 36)),
            options =>
            {
                
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_esperanto_ci");

        modelBuilder.Entity<Radiko>()
            .HasOne(v => v.RadikaVorto)
            .WithMany(v => v.Derivaĵoj);

        modelBuilder.Entity<Vorto>()
            .Navigation(v => v.Radikoj)
            .AutoInclude();

        modelBuilder.Entity<Radiko>()
            .HasOne(v => v.DerivaĵaVorto)
            .WithMany(v => v.Radikoj);

        modelBuilder.Entity<Vorto>()
            .HasOne(v => v.Finaĵo);
    }
}