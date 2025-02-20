﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieRental.Data;

#nullable disable

namespace MovieRental.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250206114329_AddMovieCopiesAmount")]
    partial class AddMovieCopiesAmount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("MovieRental.Entity.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MovieRental.Entity.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AvailableCopies")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GenreId")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("ReleaseYear")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalCopies")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieRental.Entity.Rentals", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("RentalDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ReturnDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("UserId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("MovieRental.Entity.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MovieRental.Entity.Movie", b =>
                {
                    b.HasOne("MovieRental.Entity.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("MovieRental.Entity.Rentals", b =>
                {
                    b.HasOne("MovieRental.Entity.Movie", "Movie")
                        .WithMany("Rentals")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieRental.Entity.Users", "User")
                        .WithMany("Rentals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MovieRental.Entity.Genre", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("MovieRental.Entity.Movie", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("MovieRental.Entity.Users", b =>
                {
                    b.Navigation("Rentals");
                });
#pragma warning restore 612, 618
        }
    }
}
