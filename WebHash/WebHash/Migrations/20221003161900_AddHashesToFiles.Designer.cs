﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebHash.DataModels;

namespace WebHash.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20221003161900_AddHashesToFiles")]
    partial class AddHashesToFiles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebHash.DataModels.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("WebHash.DataModels.Hash", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CrackingTime")
                        .HasColumnType("int");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Hashes");
                });

            modelBuilder.Entity("WebHash.DataModels.Hash", b =>
                {
                    b.HasOne("WebHash.DataModels.File", null)
                        .WithMany("Hashes")
                        .HasForeignKey("FileId");
                });

            modelBuilder.Entity("WebHash.DataModels.File", b =>
                {
                    b.Navigation("Hashes");
                });
#pragma warning restore 612, 618
        }
    }
}
