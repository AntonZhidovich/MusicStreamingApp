﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubscriptionService.DataAccess.Data;

#nullable disable

namespace SubscriptionService.DataAccess.Migrations
{
    [DbContext(typeof(SubscriptionDbContext))]
    partial class SubscriptionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.1.24081.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SubscriptionService.DataAccess.Entities.Subscription", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Fee")
                        .HasColumnType("float");

                    b.Property<DateTime>("NextFeeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubscribedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("TariffPlanId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("TariffPlanId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("SubscriptionService.DataAccess.Entities.TariffPlan", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("AnnualFee")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("MaxPlaylistsCount")
                        .HasColumnType("int");

                    b.Property<double>("MonthFee")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TariffPlans");

                    b.HasData(
                        new
                        {
                            Id = "7dad33ee-b174-4c27-972e-ddd969145d52",
                            AnnualFee = 29.989999999999998,
                            Description = "A minimal plan for those who simply enjoy music.",
                            MaxPlaylistsCount = 3,
                            MonthFee = 2.9900000000000002,
                            Name = "Base"
                        },
                        new
                        {
                            Id = "83e7cb90-520c-45c2-b1f2-222974cb74c5",
                            AnnualFee = 35.990000000000002,
                            Description = "Tariff plan for multifaceted personality who sets the mood of the day with personal playlists.",
                            MaxPlaylistsCount = 7,
                            MonthFee = 3.9900000000000002,
                            Name = "Enhanced"
                        },
                        new
                        {
                            Id = "1ece6d0a-a08b-4839-a1c5-efe06496df64",
                            AnnualFee = 59.990000000000002,
                            Description = "Set playlists for every important moment of your life to share it with you favorite artists.",
                            MaxPlaylistsCount = 25,
                            MonthFee = 6.9900000000000002,
                            Name = "Push the boundaries"
                        });
                });

            modelBuilder.Entity("SubscriptionService.DataAccess.Entities.Subscription", b =>
                {
                    b.HasOne("SubscriptionService.DataAccess.Entities.TariffPlan", "TariffPlan")
                        .WithMany("Subscriptions")
                        .HasForeignKey("TariffPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TariffPlan");
                });

            modelBuilder.Entity("SubscriptionService.DataAccess.Entities.TariffPlan", b =>
                {
                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
