using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billiard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class lichLam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lich_lam_viec",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nv = table.Column<int>(type: "int", nullable: false),
                    ngay = table.Column<DateOnly>(type: "date", nullable: false),
                    gio_bat_dau = table.Column<TimeOnly>(type: "time", nullable: false),
                    gio_ket_thuc = table.Column<TimeOnly>(type: "time", nullable: false),
                    ca = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Sang"),
                    trang_thai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "DaXepLich"),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    nguoi_tao = table.Column<int>(type: "int", nullable: true),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime", nullable: true),
                    nguoi_cap_nhat = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lich_lam_viec", x => x.id);
                    table.ForeignKey(
                        name: "FK_lich_lam_viec_nguoi_cap_nhat",
                        column: x => x.nguoi_cap_nhat,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                    table.ForeignKey(
                        name: "FK_lich_lam_viec_nguoi_tao",
                        column: x => x.nguoi_tao,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv");
                    table.ForeignKey(
                        name: "FK_lich_lam_viec_nhan_vien",
                        column: x => x.ma_nv,
                        principalTable: "nhan_vien",
                        principalColumn: "ma_nv",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_lich_lam_viec_ma_nv",
                table: "lich_lam_viec",
                column: "ma_nv");

            migrationBuilder.CreateIndex(
                name: "idx_lich_lam_viec_ngay",
                table: "lich_lam_viec",
                column: "ngay");

            migrationBuilder.CreateIndex(
                name: "idx_lich_lam_viec_unique",
                table: "lich_lam_viec",
                columns: new[] { "ma_nv", "ngay", "gio_bat_dau", "gio_ket_thuc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lich_lam_viec_nguoi_cap_nhat",
                table: "lich_lam_viec",
                column: "nguoi_cap_nhat");

            migrationBuilder.CreateIndex(
                name: "IX_lich_lam_viec_nguoi_tao",
                table: "lich_lam_viec",
                column: "nguoi_tao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lich_lam_viec");
        }
    }
}
