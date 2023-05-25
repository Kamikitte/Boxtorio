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
    [Migration("20230523204425_DeliveryPoint")]
    partial class DeliveryPoint
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

            modelBuilder.Entity("Boxtorio.Data.Entities.AccountSession", b =>
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

                    b.ToTable("AccountSessions");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Box", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DeliveryPointId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryPointId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Box");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.DeliveryPoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DeliveryPoints");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DeliveryPointId")
                        .HasColumnType("uuid");

                    b.Property<int>("RackId")
                        .HasColumnType("integer");

                    b.Property<int>("SectionId")
                        .HasColumnType("integer");

                    b.Property<int>("ShelfId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryPointId");

                    b.ToTable("Place");
                });

            modelBuilder.Entity("DeliveryPointWorker", b =>
                {
                    b.Property<Guid>("DeliveryPointsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WorkersId")
                        .HasColumnType("uuid");

                    b.HasKey("DeliveryPointsId", "WorkersId");

                    b.HasIndex("WorkersId");

                    b.ToTable("DeliveryPointWorker");
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

            modelBuilder.Entity("Boxtorio.Data.Entities.AccountSession", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.Account", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Box", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.DeliveryPoint", null)
                        .WithMany("Boxes")
                        .HasForeignKey("DeliveryPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Boxtorio.Data.Entities.Place", null)
                        .WithMany("Boxes")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Place", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.DeliveryPoint", null)
                        .WithMany("Places")
                        .HasForeignKey("DeliveryPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DeliveryPointWorker", b =>
                {
                    b.HasOne("Boxtorio.Data.Entities.DeliveryPoint", null)
                        .WithMany()
                        .HasForeignKey("DeliveryPointsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Boxtorio.Data.Entities.Worker", null)
                        .WithMany()
                        .HasForeignKey("WorkersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Boxtorio.Data.Entities.DeliveryPoint", b =>
                {
                    b.Navigation("Boxes");

                    b.Navigation("Places");
                });

            modelBuilder.Entity("Boxtorio.Data.Entities.Place", b =>
                {
                    b.Navigation("Boxes");
                });
#pragma warning restore 612, 618
        }
    }
}
