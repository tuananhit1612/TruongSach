using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TruongSach_API.Models;

public partial class TruongSachContext : DbContext
{
    public TruongSachContext()
    {
    }

    public TruongSachContext(DbContextOptions<TruongSachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Baiviet> Baiviets { get; set; }

    public virtual DbSet<Binhluan> Binhluans { get; set; }

    public virtual DbSet<Chiendich> Chiendiches { get; set; }

    public virtual DbSet<Chitiethoadon> Chitiethoadons { get; set; }

    public virtual DbSet<Donggop> Donggops { get; set; }

    public virtual DbSet<Hinhanhbaiviet> Hinhanhbaiviets { get; set; }

    public virtual DbSet<Hinhanhchiendich> Hinhanhchiendiches { get; set; }

    public virtual DbSet<Hinhanhsanpham> Hinhanhsanphams { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Loaisanpham> Loaisanphams { get; set; }

    public virtual DbSet<Luotthich> Luotthiches { get; set; }

    public virtual DbSet<Nguoidung> Nguoidungs { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Truong> Truongs { get; set; }

    public virtual DbSet<Vaitro> Vaitros { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=TUANANHIT\\SQLEXPRESS;Initial Catalog=TruongSach;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Baiviet>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK__BAIVIET__AEDD56477BEB4E56");

            entity.ToTable("BAIVIET");

            entity.Property(e => e.HinhAnh).HasMaxLength(500);
            entity.Property(e => e.NgayDang).HasColumnType("smalldatetime");
            entity.Property(e => e.TieuDe).HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaTruongNavigation).WithMany(p => p.Baiviets)
                .HasForeignKey(d => d.MaTruong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BAIVIET__MaTruon__5EBF139D");
        });

        modelBuilder.Entity<Binhluan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BINHLUAN__87CB66A0E5FF54D0");

            entity.ToTable("BINHLUAN");

            entity.Property(e => e.NoiDung).HasMaxLength(500);
            entity.Property(e => e.ThoiGian).HasColumnType("smalldatetime");

            entity.HasOne(d => d.MaBaiVietNavigation).WithMany(p => p.Binhluans)
                .HasForeignKey(d => d.MaBaiViet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BINHLUAN__MaBaiV__5812160E");
        });

        modelBuilder.Entity<Chiendich>(entity =>
        {
            entity.HasKey(e => e.MaChienDich).HasName("PK__CHIENDIC__19681D4B2092A9E3");

            entity.ToTable("CHIENDICH");

            entity.Property(e => e.HinhAnhDaiDien).HasMaxLength(500);
            entity.Property(e => e.MucTieuQuyenGop).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NgayTao).HasColumnType("smalldatetime");
            entity.Property(e => e.NguoiDaiDien).HasMaxLength(50);
            entity.Property(e => e.SoTienHienTai)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("SoTienHienTai ");
            entity.Property(e => e.TieuDe).HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaTruongNavigation).WithMany(p => p.Chiendiches)
                .HasForeignKey(d => d.MaTruong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHIENDICH__MaTru__5DCAEF64");
        });

        modelBuilder.Entity<Chitiethoadon>(entity =>
        {
            entity.HasKey(e => new { e.MaSanPham, e.MaHoaDon }).HasName("PK__CHITIETH__52F2A93E0B523AAF");

            entity.ToTable("CHITIETHOADON");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHO__MaHoa__5CD6CB2B");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETHO__MaSan__5629CD9C");
        });

        modelBuilder.Entity<Donggop>(entity =>
        {
            entity.HasKey(e => e.MaDongGop).HasName("PK__DONGGOP__8CE1C5621A82EAD3");

            entity.ToTable("DONGGOP", tb => tb.HasTrigger("TRG_Update_Chiendich_After_DongGop"));

            entity.Property(e => e.HinhThuc).HasMaxLength(20);
            entity.Property(e => e.SoTien).HasMaxLength(50);

            entity.HasOne(d => d.MaChienDichNavigation).WithMany(p => p.Donggops)
                .HasForeignKey(d => d.MaChienDich)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONGGOP__MaChien__5BE2A6F2");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Donggops)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONGGOP__MaNguoi__52593CB8");
        });

        modelBuilder.Entity<Hinhanhbaiviet>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HINHANHB__A9C37A9BC9915FEC");

            entity.ToTable("HINHANHBAIVIET");

            entity.HasOne(d => d.MaBaiVietNavigation).WithMany(p => p.Hinhanhbaiviets)
                .HasForeignKey(d => d.MaBaiViet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HINHANHBA__MaBai__571DF1D5");
        });

        modelBuilder.Entity<Hinhanhchiendich>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HINHANHC__A9C37A9B4A0050DF");

            entity.ToTable("HINHANHCHIENDICH");

            entity.HasOne(d => d.MaChienDichNavigation).WithMany(p => p.Hinhanhchiendiches)
                .HasForeignKey(d => d.MaChienDich)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HINHANHCH__MaChi__5AEE82B9");
        });

        modelBuilder.Entity<Hinhanhsanpham>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HINHANHS__A9C37A9BD028FFBF");

            entity.ToTable("HINHANHSANPHAM");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.Hinhanhsanphams)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HINHANHSA__MaSan__5535A963");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HOADON__835ED13B4B7362CD");

            entity.ToTable("HOADON");

            entity.Property(e => e.HinhThucThanhToan).HasMaxLength(50);
            entity.Property(e => e.NgayLap).HasColumnType("smalldatetime");
            entity.Property(e => e.TongTien).HasColumnType("money");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HOADON__MaNguoiD__534D60F1");
        });

        modelBuilder.Entity<Loaisanpham>(entity =>
        {
            entity.HasKey(e => e.MaLoaiSanPham).HasName("PK__LOAISANP__ECCF699F93F18BC3");

            entity.ToTable("LOAISANPHAM");

            entity.Property(e => e.TenLoaiSanPham).HasMaxLength(20);
        });

        modelBuilder.Entity<Luotthich>(entity =>
        {
            entity.HasKey(e => e.MaLuotThich).HasName("PK__LUOTTHIC__CF7B01A459E56257");

            entity.ToTable("LUOTTHICH");

            entity.Property(e => e.ThoiGian).HasColumnType("smalldatetime");

            entity.HasOne(d => d.MaBaiVietNavigation).WithMany(p => p.Luotthiches)
                .HasForeignKey(d => d.MaBaiViet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LUOTTHICH__MaBai__59063A47");
        });

        modelBuilder.Entity<Nguoidung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NGUOIDUN__C539D7620FAE7638");

            entity.ToTable("NGUOIDUNG");

            entity.Property(e => e.DiemThuong).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GoogleId).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.NgayDangKy).HasColumnType("smalldatetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.Nguoidungs)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NGUOIDUNG__MaVai__5441852A");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SANPHAM__FAC7442D27625D6C");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenSanPham).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaLoaiSanPhamNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaLoaiSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM__MaLoaiS__59FA5E80");
        });

        modelBuilder.Entity<Truong>(entity =>
        {
            entity.HasKey(e => e.MaTruong).HasName("PK__TRUONG__5ECEF88A35C9BA1B");

            entity.ToTable("TRUONG");

            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.GhiChu).HasMaxLength(50);
            entity.Property(e => e.KhuVuc).HasMaxLength(50);
            entity.Property(e => e.KinhDo).HasMaxLength(50);
            entity.Property(e => e.TenTruong).HasMaxLength(50);
            entity.Property(e => e.ViDo).HasMaxLength(50);
        });

        modelBuilder.Entity<Vaitro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VAITRO__C24C41CF3A7EF38B");

            entity.ToTable("VAITRO");

            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenVaiTro).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
