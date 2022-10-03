using System;
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
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__AKBAdmin__B611CB7D59EB8C0B");

                entity.ToTable("AKBAdmin");

                entity.HasIndex(e => e.Email, "UQ__AKBAdmin__AB6E6164BD81E4E1")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Akbcustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__AKBCusto__B611CB7D74A0B4AD");

                entity.ToTable("AKBCustomer");

                entity.HasIndex(e => e.Email, "UQ__AKBCusto__AB6E6164BFC32537")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Akbmovie>(entity =>
            {
                entity.HasKey(e => e.MovieId)
                    .HasName("PK__AKBMovie__42EB374EFB8B72C3");

                entity.ToTable("AKBMovie");

                entity.Property(e => e.MovieId).HasColumnName("movieId");

                entity.Property(e => e.AgeRating)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("ageRating");

                entity.Property(e => e.CostPerSeat)
                    .HasColumnName("costPerSeat")
                    .HasDefaultValueSql("((250))");

                entity.Property(e => e.Duration)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("duration");

                entity.Property(e => e.Genres)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("genres");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("imageUrl");

                entity.Property(e => e.Language)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("language");

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("movieName");

                entity.Property(e => e.MovieType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("movieType");

                entity.Property(e => e.Ratings).HasColumnName("ratings");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("releaseDate");

                entity.Property(e => e.ShowTime)
                    .HasColumnType("datetime")
                    .HasColumnName("showTime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
