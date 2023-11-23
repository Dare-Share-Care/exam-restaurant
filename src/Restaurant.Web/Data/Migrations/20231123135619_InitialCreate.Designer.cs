﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.Web.Data;

#nullable disable

namespace Restaurant.Web.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    [Migration("20231123135619_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurant.Web.Entities.MenuItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("RestaurantId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Description = "Green and from trees",
                            Name = "Apple",
                            Price = 1.99m,
                            RestaurantId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            Description = "Green and from trees",
                            Name = "Banana",
                            Price = 2.99m,
                            RestaurantId = 1L
                        },
                        new
                        {
                            Id = 3L,
                            Description = "Green and from trees",
                            Name = "Orange",
                            Price = 3.99m,
                            RestaurantId = 1L
                        });
                });

            modelBuilder.Entity("Restaurant.Web.Entities.Restauranten", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Zipcode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Restaurant");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Address = "Cool kid street",
                            Name = "McDorra",
                            Zipcode = 42069
                        });
                });

            modelBuilder.Entity("Restaurant.Web.Entities.MenuItem", b =>
                {
                    b.HasOne("Restaurant.Web.Entities.Restauranten", "Restauranten")
                        .WithMany("Menu")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restauranten");
                });

            modelBuilder.Entity("Restaurant.Web.Entities.Restauranten", b =>
                {
                    b.Navigation("Menu");
                });
#pragma warning restore 612, 618
        }
    }
}
