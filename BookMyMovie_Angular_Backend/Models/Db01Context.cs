﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class Db01Context : DbContext
    {
        public Db01Context()
        {
        }

        public Db01Context(DbContextOptions<Db01Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Akbadmin> Akbadmins { get; set; }
        public virtual DbSet<Akbcustomer> Akbcustomers { get; set; }
        public virtual DbSet<Akbmovie> Akbmovies { get; set; }
        public virtual DbSet<AkbseatMap> AkbseatMaps { get; set; }
        public virtual DbSet<Akbtdet> Akbtdets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:cldazure.database.windows.net,1433;Initial Catalog=Db01;Persist Security Info=False;User ID=cldazure;Password=b@atch@12345!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Akbadmin>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK__AKBAdmin__719FE48811536A10");

                entity.ToTable("AKBAdmin");

                entity.HasIndex(e => e.Email, "UQ__AKBAdmin__A9D1053432BD368C")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Akbcustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__AKBCusto__A4AE64D8E2210672");

                entity.ToTable("AKBCustomer");

                entity.HasIndex(e => e.Email, "UQ__AKBCusto__A9D10534C47C9CEC")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Akbmovie>(entity =>
            {
                entity.HasKey(e => e.MovieId)
                    .HasName("PK__AKBMovie__4BD2941A632096FA");

                entity.ToTable("AKBMovie");

                entity.Property(e => e.AgeRating)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CostPerSeat).HasDefaultValueSql("((250))");

                entity.Property(e => e.Duration)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Genres)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Language)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MovieType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

                entity.Property(e => e.ShowTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<AkbseatMap>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.SeatNo })
                    .HasName("PK_SeatMap");

                entity.ToTable("AKBSeatMap");

                entity.Property(e => e.SeatNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.AkbseatMaps)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AKBSeatMa__Movie__415A6496");
            });

            modelBuilder.Entity<Akbtdet>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__AKBTDet__55433A6B82A5DF11");

                entity.ToTable("AKBTDet");

                entity.Property(e => e.TransactionTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Akbtdets)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__AKBTDet__Custome__452AF57A");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Akbtdets)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK__AKBTDet__MovieId__461F19B3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
