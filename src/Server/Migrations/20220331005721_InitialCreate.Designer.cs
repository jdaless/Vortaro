﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace vortaro.Server.Migrations
{
    [DbContext(typeof(VortaroContext))]
    [Migration("20220331005721_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_esperanto_ci")
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Enhavo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("FontoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Teksto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("VortoId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FontoId");

                    b.ToTable("Enhavoj");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Enhavo");
                });

            modelBuilder.Entity("Fonto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Favoreco")
                        .HasColumnType("int");

                    b.Property<string>("KreintoId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Ligilo")
                        .HasColumnType("longtext");

                    b.Property<string>("Signo")
                        .HasColumnType("longtext");

                    b.Property<string>("Titolo")
                        .HasColumnType("longtext");

                    b.Property<bool>("ĈuUzantkreita")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("KreintoId");

                    b.ToTable("Fontoj");
                });

            modelBuilder.Entity("Lingvo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nomo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Lingvoj");
                });

            modelBuilder.Entity("Radiko", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DerivaĵaVortoId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Ordo")
                        .HasColumnType("int");

                    b.Property<Guid>("RadikaVortoId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DerivaĵaVortoId");

                    b.HasIndex("RadikaVortoId");

                    b.ToTable("Radiko");
                });

            modelBuilder.Entity("Uzanto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Bildo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nomo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Uzantoj");
                });

            modelBuilder.Entity("Voĉdono", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("EnhavoId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("FontoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UzantoId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("ĈuSupraPoento")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("EnhavoId");

                    b.HasIndex("FontoId");

                    b.HasIndex("UzantoId");

                    b.ToTable("Voĉdonoj");
                });

            modelBuilder.Entity("Vorto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("FinaĵoId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FontoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Teksto")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("FinaĵoId");

                    b.HasIndex("FontoId");

                    b.ToTable("Vortoj");
                });

            modelBuilder.Entity("Difino", b =>
                {
                    b.HasBaseType("Enhavo");

                    b.HasIndex("VortoId");

                    b.HasDiscriminator().HasValue("Difino");
                });

            modelBuilder.Entity("Ekzemplo", b =>
                {
                    b.HasBaseType("Enhavo");

                    b.HasIndex("VortoId");

                    b.HasDiscriminator().HasValue("Ekzemplo");
                });

            modelBuilder.Entity("Traduko", b =>
                {
                    b.HasBaseType("Enhavo");

                    b.Property<string>("LingvoId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasIndex("LingvoId");

                    b.HasIndex("VortoId");

                    b.HasDiscriminator().HasValue("Traduko");
                });

            modelBuilder.Entity("Enhavo", b =>
                {
                    b.HasOne("Fonto", "Fonto")
                        .WithMany()
                        .HasForeignKey("FontoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fonto");
                });

            modelBuilder.Entity("Fonto", b =>
                {
                    b.HasOne("Uzanto", "Kreinto")
                        .WithMany()
                        .HasForeignKey("KreintoId");

                    b.Navigation("Kreinto");
                });

            modelBuilder.Entity("Radiko", b =>
                {
                    b.HasOne("Vorto", "DerivaĵaVorto")
                        .WithMany("Radikoj")
                        .HasForeignKey("DerivaĵaVortoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vorto", "RadikaVorto")
                        .WithMany("Derivaĵoj")
                        .HasForeignKey("RadikaVortoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DerivaĵaVorto");

                    b.Navigation("RadikaVorto");
                });

            modelBuilder.Entity("Voĉdono", b =>
                {
                    b.HasOne("Enhavo", "Enhavo")
                        .WithMany()
                        .HasForeignKey("EnhavoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fonto", null)
                        .WithMany("Voĉdonoj")
                        .HasForeignKey("FontoId");

                    b.HasOne("Uzanto", "Uzanto")
                        .WithMany()
                        .HasForeignKey("UzantoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enhavo");

                    b.Navigation("Uzanto");
                });

            modelBuilder.Entity("Vorto", b =>
                {
                    b.HasOne("Vorto", "Finaĵo")
                        .WithMany()
                        .HasForeignKey("FinaĵoId");

                    b.HasOne("Fonto", "Fonto")
                        .WithMany()
                        .HasForeignKey("FontoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Finaĵo");

                    b.Navigation("Fonto");
                });

            modelBuilder.Entity("Difino", b =>
                {
                    b.HasOne("Vorto", "Vorto")
                        .WithMany("Difinoj")
                        .HasForeignKey("VortoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vorto");
                });

            modelBuilder.Entity("Ekzemplo", b =>
                {
                    b.HasOne("Vorto", "Vorto")
                        .WithMany("Ekzemploj")
                        .HasForeignKey("VortoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vorto");
                });

            modelBuilder.Entity("Traduko", b =>
                {
                    b.HasOne("Lingvo", "Lingvo")
                        .WithMany()
                        .HasForeignKey("LingvoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vorto", "Vorto")
                        .WithMany("Tradukoj")
                        .HasForeignKey("VortoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lingvo");

                    b.Navigation("Vorto");
                });

            modelBuilder.Entity("Fonto", b =>
                {
                    b.Navigation("Voĉdonoj");
                });

            modelBuilder.Entity("Vorto", b =>
                {
                    b.Navigation("Derivaĵoj");

                    b.Navigation("Difinoj");

                    b.Navigation("Ekzemploj");

                    b.Navigation("Radikoj");

                    b.Navigation("Tradukoj");
                });
#pragma warning restore 612, 618
        }
    }
}