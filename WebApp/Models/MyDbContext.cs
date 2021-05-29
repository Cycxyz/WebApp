using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApp
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public virtual DbSet<BandToStyle> BandToStyle { get; set; }
        public virtual DbSet<Bands> Bands { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<ConcertAdresses> ConcertAdresses { get; set; }
        public virtual DbSet<ConcertManToBand> ConcertManToBand { get; set; }
        public virtual DbSet<ConcertMans> ConcertMans { get; set; }
        public virtual DbSet<ConcertToBand> ConcertToBand { get; set; }
        public virtual DbSet<Concerts> Concerts { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Styles> Styles { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=vladm23\\cycxyzserver; Database=MyDb; Trusted_Connection=true");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BandToStyle>(entity =>
            {
                entity.Property(e => e.BandToStyleId).HasColumnName("BandToStyleID");

                entity.Property(e => e.BandId).HasColumnName("BandID");

                entity.Property(e => e.StyleId).HasColumnName("StyleID");

                entity.HasOne(d => d.Band)
                    .WithMany(p => p.BandToStyle)
                    .HasForeignKey(d => d.BandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BandToSty__BandI__5CA1C101");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.BandToStyle)
                    .HasForeignKey(d => d.StyleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BandToSty__Style__5BAD9CC8");
            });

            modelBuilder.Entity<Bands>(entity =>
            {
                entity.HasKey(e => e.BandId)
                    .HasName("PK__Band__A03693887A440A94");

                entity.Property(e => e.BandId).HasColumnName("BandID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(e => e.CityId)
                    .HasName("PK__Cities__F2D21B76C8158D06");

                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Cities__737584F6CC5B3E0C")
                    .IsUnique();

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ConcertAdresses>(entity =>
            {
                entity.HasKey(e => e.ConcertAdressId)
                    .HasName("PK__ConcertA__1F6C1DC6DD15229B");

                entity.Property(e => e.ConcertAdressId).HasColumnName("ConcertAdressID");

                entity.Property(e => e.Adress).HasMaxLength(50);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Details).HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.ConcertAdresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ConcertAd__CityI__55009F39");
            });

            modelBuilder.Entity<ConcertManToBand>(entity =>
            {
                entity.Property(e => e.ConcertManToBandId).HasColumnName("ConcertManToBandID");

                entity.Property(e => e.BandId).HasColumnName("BandID");

                entity.Property(e => e.ConcertManId).HasColumnName("ConcertManID");

                entity.HasOne(d => d.Band)
                    .WithMany(p => p.ConcertManToBand)
                    .HasForeignKey(d => d.BandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ConcertMa__BandI__662B2B3B");

                entity.HasOne(d => d.ConcertMan)
                    .WithMany(p => p.ConcertManToBand)
                    .HasForeignKey(d => d.ConcertManId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConcertManToBand_ConcertMans");
            });

            modelBuilder.Entity<ConcertMans>(entity =>
            {
                entity.HasKey(e => e.ConcertManId)
                    .HasName("PK__ConcertM__1A699BD9BF99E286");

                entity.Property(e => e.ConcertManId).HasColumnName("ConcertManID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ConcertToBand>(entity =>
            {
                entity.Property(e => e.ConcertToBandId).HasColumnName("ConcertToBandID");

                entity.Property(e => e.BandId).HasColumnName("BandID");

                entity.Property(e => e.ConcertId).HasColumnName("ConcertID");

                entity.HasOne(d => d.Band)
                    .WithMany(p => p.ConcertToBand)
                    .HasForeignKey(d => d.BandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ConcertTo__BandI__58D1301D");

                entity.HasOne(d => d.Concert)
                    .WithMany(p => p.ConcertToBand)
                    .HasForeignKey(d => d.ConcertId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConcertTo_BandId_23");
            });

            modelBuilder.Entity<Concerts>(entity =>
            {
                entity.HasKey(e => e.ConcertId)
                    .HasName("PK__Concert__06ED384CB36B4EAD");

                entity.Property(e => e.ConcertId).HasColumnName("ConcertID");

                entity.Property(e => e.ConcertAdressId).HasColumnName("ConcertAdressID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.ConcertAdress)
                    .WithMany(p => p.Concerts)
                    .HasForeignKey(d => d.ConcertAdressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Concert__Concert__47DBAE45");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__Customer__A4AE64B884D0A235");

                entity.HasIndex(e => e.Phone)
                    .HasName("UQ__Customer__5C7E359E1C01B298")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsFixedLength();

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Styles>(entity =>
            {
                entity.HasKey(e => e.StyleId)
                    .HasName("PK__Style__8AD147A055410502");

                entity.HasIndex(e => e.Type)
                    .HasName("Ck")
                    .IsUnique();

                entity.Property(e => e.StyleId).HasColumnName("StyleID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Tickets>(entity =>
            {
                entity.HasKey(e => e.TicketId)
                    .HasName("PK__Ticket__712CC627F8FD3D79");

                entity.Property(e => e.TicketId).HasColumnName("TicketID");

                entity.Property(e => e.ConcertId).HasColumnName("ConcertID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Concert)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ConcertId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Tickets_Concerts_23");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Ticket__Customer__4BAC3F29");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
