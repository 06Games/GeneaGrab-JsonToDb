﻿// <auto-generated />
using System;
using GeneaGrab_JsonToDb.New;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeneaGrab.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20231231161410_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("GeneaGrab.Core.Models.Frame", b =>
                {
                    b.Property<string>("ProviderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistryId")
                        .HasColumnType("TEXT");

                    b.Property<int>("FrameNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ArkUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("DownloadUrl")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageSize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TileSize")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProviderId", "RegistryId", "FrameNumber");

                    b.ToTable("Frames");
                });

            modelBuilder.Entity("GeneaGrab.Core.Models.Registry", b =>
                {
                    b.Property<string>("ProviderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ArkURL")
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<string>("CallNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subtitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Types")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("URL")
                        .HasColumnType("TEXT");

                    b.HasKey("ProviderId", "Id");

                    b.ToTable("Registries");
                });

            modelBuilder.Entity("GeneaGrab.Models.Indexing.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Age")
                        .HasColumnType("TEXT");

                    b.Property<string>("CivilStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlaceOrigin")
                        .HasColumnType("TEXT");

                    b.Property<int>("RecordId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Relation")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Sex")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RecordId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("GeneaGrab.Models.Indexing.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ArkUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("District")
                        .HasColumnType("TEXT");

                    b.Property<int>("FrameNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("PageNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Parish")
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistryId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistryProviderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Road")
                        .HasColumnType("TEXT");

                    b.Property<string>("SequenceNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RegistryProviderId", "RegistryId");

                    b.HasIndex("ProviderId", "RegistryId", "FrameNumber");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("GeneaGrab.Core.Models.Frame", b =>
                {
                    b.HasOne("GeneaGrab.Core.Models.Registry", "Registry")
                        .WithMany("Frames")
                        .HasForeignKey("ProviderId", "RegistryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Registry");
                });

            modelBuilder.Entity("GeneaGrab.Models.Indexing.Person", b =>
                {
                    b.HasOne("GeneaGrab.Models.Indexing.Record", "Record")
                        .WithMany("Persons")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("GeneaGrab.Models.Indexing.Record", b =>
                {
                    b.HasOne("GeneaGrab.Core.Models.Registry", "Registry")
                        .WithMany()
                        .HasForeignKey("RegistryProviderId", "RegistryId");

                    b.HasOne("GeneaGrab.Core.Models.Frame", "Frame")
                        .WithMany()
                        .HasForeignKey("ProviderId", "RegistryId", "FrameNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Frame");

                    b.Navigation("Registry");
                });

            modelBuilder.Entity("GeneaGrab.Core.Models.Registry", b =>
                {
                    b.Navigation("Frames");
                });

            modelBuilder.Entity("GeneaGrab.Models.Indexing.Record", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
