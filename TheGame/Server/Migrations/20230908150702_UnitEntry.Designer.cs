﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheGame.Server.Data;

namespace TheGame.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230908150702_UnitEntry")]
    partial class UnitEntry
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("TheGame.Shared.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Attack")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Defence")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HitPoint")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("TheGame.Shared.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Battles")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Defeats")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<int>("TotalCosts")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<int>("Victories")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
