using System;
using System.Collections.Generic;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Billiard.DAL.Data;

public partial class BilliardDbContext : DbContext
{
    public BilliardDbContext(DbContextOptions<BilliardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanBium> BanBia { get; set; }

    public virtual DbSet<BangLuong> BangLuongs { get; set; }

    public virtual DbSet<ChamCong> ChamCongs { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<ChucNang> ChucNangs { get; set; }

    public virtual DbSet<DatBan> DatBans { get; set; }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<GiaGioChoi> GiaGioChois { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhuVuc> KhuVucs { get; set; }

    public virtual DbSet<LichSuHoatDong> LichSuHoatDongs { get; set; }

    public virtual DbSet<LoaiBan> LoaiBans { get; set; }

    public virtual DbSet<MatHang> MatHangs { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhomQuyen> NhomQuyens { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<SoQuy> SoQuies { get; set; }

    public virtual DbSet<VBaoCaoVietQr> VBaoCaoVietQrs { get; set; }

    public virtual DbSet<VietqrConfig> VietqrConfigs { get; set; }

    public virtual DbSet<VietqrGiaoDich> VietqrGiaoDiches { get; set; }

    public virtual DbSet<VietqrWebhookLog> VietqrWebhookLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanBium>(entity =>
        {
            entity.HasKey(e => e.MaBan).HasName("PK__ban_bia__03FFDF0D93987EF7");

            entity.ToTable("ban_bia");

            entity.HasIndex(e => e.TenBan, "UQ__ban_bia__5B4758ACF530C070").IsUnique();

            entity.Property(e => e.MaBan).HasColumnName("ma_ban");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(255)
                .HasColumnName("ghi_chu");
            entity.Property(e => e.GioBatDau)
                .HasColumnType("datetime")
                .HasColumnName("gio_bat_dau");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("hinh_anh");
            entity.Property(e => e.MaKh).HasColumnName("ma_kh");
            entity.Property(e => e.MaKhuVuc).HasColumnName("ma_khu_vuc");
            entity.Property(e => e.MaLoai).HasColumnName("ma_loai");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.TenBan)
                .HasMaxLength(50)
                .HasColumnName("ten_ban");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Trống")
                .HasColumnName("trang_thai");
            entity.Property(e => e.ViTriX)
                .HasDefaultValue(0)
                .HasColumnName("vi_tri_x");
            entity.Property(e => e.ViTriY)
                .HasDefaultValue(0)
                .HasColumnName("vi_tri_y");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.BanBia)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__ban_bia__ma_kh__4F7CD00D");

            entity.HasOne(d => d.MaKhuVucNavigation).WithMany(p => p.BanBia)
                .HasForeignKey(d => d.MaKhuVuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ban_bia__ma_khu___5070F446");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.BanBia)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ban_bia__ma_loai__4E88ABD4");
        });

        modelBuilder.Entity<BangLuong>(entity =>
        {
            entity.HasKey(e => e.MaLuong).HasName("PK__bang_luo__CC15EB614C98417D");

            entity.ToTable("bang_luong");

            entity.Property(e => e.MaLuong).HasColumnName("ma_luong");
            entity.Property(e => e.LuongCoBan)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("luong_co_ban");
            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.Nam).HasColumnName("nam");
            entity.Property(e => e.NgayTinh)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tinh");
            entity.Property(e => e.Phat)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("phat");
            entity.Property(e => e.PhuCap)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("phu_cap");
            entity.Property(e => e.Thang).HasColumnName("thang");
            entity.Property(e => e.Thuong)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("thuong");
            entity.Property(e => e.TongGio)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("tong_gio");
            entity.Property(e => e.TongLuong)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("tong_luong");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.BangLuongs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__bang_luon__ma_nv__18EBB532");
        });

        modelBuilder.Entity<ChamCong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cham_con__3213E83FB9920DA0");

            entity.ToTable("cham_cong");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(255)
                .HasColumnName("ghi_chu");
            entity.Property(e => e.GioRa)
                .HasColumnType("datetime")
                .HasColumnName("gio_ra");
            entity.Property(e => e.GioVao)
                .HasColumnType("datetime")
                .HasColumnName("gio_vao");
            entity.Property(e => e.HinhAnhRa)
                .HasMaxLength(255)
                .HasColumnName("hinh_anh_ra");
            entity.Property(e => e.HinhAnhVao)
                .HasMaxLength(255)
                .HasColumnName("hinh_anh_vao");
            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.Ngay).HasColumnName("ngay");
            entity.Property(e => e.SoGioLam)
                .HasComputedColumnSql("(case when [gio_ra] IS NOT NULL then CONVERT([decimal](5,2),datediff(minute,[gio_vao],[gio_ra]))/(60.0) else (0) end)", true)
                .HasColumnType("numeric(10, 6)")
                .HasColumnName("so_gio_lam");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đúng giờ")
                .HasColumnName("trang_thai");
            entity.Property(e => e.XacThucBang)
                .HasMaxLength(20)
                .HasDefaultValue("Thủ công")
                .HasColumnName("xac_thuc_bang");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cham_cong__ma_nv__0F624AF8");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__chi_tiet__3213E83F2C775D02");

            entity.ToTable("chi_tiet_hoa_don", tb =>
                {
                    tb.HasTrigger("trg_CongLaiSoLuongTon_HuyMon");
                    tb.HasTrigger("trg_TruSoLuongTon_BanHang");
                });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaDv).HasColumnName("ma_dv");
            entity.Property(e => e.MaHd).HasColumnName("ma_hd");
            entity.Property(e => e.SoLuong)
                .HasDefaultValue(1)
                .HasColumnName("so_luong");
            entity.Property(e => e.ThanhTien)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("thanh_tien");

            entity.HasOne(d => d.MaDvNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaDv)
                .HasConstraintName("FK__chi_tiet___ma_dv__2A164134");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHd)
                .HasConstraintName("FK__chi_tiet___ma_hd__29221CFB");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__chi_tiet__3213E83F0CBC0803");

            entity.ToTable("chi_tiet_phieu_nhap", tb => tb.HasTrigger("trg_CongSoLuongTon_NhapKho"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DonGiaNhap)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("don_gia_nhap");
            entity.Property(e => e.MaHang).HasColumnName("ma_hang");
            entity.Property(e => e.MaPn).HasColumnName("ma_pn");
            entity.Property(e => e.SoLuongNhap).HasColumnName("so_luong_nhap");
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([so_luong_nhap]*[don_gia_nhap])", true)
                .HasColumnType("decimal(21, 0)")
                .HasColumnName("thanh_tien");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chi_tiet___ma_ha__7A672E12");

            entity.HasOne(d => d.MaPnNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaPn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__chi_tiet___ma_pn__797309D9");
        });

        modelBuilder.Entity<ChucNang>(entity =>
        {
            entity.HasKey(e => e.MaCn).HasName("PK__chuc_nan__0FE1769E06399444");

            entity.ToTable("chuc_nang");

            entity.Property(e => e.MaCn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ma_cn");
            entity.Property(e => e.MoTa)
                .HasMaxLength(255)
                .HasColumnName("mo_ta");
            entity.Property(e => e.TenCn)
                .HasMaxLength(100)
                .HasColumnName("ten_cn");
        });

        modelBuilder.Entity<DatBan>(entity =>
        {
            entity.HasKey(e => e.MaDat).HasName("PK__dat_ban__057BE11974124ACC");

            entity.ToTable("dat_ban");

            entity.HasIndex(e => new { e.ThoiGianBatDau, e.ThoiGianKetThuc }, "idx_dat_ban_thoi_gian");

            entity.Property(e => e.MaDat).HasColumnName("ma_dat");
            entity.Property(e => e.GhiChu).HasColumnName("ghi_chu");
            entity.Property(e => e.MaBan).HasColumnName("ma_ban");
            entity.Property(e => e.MaKh).HasColumnName("ma_kh");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("sdt");
            entity.Property(e => e.SoNguoi).HasColumnName("so_nguoi");
            entity.Property(e => e.TenKhach)
                .HasMaxLength(100)
                .HasColumnName("ten_khach");
            entity.Property(e => e.ThoiGianBatDau)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_bat_dau");
            entity.Property(e => e.ThoiGianDat)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_dat");
            entity.Property(e => e.ThoiGianKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_ket_thuc");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang chờ")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaBanNavigation).WithMany(p => p.DatBans)
                .HasForeignKey(d => d.MaBan)
                .HasConstraintName("FK__dat_ban__ma_ban__5629CD9C");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DatBans)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__dat_ban__ma_kh__571DF1D5");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.MaDv).HasName("PK__dich_vu__0FE14F358D88F7EC");

            entity.ToTable("dich_vu");

            entity.Property(e => e.MaDv).HasColumnName("ma_dv");
            entity.Property(e => e.DonVi)
                .HasMaxLength(50)
                .HasDefaultValue("phần")
                .HasColumnName("don_vi");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("gia");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("hinh_anh");
            entity.Property(e => e.Loai)
                .HasMaxLength(20)
                .HasDefaultValue("Khác")
                .HasColumnName("loai");
            entity.Property(e => e.MaHang).HasColumnName("ma_hang");
            entity.Property(e => e.MoTa)
                .HasMaxLength(255)
                .HasColumnName("mo_ta");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.TenDv)
                .HasMaxLength(100)
                .HasColumnName("ten_dv");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Còn hàng")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaHangNavigation).WithMany(p => p.DichVus)
                .HasForeignKey(d => d.MaHang)
                .HasConstraintName("FK__dich_vu__ma_hang__02FC7413");
        });

        modelBuilder.Entity<GiaGioChoi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__gia_gio___3213E83FA9F5FADD");

            entity.ToTable("gia_gio_choi");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApDungTuNgay).HasColumnName("ap_dung_tu_ngay");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("gia");
            entity.Property(e => e.GioBatDau).HasColumnName("gio_bat_dau");
            entity.Property(e => e.GioKetThuc).HasColumnName("gio_ket_thuc");
            entity.Property(e => e.KhungGio)
                .HasMaxLength(50)
                .HasColumnName("khung_gio");
            entity.Property(e => e.MaLoai).HasColumnName("ma_loai");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang áp dụng")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.GiaGioChois)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__gia_gio_c__ma_lo__3493CFA7");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__hoa_don__0FE16E8692E6CCF6");

            entity.ToTable("hoa_don");

            entity.Property(e => e.MaHd).HasColumnName("ma_hd");
            entity.Property(e => e.GhiChu).HasColumnName("ghi_chu");
            entity.Property(e => e.GhiChuGiamGia)
                .HasMaxLength(255)
                .HasColumnName("ghi_chu_giam_gia");
            entity.Property(e => e.GiamGia)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("giam_gia");
            entity.Property(e => e.MaBan).HasColumnName("ma_ban");
            entity.Property(e => e.MaGiaoDichQr)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ma_giao_dich_qr");
            entity.Property(e => e.MaKh).HasColumnName("ma_kh");
            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.PhuongThucThanhToan)
                .HasMaxLength(20)
                .HasDefaultValue("Tiền mặt")
                .HasColumnName("phuong_thuc_thanh_toan");
            entity.Property(e => e.QrCodeUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("qr_code_url");
            entity.Property(e => e.ThoiGianBatDau)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_bat_dau");
            entity.Property(e => e.ThoiGianKetThuc)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_ket_thuc");
            entity.Property(e => e.ThoiLuongPhut)
                .HasComputedColumnSql("(case when [thoi_gian_ket_thuc] IS NOT NULL then datediff(minute,[thoi_gian_bat_dau],[thoi_gian_ket_thuc]) else (0) end)", true)
                .HasColumnName("thoi_luong_phut");
            entity.Property(e => e.TienBan)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("tien_ban");
            entity.Property(e => e.TienDichVu)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("tien_dich_vu");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("tong_tien");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang chơi")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaBanNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaBan)
                .HasConstraintName("FK__hoa_don__ma_ban__236943A5");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__hoa_don__ma_kh__245D67DE");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__hoa_don__ma_nv__25518C17");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__khach_ha__0FE0B7EE42C6A3A1");

            entity.ToTable("khach_hang");

            entity.HasIndex(e => e.Sdt, "UQ__khach_ha__DDDFB4839B1180CA").IsUnique();

            entity.Property(e => e.MaKh).HasColumnName("ma_kh");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.DiemTichLuy)
                .HasDefaultValue(0)
                .HasColumnName("diem_tich_luy");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HangTv)
                .HasMaxLength(20)
                .HasDefaultValue("Đồng")
                .HasColumnName("hang_tv");
            entity.Property(e => e.HoatDong)
                .HasDefaultValue(false)
                .HasColumnName("hoat_dong");
            entity.Property(e => e.LanDenCuoi)
                .HasColumnType("datetime")
                .HasColumnName("lan_den_cuoi");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mat_khau");
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_dang_ky");
            entity.Property(e => e.NgaySinh).HasColumnName("ngay_sinh");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("sdt");
            entity.Property(e => e.TenKh)
                .HasMaxLength(100)
                .HasColumnName("ten_kh");
            entity.Property(e => e.TongChiTieu)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("tong_chi_tieu");
        });

        modelBuilder.Entity<KhuVuc>(entity =>
        {
            entity.HasKey(e => e.MaKhuVuc).HasName("PK__khu_vuc__6A5E05CD906921C3");

            entity.ToTable("khu_vuc");

            entity.HasIndex(e => e.TenKhuVuc, "UQ__khu_vuc__5A585CB0A8D242DE").IsUnique();

            entity.Property(e => e.MaKhuVuc).HasColumnName("ma_khu_vuc");
            entity.Property(e => e.MoTa)
                .HasMaxLength(255)
                .HasColumnName("mo_ta");
            entity.Property(e => e.TenKhuVuc)
                .HasMaxLength(50)
                .HasColumnName("ten_khu_vuc");
        });

        modelBuilder.Entity<LichSuHoatDong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__lich_su___3213E83F9E56791C");

            entity.ToTable("lich_su_hoat_dong");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChiTiet).HasColumnName("chi_tiet");
            entity.Property(e => e.HanhDong)
                .HasMaxLength(255)
                .HasColumnName("hanh_dong");
            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.LichSuHoatDongs)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK__lich_su_h__ma_nv__3864608B");
        });

        modelBuilder.Entity<LoaiBan>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__loai_ban__D9476E5789660C13");

            entity.ToTable("loai_ban");

            entity.HasIndex(e => e.TenLoai, "UQ__loai_ban__5FFB310CAD7B6805").IsUnique();

            entity.Property(e => e.MaLoai).HasColumnName("ma_loai");
            entity.Property(e => e.GiaGio)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("gia_gio");
            entity.Property(e => e.MoTa)
                .HasMaxLength(255)
                .HasColumnName("mo_ta");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(50)
                .HasColumnName("ten_loai");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang áp dụng")
                .HasColumnName("trang_thai");
        });

        modelBuilder.Entity<MatHang>(entity =>
        {
            entity.HasKey(e => e.MaHang).HasName("PK__mat_hang__6DE84043F6F63F26");

            entity.ToTable("mat_hang");

            entity.Property(e => e.MaHang).HasColumnName("ma_hang");
            entity.Property(e => e.DonVi)
                .HasMaxLength(50)
                .HasDefaultValue("cái")
                .HasColumnName("don_vi");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("gia");
            entity.Property(e => e.HinhAnh)
                .HasMaxLength(255)
                .HasColumnName("hinh_anh");
            entity.Property(e => e.Loai)
                .HasMaxLength(20)
                .HasDefaultValue("Khác")
                .HasColumnName("loai");
            entity.Property(e => e.MaNccDefault).HasColumnName("ma_ncc_default");
            entity.Property(e => e.MoTa)
                .HasMaxLength(255)
                .HasColumnName("mo_ta");
            entity.Property(e => e.NgayNhapGanNhat).HasColumnName("ngay_nhap_gan_nhat");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NguongCanhBao)
                .HasDefaultValue(10)
                .HasColumnName("nguong_canh_bao");
            entity.Property(e => e.SoLuongTon)
                .HasDefaultValue(0)
                .HasColumnName("so_luong_ton");
            entity.Property(e => e.TenHang)
                .HasMaxLength(100)
                .HasColumnName("ten_hang");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Còn hàng")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaNccDefaultNavigation).WithMany(p => p.MatHangs)
                .HasForeignKey(d => d.MaNccDefault)
                .HasConstraintName("FK__mat_hang__ma_ncc__6383C8BA");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__nha_cung__04FFEEB97AD2B3ED");

            entity.ToTable("nha_cung_cap");

            entity.Property(e => e.MaNcc).HasColumnName("ma_ncc");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .HasColumnName("dia_chi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("sdt");
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("ten_ncc");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__nhan_vie__0FE15F7CD9B29355");

            entity.ToTable("nhan_vien");

            entity.HasIndex(e => e.MaVanTay, "UQ__nhan_vie__C17D2329E67CBEE1").IsUnique();

            entity.HasIndex(e => e.FaceidHash, "UQ__nhan_vie__E37BF412AB415224").IsUnique();

            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.CaMacDinh)
                .HasMaxLength(10)
                .HasDefaultValue("Sáng")
                .HasColumnName("ca_mac_dinh");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FaceidAnh)
                .HasMaxLength(255)
                .HasColumnName("faceid_anh");
            entity.Property(e => e.FaceidHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("faceid_hash");
            entity.Property(e => e.LuongCoBan)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("luong_co_ban");
            entity.Property(e => e.MaNhom).HasColumnName("ma_nhom");
            entity.Property(e => e.MaVanTay)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ma_van_tay");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mat_khau");
            entity.Property(e => e.PhuCap)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("phu_cap");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("sdt");
            entity.Property(e => e.TenNv)
                .HasMaxLength(100)
                .HasColumnName("ten_nv");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang làm")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaNhomNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaNhom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__nhan_vien__ma_nh__70DDC3D8");
        });

        modelBuilder.Entity<NhomQuyen>(entity =>
        {
            entity.HasKey(e => e.MaNhom).HasName("PK__nhom_quy__9B8FD98CCE19E15A");

            entity.ToTable("nhom_quyen");

            entity.HasIndex(e => e.TenNhom, "UQ__nhom_quy__CA147C69D99B9308").IsUnique();

            entity.Property(e => e.MaNhom).HasColumnName("ma_nhom");
            entity.Property(e => e.TenNhom)
                .HasMaxLength(50)
                .HasColumnName("ten_nhom");

            entity.HasMany(d => d.MaCns).WithMany(p => p.MaNhoms)
                .UsingEntity<Dictionary<string, object>>(
                    "PhanQuyen",
                    r => r.HasOne<ChucNang>().WithMany()
                        .HasForeignKey("MaCn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__phan_quye__ma_cn__08B54D69"),
                    l => l.HasOne<NhomQuyen>().WithMany()
                        .HasForeignKey("MaNhom")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__phan_quye__ma_nh__07C12930"),
                    j =>
                    {
                        j.HasKey("MaNhom", "MaCn").HasName("PK__phan_quy__6B71CEE5E61FCB0B");
                        j.ToTable("phan_quyen");
                        j.IndexerProperty<int>("MaNhom").HasColumnName("ma_nhom");
                        j.IndexerProperty<string>("MaCn")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("ma_cn");
                    });
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPn).HasName("PK__phieu_nh__0FE0AFB6136FC471");

            entity.ToTable("phieu_nhap");

            entity.Property(e => e.MaPn).HasColumnName("ma_pn");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(255)
                .HasColumnName("ghi_chu");
            entity.Property(e => e.MaNcc).HasColumnName("ma_ncc");
            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_nhap");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("tong_tien");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNcc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__phieu_nha__ma_nc__76969D2E");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__phieu_nha__ma_nv__75A278F5");
        });

        modelBuilder.Entity<SoQuy>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__so_quy__11D0F07A4814454C");

            entity.ToTable("so_quy");

            entity.Property(e => e.MaPhieu).HasColumnName("ma_phieu");
            entity.Property(e => e.LoaiPhieu)
                .HasMaxLength(10)
                .HasColumnName("loai_phieu");
            entity.Property(e => e.LyDo)
                .HasMaxLength(500)
                .HasColumnName("ly_do");
            entity.Property(e => e.MaHdLienQuan).HasColumnName("ma_hd_lien_quan");
            entity.Property(e => e.MaNv).HasColumnName("ma_nv");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_lap");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("so_tien");

            entity.HasOne(d => d.MaHdLienQuanNavigation).WithMany(p => p.SoQuies)
                .HasForeignKey(d => d.MaHdLienQuan)
                .HasConstraintName("FK__so_quy__ma_hd_li__2FCF1A8A");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.SoQuies)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__so_quy__ma_nv__2EDAF651");
        });

        modelBuilder.Entity<VBaoCaoVietQr>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_BaoCaoVietQR");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaBan).HasColumnName("ma_ban");
            entity.Property(e => e.MaGiaoDich)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ma_giao_dich");
            entity.Property(e => e.MaGiaoDichNganHang)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ma_giao_dich_ngan_hang");
            entity.Property(e => e.MaHd).HasColumnName("ma_hd");
            entity.Property(e => e.NgayHetHan)
                .HasColumnType("datetime")
                .HasColumnName("ngay_het_han");
            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NoiDung)
                .HasMaxLength(255)
                .HasColumnName("noi_dung");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("so_tien");
            entity.Property(e => e.TenBan)
                .HasMaxLength(50)
                .HasColumnName("ten_ban");
            entity.Property(e => e.TenKh)
                .HasMaxLength(100)
                .HasColumnName("ten_kh");
            entity.Property(e => e.ThoiGianThanhToan)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_thanh_toan");
            entity.Property(e => e.ThoiGianThanhToanGiay).HasColumnName("thoi_gian_thanh_toan_giay");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasColumnName("trang_thai");
        });

        modelBuilder.Entity<VietqrConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vietqr_c__3213E83F5C8B373C");

            entity.ToTable("vietqr_config");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_no");
            entity.Property(e => e.ApiKey)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("api_key");
            entity.Property(e => e.ApiUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("api_url");
            entity.Property(e => e.BankId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("bank_id");
            entity.Property(e => e.ClientId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("client_id");
            entity.Property(e => e.LaMacDinh)
                .HasDefaultValue(false)
                .HasColumnName("la_mac_dinh");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_cap_nhat");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.Template)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("compact")
                .HasColumnName("template");
            entity.Property(e => e.TenCauHinh)
                .HasMaxLength(100)
                .HasColumnName("ten_cau_hinh");
            entity.Property(e => e.TrangThai)
                .HasDefaultValue(true)
                .HasColumnName("trang_thai");
            entity.Property(e => e.WebhookSecret)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("webhook_secret");
            entity.Property(e => e.WebhookUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("webhook_url");
        });

        modelBuilder.Entity<VietqrGiaoDich>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vietqr_g__3213E83FF917D349");

            entity.ToTable("vietqr_giao_dich");

            entity.HasIndex(e => e.MaGiaoDich, "UQ__vietqr_g__FB80ED332688027D").IsUnique();

            entity.HasIndex(e => e.MaGiaoDich, "idx_vietqr_giao_dich_ma_giao_dich");

            entity.HasIndex(e => e.MaHd, "idx_vietqr_giao_dich_ma_hd");

            entity.HasIndex(e => e.TrangThai, "idx_vietqr_giao_dich_trang_thai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_no");
            entity.Property(e => e.BankId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("bank_id");
            entity.Property(e => e.MaGiaoDich)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ma_giao_dich");
            entity.Property(e => e.MaGiaoDichNganHang)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ma_giao_dich_ngan_hang");
            entity.Property(e => e.MaHd).HasColumnName("ma_hd");
            entity.Property(e => e.NgayHetHan)
                .HasColumnType("datetime")
                .HasColumnName("ngay_het_han");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ngay_tao");
            entity.Property(e => e.NoiDung)
                .HasMaxLength(255)
                .HasColumnName("noi_dung");
            entity.Property(e => e.NoiDungThucTe)
                .HasMaxLength(255)
                .HasColumnName("noi_dung_thuc_te");
            entity.Property(e => e.QrCodeBase64).HasColumnName("qr_code_base64");
            entity.Property(e => e.QrCodeUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("qr_code_url");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(12, 0)")
                .HasColumnName("so_tien");
            entity.Property(e => e.ThoiGianThanhToan)
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_thanh_toan");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Chờ thanh toán")
                .HasColumnName("trang_thai");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.VietqrGiaoDiches)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vietqr_gi__ma_hd__3F115E1A");
        });

        modelBuilder.Entity<VietqrWebhookLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vietqr_w__3213E83FD9C75BB4");

            entity.ToTable("vietqr_webhook_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Headers).HasColumnName("headers");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ip_address");
            entity.Property(e => e.MaGiaoDich)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ma_giao_dich");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.ThoiGianNhan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("thoi_gian_nhan");
            entity.Property(e => e.ThongBaoLoi).HasColumnName("thong_bao_loi");
            entity.Property(e => e.XuLyThanhCong)
                .HasDefaultValue(false)
                .HasColumnName("xu_ly_thanh_cong");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
