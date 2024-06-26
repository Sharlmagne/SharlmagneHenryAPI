﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharlmagneHenryAPI.Data;

#nullable disable

namespace SharlmagneHenryAPI.Data.Migrations
{
    [DbContext(typeof(DataContextEf))]
    [Migration("20240520230504_PortfolioSchemaInit")]
    partial class PortfolioSchemaInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("PortfolioSchema")
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Media", "PortfolioSchema");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Link")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("SkillId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("SkillId");

                    b.ToTable("Projects", "PortfolioSchema");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Skills", "PortfolioSchema");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Media", b =>
                {
                    b.HasOne("SharlmagneHenryAPI.Models.Project", "Project")
                        .WithMany("Media")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Project", b =>
                {
                    b.HasOne("SharlmagneHenryAPI.Models.Skill", null)
                        .WithMany("Projects")
                        .HasForeignKey("SkillId");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Skill", b =>
                {
                    b.HasOne("SharlmagneHenryAPI.Models.Skill", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Project", b =>
                {
                    b.Navigation("Media");
                });

            modelBuilder.Entity("SharlmagneHenryAPI.Models.Skill", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Projects");
                });
#pragma warning restore 612, 618
        }
    }
}
