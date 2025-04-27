using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace basa20.Models;

public partial class ProizvodstvoContext : DbContext
{
    public ProizvodstvoContext()
    {
    }

    public ProizvodstvoContext(DbContextOptions<ProizvodstvoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Детали> Деталиs { get; set; }

    public virtual DbSet<Изделия> Изделияs { get; set; }

    public virtual DbSet<ПланВыпуска> ПланВыпускаs { get; set; }

    public virtual DbSet<СоставИзделия> СоставИзделияs { get; set; }

    public virtual DbSet<Цеха> Цехаs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LEITIDID;Database=proizvodstvo;Trusted_Connection=True; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Детали>(entity =>
        {
            entity.HasKey(e => e.КодДетали).HasName("PK__Детали__1130249F2723D68E");

            entity.ToTable("Детали");

            entity.Property(e => e.НаименованиеДетали).HasMaxLength(100);
            entity.Property(e => e.Цена).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Изделия>(entity =>
        {
            entity.HasKey(e => e.КодИзделия).HasName("PK__Изделия__4CCFC4A273D363AB");

            entity.ToTable("Изделия");

            entity.Property(e => e.НаименованиеИзделия).HasMaxLength(100);
            entity.Property(e => e.СтоимостьСборки).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<ПланВыпуска>(entity =>
        {
            entity.HasKey(e => e.КодПлана).HasName("PK__ПланВыпу__741F2DBF198C4F6A");

            entity.ToTable("ПланВыпуска");

            entity.HasOne(d => d.КодИзделияNavigation).WithMany(p => p.ПланВыпускаs)
                .HasForeignKey(d => d.КодИзделия)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ПланВыпус__КодИз__4316F928");

            entity.HasOne(d => d.КодЦехаNavigation).WithMany(p => p.ПланВыпускаs)
                .HasForeignKey(d => d.КодЦеха)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ПланВыпус__КодЦе__440B1D61");
        });

        modelBuilder.Entity<СоставИзделия>(entity =>
        {
            entity.HasKey(e => e.КодСостава).HasName("PK__СоставИз__383A29C6C012134C");

            entity.ToTable("СоставИзделия");

            entity.HasOne(d => d.КодДеталиNavigation).WithMany(p => p.СоставИзделияs)
                .HasForeignKey(d => d.КодДетали)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__СоставИзд__КодДе__47DBAE45");

            entity.HasOne(d => d.КодИзделияNavigation).WithMany(p => p.СоставИзделияs)
                .HasForeignKey(d => d.КодИзделия)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__СоставИзд__КодИз__46E78A0C");
        });

        modelBuilder.Entity<Цеха>(entity =>
        {
            entity.HasKey(e => e.КодЦеха).HasName("PK__Цеха__906E9A776A8880A0");

            entity.ToTable("Цеха");

            entity.Property(e => e.НаименованиеЦеха).HasMaxLength(100);
            entity.Property(e => e.Начальник).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
