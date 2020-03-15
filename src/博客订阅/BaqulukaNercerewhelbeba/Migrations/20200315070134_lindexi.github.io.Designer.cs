﻿// <auto-generated />
using System;
using BaqulukaNercerewhelbeba.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BaqulukaNercerewhelbeba.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20200315070134_lindexi.github.io")]
    partial class lindexigithubio
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("BaqulukaNercerewhelbeba.Model.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BlogRss")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServerUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BlogRss");

                    b.ToTable("Blog");
                });

            modelBuilder.Entity("BaqulukaNercerewhelbeba.Model.PublishedBlog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Blog")
                        .HasColumnType("TEXT");

                    b.Property<string>("MatterMost")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Blog", "MatterMost");

                    b.ToTable("PublishedBlogList");
                });
#pragma warning restore 612, 618
        }
    }
}
