﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NZWalks.API.Data;

#nullable disable

namespace NZWalks.API.Migrations
{
    [DbContext(typeof(NZWalksDbContext))]
    partial class NZWalksDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NZWalks.API.Models.Domains.Difficulty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Difficulties");

                    b.HasData(
                        new
                        {
                            Id = new Guid("250713a7-58ab-43a9-8256-0574718edde3"),
                            Name = "Easy"
                        },
                        new
                        {
                            Id = new Guid("51d91533-e380-40bd-b0b9-2ab226885ef2"),
                            Name = "Medium"
                        },
                        new
                        {
                            Id = new Guid("bbbc3995-0041-440a-bc04-a327c9b4fc89"),
                            Name = "Hard"
                        });
                });

            modelBuilder.Entity("NZWalks.API.Models.Domains.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FileSizeInBytes")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("NZWalks.API.Models.Domains.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Regions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f1b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                            Code = "NO",
                            ImageUrl = "https://www.doc.govt.nz/globalassets/images/places/northland/northland-landscape-1.jpg",
                            Name = "Northland"
                        },
                        new
                        {
                            Id = new Guid("f2b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                            Code = "AU",
                            ImageUrl = "https://www.doc.govt.nz/globalassets/images/places/auckland/auckland-landscape-1.jpg",
                            Name = "Auckland"
                        },
                        new
                        {
                            Id = new Guid("f3b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                            Code = "WK",
                            ImageUrl = "https://www.doc.govt.nz/globalassets/images/places/waikato/waikato-landscape-1.jpg",
                            Name = "Waikato"
                        },
                        new
                        {
                            Id = new Guid("f4b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                            Code = "BP",
                            ImageUrl = "https://www.doc.govt.nz/globalassets/images/places/bay-of-plenty/bay-of-plenty-landscape-1.jpg",
                            Name = "Bay of Plenty"
                        },
                        new
                        {
                            Id = new Guid("f5b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                            Code = "GI",
                            ImageUrl = "https://www.doc.govt.nz/globalassets/images/places/gisborne/gisborne-landscape-1.jpg",
                            Name = "Gisborne"
                        },
                        new
                        {
                            Id = new Guid("f6b3b3b4-1b3b-4b3b-8b3b-1b3b3b3b3b3b"),
                            Code = "HB",
                            ImageUrl = "https://www.doc.govt.nz/globalassets/images/places/hawkes-bay/hawkes-bay-landscape-1.jpg",
                            Name = "Hawke's Bay"
                        });
                });

            modelBuilder.Entity("NZWalks.API.Models.Domains.Walk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DifficultyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LengthInKm")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DifficultyId");

                    b.HasIndex("RegionId");

                    b.ToTable("Walks");
                });

            modelBuilder.Entity("NZWalks.API.Models.Domains.Walk", b =>
                {
                    b.HasOne("NZWalks.API.Models.Domains.Difficulty", "Difficulty")
                        .WithMany()
                        .HasForeignKey("DifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NZWalks.API.Models.Domains.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Difficulty");

                    b.Navigation("Region");
                });
#pragma warning restore 612, 618
        }
    }
}
