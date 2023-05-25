﻿// <auto-generated />
using System;
using Boxtorio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Boxtorio.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230522225633_AccountRoles")]
    partial class AccountRoles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Boxtorio.Data.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.UserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RefreshToken")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Admin", b =>
                {
                    b.HasBaseType("Boxtorio.Data.Entities.Account");

                    b.ToTable("Admins", (string)null);
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Worker", b =>
                {
                    b.HasBaseType("Boxtorio.Data.Entities.Account");

                    b.ToTable("Workers", (string)null);
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.UserSession", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.Account", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Admin", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.Account", null)
                        .WithOne()
                        .HasForeignKey("Boxtorio.Data.Entities.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Worker", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.Account", null)
                        .WithOne()
                        .HasForeignKey("Boxtorio.Data.Entities.Worker", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Account", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}