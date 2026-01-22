using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billiard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddThoiGianThanhToanToHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "thoi_gian_thanh_toan",
                table: "hoa_don",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thoi_gian_thanh_toan",
                table: "hoa_don");
        }
    }
}
