using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billiard.DAL.Entities
{
    [Table("lich_lam_viec")]
    public class LichLamViec
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ma_nv")]
        public int MaNv { get; set; }

        [Required]
        [Column("ngay")]
        public DateOnly Ngay { get; set; }

        [Required]
        [Column("gio_bat_dau")]
        public TimeOnly GioBatDau { get; set; }

        [Required]
        [Column("gio_ket_thuc")]
        public TimeOnly GioKetThuc { get; set; }

        [Required]
        [Column("ca")]
        [MaxLength(20)]
        public string Ca { get; set; } = "Sang";

        [Column("trang_thai")]
        [MaxLength(20)]
        public string TrangThai { get; set; } = "DaXepLich";

        [Column("ghi_chu")]
        [MaxLength(255)]
        public string? GhiChu { get; set; }

        [Column("ngay_tao")]
        public DateTime? NgayTao { get; set; }

        [Column("nguoi_tao")]
        public int? NguoiTao { get; set; }

        [Column("ngay_cap_nhat")]
        public DateTime? NgayCapNhat { get; set; }

        [Column("nguoi_cap_nhat")]
        public int? NguoiCapNhat { get; set; }

        // Navigation Properties
        [ForeignKey("MaNv")]
        public virtual NhanVien? NhanVien { get; set; }

        [ForeignKey("NguoiTao")]
        public virtual NhanVien? NguoiTaoNavigation { get; set; }

        [ForeignKey("NguoiCapNhat")]
        public virtual NhanVien? NguoiCapNhatNavigation { get; set; }
    }
}