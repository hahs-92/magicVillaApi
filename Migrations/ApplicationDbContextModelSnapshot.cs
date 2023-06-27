﻿// <auto-generated />
using System;
using MagicVilla_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MagicVilla_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Area")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Area = 60,
                            CreatedAt = new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(995),
                            Description = "Description test",
                            ImageUrl = "https://image.com",
                            Name = "villa test",
                            Price = 999.0,
                            UpdatedAt = new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(1011)
                        },
                        new
                        {
                            Id = 2,
                            Area = 40,
                            CreatedAt = new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(1013),
                            Description = "Description test 02",
                            ImageUrl = "https://image02.com",
                            Name = "villa test 02",
                            Price = 990.0,
                            UpdatedAt = new DateTime(2023, 6, 27, 12, 17, 52, 331, DateTimeKind.Local).AddTicks(1014)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
