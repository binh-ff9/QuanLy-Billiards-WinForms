using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billiard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chuc_nang",
                columns: table => new
                {
                    ma_cn = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ten_cn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__chuc_nan__0FE1769E06399444", x => x.ma_cn);
                });

            migrationBuilder.CreateTable(
                name: "khach_hang",
                columns: table => new
                {
                    ma_kh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_kh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sdt = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    mat_khau = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ngay_sinh = table.Column<DateOnly>(type: "date", nullable: true),
                    hang_tv = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đồng"),
                    diem_tich_luy = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    tong_chi_tieu = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    ngay_dang_ky = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    lan_den_cuoi = table.Column<DateTime>(type: "datetime", nullable: true),
                    hoat_dong = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__khach_ha__0FE0B7EE42C6A3A1", x => x.ma_kh);
                });

            migrationBuilder.CreateTable(
                name: "khu_vuc",
                columns: table => new
                {
                    ma_khu_vuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_khu_vuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__khu_vuc__6A5E05CD906921C3", x => x.ma_khu_vuc);
                });

            migrationBuilder.CreateTable(
                name: "loai_ban",
                columns: table => new
                {
                    ma_loai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    gia_gio = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đang áp dụng")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__loai_ban__D9476E5789660C13", x => x.ma_loai);
                });

            migrationBuilder.CreateTable(
                name: "nha_cung_cap",
                columns: table => new
                {
                    ma_ncc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_ncc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sdt = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    dia_chi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__nha_cung__04FFEEB97AD2B3ED", x => x.ma_ncc);
                });

            migrationBuilder.CreateTable(
                name: "nhom_quyen",
                columns: table => new
                {
                    ma_nhom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_nhom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__nhom_quy__9B8FD98CCE19E15A", x => x.ma_nhom);
                });

            migrationBuilder.CreateTable(
                name: "vietqr_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_cau_hinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    bank_id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    account_no = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    account_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    api_url = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    client_id = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    api_key = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    template = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "compact"),
                    webhook_url = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    webhook_secret = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    trang_thai = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    la_mac_dinh = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__vietqr_c__3213E83F5C8B373C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vietqr_webhook_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_giao_dich = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip_address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    xu_ly_thanh_cong = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    thong_bao_loi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thoi_gian_nhan = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__vietqr_w__3213E83FD9C75BB4", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ban_bia",
                columns: table => new
                {
                    ma_ban = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_ban = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ma_loai = table.Column<int>(type: "int", nullable: false),
                    ma_khu_vuc = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Trống"),
                    gio_bat_dau = table.Column<DateTime>(type: "datetime", nullable: true),
                    ma_kh = table.Column<int>(type: "int", nullable: true),
                    vi_tri_x = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    vi_tri_y = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    hinh_anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ban_bia__03FFDF0D93987EF7", x => x.ma_ban);
                    table.ForeignKey(
                        name: "FK__ban_bia__ma_kh__4F7CD00D",
                        column: x => x.ma_kh,
                        principalTable: "khach_hang",
                        principalColumn: "ma_kh");
                    table.ForeignKey(
                        name: "FK__ban_bia__ma_khu___5070F446",
                        column: x => x.ma_khu_vuc,
                        principalTable: "khu_vuc",
                        principalColumn: "ma_khu_vuc");
                    table.ForeignKey(
                        name: "FK__ban_bia__ma_loai__4E88ABD4",
                        column: x => x.ma_loai,
                        principalTable: "loai_ban",
                        principalColumn: "ma_loai");
                });

            migrationBuilder.CreateTable(
                name: "gia_gio_choi",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    khung_gio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gio_bat_dau = table.Column<TimeOnly>(type: "time", nullable: false),
                    gio_ket_thuc = table.Column<TimeOnly>(type: "time", nullable: false),
                    ma_loai = table.Column<int>(type: "int", nullable: true),
                    gia = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    ap_dung_tu_ngay = table.Column<DateOnly>(type: "date", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đang áp dụng")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__gia_gio___3213E83FA9F5FADD", x => x.id);
                    table.ForeignKey(
                        name: "FK__gia_gio_c__ma_lo__3493CFA7",
                        column: x => x.ma_loai,
                        principalTable: "loai_ban",
                        principalColumn: "ma_loai");
                });

            migrationBuilder.CreateTable(
                name: "mat_hang",
                columns: table => new
                {
                    ma_hang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_hang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Khác"),
                    don_vi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "cái"),
                    gia = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    so_luong_ton = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    nguong_canh_bao = table.Column<int>(type: "int", nullable: true, defaultValue: 10),
                    ma_ncc_default = table.Column<int>(type: "int", nullable: true),
                    ngay_nhap_gan_nhat = table.Column<DateOnly>(type: "date", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Còn hàng"),
                    mo_ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    hinh_anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__mat_hang__6DE84043F6F63F26", x => x.ma_hang);
                    table.ForeignKey(
                        name: "FK__mat_hang__ma_ncc__6383C8BA",
                        column: x => x.ma_ncc_default,
                        principalTable: "nha_cung_cap",
                        principalColumn: "ma_ncc");
                });

            migrationBuilder.CreateTable(
                name: "nhan_vien",
                columns: table => new
                {
                    ma_nv = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_nv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ma_van_tay = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    faceid_hash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    faceid_anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ma_nhom = table.Column<int>(type: "int", nullable: false),
                    sdt = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    luong_co_ban = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    phu_cap = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    ca_mac_dinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "Sáng"),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đang làm"),
                    mat_khau = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__nhan_vie__0FE15F7CD9B29355", x => x.ma_nv);
                    table.ForeignKey(
                        name: "FK__nhan_vien__ma_nh__70DDC3D8",
                        column: x => x.ma_nhom,
                        principalTable: "nhom_quyen",
                        principalColumn: "ma_nhom");
                });

            migrationBuilder.CreateTable(
                name: "phan_quyen",
                columns: table => new
                {
                    ma_nhom = table.Column<int>(type: "int", nullable: false),
                    ma_cn = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__phan_quy__6B71CEE5E61FCB0B", x => new { x.ma_nhom, x.ma_cn });
                    table.ForeignKey(
                        name: "FK__phan_quye__ma_cn__08B54D69",
                        column: x => x.ma_cn,
                        principalTable: "chuc_nang",
                        principalColumn: "ma_cn");
                    table.ForeignKey(
                        name: "FK__phan_quye__ma_nh__07C12930",
                        column: x => x.ma_nhom,
                        principalTable: "nhom_quyen",
                        principalColumn: "ma_nhom");
                });

            migrationBuilder.CreateTable(
                name: "dat_ban",
                columns: table => new
                {
                    ma_dat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_ban = table.Column<int>(type: "int", nullable: true),
                    ma_kh = table.Column<int>(type: "int", nullable: true),
                    ten_khach = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sdt = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    thoi_gian_dat = table.Column<DateTime>(type: "datetime", nullable: false),
                    so_nguoi = table.Column<int>(type: "int", nullable: true),
                    ghi_chu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đang chờ"),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    thoi_gian_bat_dau = table.Column<DateTime>(type: "datetime", nullable: true),
                    thoi_gian_ket_thuc = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__dat_ban__057BE11974124ACC", x => x.ma_dat);
                    table.ForeignKey(
                        name: "FK__dat_ban__ma_ban__5629CD9C",
                        column: x => x.ma_ban,
                        principalTable: "ban_bia",
                        principalColumn: "ma_ban");
                    table.ForeignKey(
                        name: "FK__dat_ban__ma_kh__571DF1D5",
                        column: x => x.ma_kh,
                        principalTable: "khach_hang",
                        principalColumn: "ma_kh");
                });

            migrationBuilder.CreateTable(
                name: "dich_vu",
                columns: table => new
                {
                    ma_dv = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_dv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Khác"),
                    gia = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    don_vi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "phần"),
                    ma_hang = table.Column<int>(type: "int", nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Còn hàng"),
                    mo_ta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    hinh_anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__dich_vu__0FE14F358D88F7EC", x => x.ma_dv);
                    table.ForeignKey(
                        name: "FK__dich_vu__ma_hang__02FC7413",
                        column: x => x.ma_hang,
                        principalTable: "mat_hang",
                        principalColumn: "ma_hang");
                });

            migrationBuilder.CreateTable(
                name: "bang_luong",
                columns: table => new
                {
                    ma_luong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nv = table.Column<int>(type: "int", nullable: false),
                    thang = table.Column<int>(type: "int", nullable: false),
                    nam = table.Column<int>(type: "int", nullable: false),
                    tong_gio = table.Column<decimal>(type: "decimal(8,2)", nullable: true, defaultValue: 0m),
                    luong_co_ban = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    phu_cap = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    thuong = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    phat = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    tong_luong = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    ngay_tinh = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__bang_luo__CC15EB614C98417D", x => x.ma_luong);
                    table.ForeignKey(
                        name: "FK__bang_luon__ma_nv__18EBB532",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                });

            migrationBuilder.CreateTable(
                name: "cham_cong",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nv = table.Column<int>(type: "int", nullable: false),
                    ngay = table.Column<DateOnly>(type: "date", nullable: false),
                    gio_vao = table.Column<DateTime>(type: "datetime", nullable: true),
                    gio_ra = table.Column<DateTime>(type: "datetime", nullable: true),
                    hinh_anh_vao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    hinh_anh_ra = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    xac_thuc_bang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Thủ công"),
                    so_gio_lam = table.Column<decimal>(type: "numeric(10,6)", nullable: true, computedColumnSql: "(case when [gio_ra] IS NOT NULL then CONVERT([decimal](5,2),datediff(minute,[gio_vao],[gio_ra]))/(60.0) else (0) end)", stored: true),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đúng giờ"),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cham_con__3213E83FB9920DA0", x => x.id);
                    table.ForeignKey(
                        name: "FK__cham_cong__ma_nv__0F624AF8",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                });

            migrationBuilder.CreateTable(
                name: "hoa_don",
                columns: table => new
                {
                    ma_hd = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_ban = table.Column<int>(type: "int", nullable: true),
                    ma_kh = table.Column<int>(type: "int", nullable: true),
                    ma_nv = table.Column<int>(type: "int", nullable: true),
                    thoi_gian_bat_dau = table.Column<DateTime>(type: "datetime", nullable: true),
                    thoi_gian_ket_thuc = table.Column<DateTime>(type: "datetime", nullable: true),
                    thoi_luong_phut = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(case when [thoi_gian_ket_thuc] IS NOT NULL then datediff(minute,[thoi_gian_bat_dau],[thoi_gian_ket_thuc]) else (0) end)", stored: true),
                    tien_ban = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    tien_dich_vu = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    giam_gia = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    ghi_chu_giam_gia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    tong_tien = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    phuong_thuc_thanh_toan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Tiền mặt"),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Đang chơi"),
                    ma_giao_dich_qr = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    qr_code_url = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    ghi_chu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__hoa_don__0FE16E8692E6CCF6", x => x.ma_hd);
                    table.ForeignKey(
                        name: "FK__hoa_don__ma_ban__236943A5",
                        column: x => x.ma_ban,
                        principalTable: "ban_bia",
                        principalColumn: "ma_ban");
                    table.ForeignKey(
                        name: "FK__hoa_don__ma_kh__245D67DE",
                        column: x => x.ma_kh,
                        principalTable: "khach_hang",
                        principalColumn: "ma_kh");
                    table.ForeignKey(
                        name: "FK__hoa_don__ma_nv__25518C17",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                });

            migrationBuilder.CreateTable(
                name: "lich_su_hoat_dong",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thoi_gian = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ma_nv = table.Column<int>(type: "int", nullable: true),
                    hanh_dong = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    chi_tiet = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__lich_su___3213E83F9E56791C", x => x.id);
                    table.ForeignKey(
                        name: "FK__lich_su_h__ma_nv__3864608B",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                });

            migrationBuilder.CreateTable(
                name: "phieu_nhap",
                columns: table => new
                {
                    ma_pn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nv = table.Column<int>(type: "int", nullable: false),
                    ma_ncc = table.Column<int>(type: "int", nullable: false),
                    ngay_nhap = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    tong_tien = table.Column<decimal>(type: "decimal(12,0)", nullable: true, defaultValue: 0m),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__phieu_nh__0FE0AFB6136FC471", x => x.ma_pn);
                    table.ForeignKey(
                        name: "FK__phieu_nha__ma_nc__76969D2E",
                        column: x => x.ma_ncc,
                        principalTable: "nha_cung_cap",
                        principalColumn: "ma_ncc");
                    table.ForeignKey(
                        name: "FK__phieu_nha__ma_nv__75A278F5",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_hoa_don",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_hd = table.Column<int>(type: "int", nullable: true),
                    ma_dv = table.Column<int>(type: "int", nullable: true),
                    so_luong = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    thanh_tien = table.Column<decimal>(type: "decimal(12,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__chi_tiet__3213E83F2C775D02", x => x.id);
                    table.ForeignKey(
                        name: "FK__chi_tiet___ma_dv__2A164134",
                        column: x => x.ma_dv,
                        principalTable: "dich_vu",
                        principalColumn: "ma_dv");
                    table.ForeignKey(
                        name: "FK__chi_tiet___ma_hd__29221CFB",
                        column: x => x.ma_hd,
                        principalTable: "hoa_don",
                        principalColumn: "ma_hd");
                });

            migrationBuilder.CreateTable(
                name: "so_quy",
                columns: table => new
                {
                    ma_phieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nv = table.Column<int>(type: "int", nullable: false),
                    ngay_lap = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    loai_phieu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    so_tien = table.Column<decimal>(type: "decimal(12,0)", nullable: false),
                    ly_do = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ma_hd_lien_quan = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__so_quy__11D0F07A4814454C", x => x.ma_phieu);
                    table.ForeignKey(
                        name: "FK__so_quy__ma_hd_li__2FCF1A8A",
                        column: x => x.ma_hd_lien_quan,
                        principalTable: "hoa_don",
                        principalColumn: "ma_hd");
                    table.ForeignKey(
                        name: "FK__so_quy__ma_nv__2EDAF651",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                });

            migrationBuilder.CreateTable(
                name: "vietqr_giao_dich",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_hd = table.Column<int>(type: "int", nullable: false),
                    ma_giao_dich = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    qr_code_url = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    qr_code_base64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    so_tien = table.Column<decimal>(type: "decimal(12,0)", nullable: false),
                    noi_dung = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bank_id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    account_no = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    account_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Chờ thanh toán"),
                    ma_giao_dich_ngan_hang = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    thoi_gian_thanh_toan = table.Column<DateTime>(type: "datetime", nullable: true),
                    noi_dung_thuc_te = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ngay_het_han = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__vietqr_g__3213E83FF917D349", x => x.id);
                    table.ForeignKey(
                        name: "FK__vietqr_gi__ma_hd__3F115E1A",
                        column: x => x.ma_hd,
                        principalTable: "hoa_don",
                        principalColumn: "ma_hd");
                });

            migrationBuilder.CreateTable(
                name: "chi_tiet_phieu_nhap",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_pn = table.Column<int>(type: "int", nullable: false),
                    ma_hang = table.Column<int>(type: "int", nullable: false),
                    so_luong_nhap = table.Column<int>(type: "int", nullable: false),
                    don_gia_nhap = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    thanh_tien = table.Column<decimal>(type: "decimal(21,0)", nullable: true, computedColumnSql: "([so_luong_nhap]*[don_gia_nhap])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__chi_tiet__3213E83F0CBC0803", x => x.id);
                    table.ForeignKey(
                        name: "FK__chi_tiet___ma_ha__7A672E12",
                        column: x => x.ma_hang,
                        principalTable: "mat_hang",
                        principalColumn: "ma_hang");
                    table.ForeignKey(
                        name: "FK__chi_tiet___ma_pn__797309D9",
                        column: x => x.ma_pn,
                        principalTable: "phieu_nhap",
                        principalColumn: "ma_pn");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ban_bia_ma_kh",
                table: "ban_bia",
                column: "ma_kh");

            migrationBuilder.CreateIndex(
                name: "IX_ban_bia_ma_khu_vuc",
                table: "ban_bia",
                column: "ma_khu_vuc");

            migrationBuilder.CreateIndex(
                name: "IX_ban_bia_ma_loai",
                table: "ban_bia",
                column: "ma_loai");

            migrationBuilder.CreateIndex(
                name: "UQ__ban_bia__5B4758ACF530C070",
                table: "ban_bia",
                column: "ten_ban",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bang_luong_ma_nv",
                table: "bang_luong",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "IX_cham_cong_ma_nv",
                table: "cham_cong",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_hoa_don_ma_dv",
                table: "chi_tiet_hoa_don",
                column: "ma_dv");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_hoa_don_ma_hd",
                table: "chi_tiet_hoa_don",
                column: "ma_hd");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_phieu_nhap_ma_hang",
                table: "chi_tiet_phieu_nhap",
                column: "ma_hang");

            migrationBuilder.CreateIndex(
                name: "IX_chi_tiet_phieu_nhap_ma_pn",
                table: "chi_tiet_phieu_nhap",
                column: "ma_pn");

            migrationBuilder.CreateIndex(
                name: "idx_dat_ban_thoi_gian",
                table: "dat_ban",
                columns: new[] { "thoi_gian_bat_dau", "thoi_gian_ket_thuc" });

            migrationBuilder.CreateIndex(
                name: "IX_dat_ban_ma_ban",
                table: "dat_ban",
                column: "ma_ban");

            migrationBuilder.CreateIndex(
                name: "IX_dat_ban_ma_kh",
                table: "dat_ban",
                column: "ma_kh");

            migrationBuilder.CreateIndex(
                name: "IX_dich_vu_ma_hang",
                table: "dich_vu",
                column: "ma_hang");

            migrationBuilder.CreateIndex(
                name: "IX_gia_gio_choi_ma_loai",
                table: "gia_gio_choi",
                column: "ma_loai");

            migrationBuilder.CreateIndex(
                name: "IX_hoa_don_ma_ban",
                table: "hoa_don",
                column: "ma_ban");

            migrationBuilder.CreateIndex(
                name: "IX_hoa_don_ma_kh",
                table: "hoa_don",
                column: "ma_kh");

            migrationBuilder.CreateIndex(
                name: "IX_hoa_don_ma_nv",
                table: "hoa_don",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "UQ__khach_ha__DDDFB4839B1180CA",
                table: "khach_hang",
                column: "sdt",
                unique: true,
                filter: "[sdt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__khu_vuc__5A585CB0A8D242DE",
                table: "khu_vuc",
                column: "ten_khu_vuc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lich_su_hoat_dong_ma_nv",
                table: "lich_su_hoat_dong",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "UQ__loai_ban__5FFB310CAD7B6805",
                table: "loai_ban",
                column: "ten_loai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mat_hang_ma_ncc_default",
                table: "mat_hang",
                column: "ma_ncc_default");

            migrationBuilder.CreateIndex(
                name: "IX_nhan_vien_ma_nhom",
                table: "nhan_vien",
                column: "ma_nhom");

            migrationBuilder.CreateIndex(
                name: "UQ__nhan_vie__C17D2329E67CBEE1",
                table: "nhan_vien",
                column: "ma_van_tay",
                unique: true,
                filter: "[ma_van_tay] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__nhan_vie__E37BF412AB415224",
                table: "nhan_vien",
                column: "faceid_hash",
                unique: true,
                filter: "[faceid_hash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__nhom_quy__CA147C69D99B9308",
                table: "nhom_quyen",
                column: "ten_nhom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_phan_quyen_ma_cn",
                table: "phan_quyen",
                column: "ma_cn");

            migrationBuilder.CreateIndex(
                name: "IX_phieu_nhap_ma_ncc",
                table: "phieu_nhap",
                column: "ma_ncc");

            migrationBuilder.CreateIndex(
                name: "IX_phieu_nhap_ma_nv",
                table: "phieu_nhap",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "IX_so_quy_ma_hd_lien_quan",
                table: "so_quy",
                column: "ma_hd_lien_quan");

            migrationBuilder.CreateIndex(
                name: "IX_so_quy_ma_nv",
                table: "so_quy",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "idx_vietqr_giao_dich_ma_giao_dich",
                table: "vietqr_giao_dich",
                column: "ma_giao_dich");

            migrationBuilder.CreateIndex(
                name: "idx_vietqr_giao_dich_ma_hd",
                table: "vietqr_giao_dich",
                column: "ma_hd");

            migrationBuilder.CreateIndex(
                name: "idx_vietqr_giao_dich_trang_thai",
                table: "vietqr_giao_dich",
                column: "trang_thai");

            migrationBuilder.CreateIndex(
                name: "UQ__vietqr_g__FB80ED332688027D",
                table: "vietqr_giao_dich",
                column: "ma_giao_dich",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bang_luong");

            migrationBuilder.DropTable(
                name: "cham_cong");

            migrationBuilder.DropTable(
                name: "chi_tiet_hoa_don");

            migrationBuilder.DropTable(
                name: "chi_tiet_phieu_nhap");

            migrationBuilder.DropTable(
                name: "dat_ban");

            migrationBuilder.DropTable(
                name: "gia_gio_choi");

            migrationBuilder.DropTable(
                name: "lich_su_hoat_dong");

            migrationBuilder.DropTable(
                name: "phan_quyen");

            migrationBuilder.DropTable(
                name: "so_quy");

            migrationBuilder.DropTable(
                name: "vietqr_config");

            migrationBuilder.DropTable(
                name: "vietqr_giao_dich");

            migrationBuilder.DropTable(
                name: "vietqr_webhook_log");

            migrationBuilder.DropTable(
                name: "dich_vu");

            migrationBuilder.DropTable(
                name: "phieu_nhap");

            migrationBuilder.DropTable(
                name: "chuc_nang");

            migrationBuilder.DropTable(
                name: "hoa_don");

            migrationBuilder.DropTable(
                name: "mat_hang");

            migrationBuilder.DropTable(
                name: "ban_bia");

            migrationBuilder.DropTable(
                name: "nhan_vien");

            migrationBuilder.DropTable(
                name: "nha_cung_cap");

            migrationBuilder.DropTable(
                name: "khach_hang");

            migrationBuilder.DropTable(
                name: "khu_vuc");

            migrationBuilder.DropTable(
                name: "loai_ban");

            migrationBuilder.DropTable(
                name: "nhom_quyen");
        }
    }
}
