﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using org.huage.EntityFramewok.Database.DBContext;

#nullable disable

namespace org.huage.EntityFramewok.Database.Migrations
{
    [DbContext(typeof(SchedulerDbContext))]
    [Migration("20231029144743_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("org.huage.EntityFramewok.Database.Table.Scheduler", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CronExpression")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("MethodName")
                        .IsUnique();

                    b.ToTable("Scheduler");
                });
#pragma warning restore 612, 618
        }
    }
}