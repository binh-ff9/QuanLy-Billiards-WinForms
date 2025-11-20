using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // <--- QUAN TRỌNG: Thêm dòng này để dùng WebBrowser và MessageBox
using System.Drawing;       // <--- Thêm dòng này nếu cần dùng Font, Color

namespace Billiard.WinForm.Helpers
{
    public class InvoicePrinter
    {
        public void PrintInvoice(HoaDon hd, string tenQuan = "BILLIARD PRO CLUB", string diaChi = "123 Đường ABC, TP.HCM")
        {
            try
            {
                // 1. Tính toán tiền giờ
                var gioVao = hd.ThoiGianBatDau ?? DateTime.Now;
                var gioRa = hd.ThoiGianKetThuc ?? DateTime.Now;
                var thoiGianChoi = gioRa - gioVao;
                decimal giaGio = hd.MaBanNavigation?.MaLoaiNavigation?.GiaGio ?? 0;
                decimal tienGio = (decimal)thoiGianChoi.TotalHours * giaGio;
                decimal tongDichVu = hd.ChiTietHoaDons.Sum(ct => ct.ThanhTien) ?? 0;
                decimal tongCong = tienGio + tongDichVu;

                // 2. Tạo nội dung HTML
                string htmlContent = $@"
                <html>
                <head>
                    <style>
                        body {{ 
                            font-family: 'Segoe UI', Tahoma, sans-serif; 
                            font-size: 18px; /* Tăng cỡ chữ cơ bản lên */
                            margin: 0; 
                            padding: 10px; 
                            color: #333;
                            width: 100%; /* Để tự co giãn theo khổ giấy */
                        }} 
        
                        /* HEADER TO, ĐẬM */
                        .header {{ text-align: center; margin-bottom: 20px; }}
                        .brand-name {{ font-size: 22px; font-weight: 900; margin-bottom: 5px; text-transform: uppercase; }}
                        .address {{ font-size: 13px; color: #555; margin-bottom: 3px;}}
                        .invoice-title {{ 
                            font-size: 24px; font-weight: bold; margin-top: 15px; 
                            border-top: 2px dashed #333; border-bottom: 2px dashed #333; 
                            padding: 10px 0; 
                        }}

                        /* THÔNG TIN CHUNG */
                        .info-section {{ margin: 15px 0; line-height: 1.6; }}
                        .row-info {{ display: flex; justify-content: space-between; }} /* Căn đều 2 bên nếu cần */

                        /* BẢNG DỊCH VỤ (QUAN TRỌNG: Chia tỷ lệ cột cứng) */
                        .table-data {{ width: 100%; border-collapse: collapse; margin-top: 15px; }}
                        .table-data th {{ 
                            border-bottom: 2px solid #000; 
                            padding: 8px 0; 
                            text-transform: uppercase; 
                            font-size: 12px;
                        }}
                        .table-data td {{ 
                            padding: 8px 0; 
                            border-bottom: 1px dotted #ccc; 
                            vertical-align: top;
                        }}
        
                        /* Căn chỉnh cột chuẩn */
                        .col-name {{ width: 45%; text-align: left; }}
                        .col-qty  {{ width: 15%; text-align: center; }}
                        .col-price {{ width: 20%; text-align: right; }}
                        .col-total {{ width: 20%; text-align: right; }}

                        /* PHẦN TỔNG TIỀN (Căn phải cho đẹp) */
                        .total-section {{ margin-top: 20px; border-top: 1px solid #000; padding-top: 10px; }}
                        .total-row {{ display: flex; justify-content: flex-end; margin-bottom: 5px; }}
                        .total-label {{ width: 150px; text-align: right; padding-right: 20px; color: #555; }}
                        .total-value {{ width: 120px; text-align: right; font-weight: bold; }}
        
                        .grand-total {{ font-size: 18px; color: #000; margin-top: 10px; }}

                        .footer {{ text-align: center; margin-top: 30px; font-style: italic; font-size: 13px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <div class='brand-name'>{tenQuan}</div>
                        <div class='address'>{diaChi}</div>
                        <div class='address'>Hotline: 0909.123.456</div>
                        <div class='invoice-title'>PHIẾU THANH TOÁN</div>
                    </div>

                    <div class='info-section'>
                        <div><b>Số HĐ:</b> #{hd.MaHd}</div>
                        <div><b>Ngày:</b> {DateTime.Now:dd/MM/yyyy HH:mm}</div>
                        <div><b>Bàn:</b> {hd.MaBanNavigation?.TenBan}</div>
                        <div><b>Nhân viên:</b> {hd.MaNvNavigation?.TenNv}</div>
                        <div><b>Khách hàng:</b> {hd.MaKhNavigation?.TenKh ?? "Khách lẻ"}</div>
                    </div>

                    <div class='info-section' style='background-color: #f8f9fa; padding: 8px; border-radius: 4px;'>
                        <div class='row-info'><span>Giờ vào:</span> <b>{gioVao:HH:mm}</b></div>
                        <div class='row-info'><span>Giờ ra:</span> <b>{gioRa:HH:mm}</b></div>
                        <div class='row-info'><span>Thời gian chơi:</span> <b>{(int)thoiGianChoi.TotalHours}h {thoiGianChoi.Minutes}p</b></div>
                    </div>

                    <table class='table-data'>
                        <thead>
                            <tr>
                                <th class='col-name'>Tên món / DV</th>
                                <th class='col-qty'>SL</th>
                                <th class='col-price'>Đơn giá</th>
                                <th class='col-total'>Thành tiền</th>
                            </tr>
                        </thead>
                        <tbody>";

                                // Dòng Tiền giờ
                                htmlContent += $@"
                            <tr>
                                <td class='col-name'>
                                    <b>Tiền giờ</b><br/>
                                    <small style='color:#666'>({giaGio:N0}/h)</small>
                                </td>
                                <td class='col-qty'>{(int)thoiGianChoi.TotalHours}:{thoiGianChoi.Minutes:00}</td>
                                <td class='col-price'>-</td>
                                <td class='col-total'>{tienGio:N0}</td>
                            </tr>";

                                // Danh sách dịch vụ
                                foreach (var item in hd.ChiTietHoaDons)
                                {
                                    var tenDv = item.MaDvNavigation?.TenDv ?? "DV xóa";
                                    var giaDv = item.MaDvNavigation?.Gia ?? 0;
                                    htmlContent += $@"
                            <tr>
                                <td class='col-name'>{tenDv}</td>
                                <td class='col-qty'>{item.SoLuong}</td>
                                <td class='col-price'>{giaDv:N0}</td>
                                <td class='col-total'>{item.ThanhTien:N0}</td>
                            </tr>";
                                }

                                htmlContent += $@"
                        </tbody>
                    </table>

                    <div class='total-section'>
                        <div class='total-row'>
                            <span class='total-label'>Tổng dịch vụ:</span>
                            <span class='total-value'>{tongDichVu:N0}</span>
                        </div>
                        <div class='total-row'>
                            <span class='total-label'>Tổng tiền giờ:</span>
                            <span class='total-value'>{tienGio:N0}</span>
                        </div>
                        <div class='total-row grand-total'>
                            <span class='total-label'>TỔNG CỘNG:</span>
                            <span class='total-value'>{tongCong:N0} đ</span>
                        </div>
                    </div>

                    <div class='footer'>
                        <p>Cảm ơn quý khách và hẹn gặp lại!</p>
                        <p>Wifi: <b>Bi-a Pro</b> | Pass: <b>88888888</b></p>
                    </div>
                </body>
                </html>";

                // 3. Gọi WebBrowser để in
                // LƯU Ý .NET 8: WebBrowser cần chạy trên luồng UI (STA).
                // Nếu bạn gọi hàm này từ Task.Run, nó sẽ lỗi. Hãy gọi trực tiếp từ Button Click.
                var wb = new WebBrowser();
                wb.DocumentText = htmlContent;
                wb.DocumentCompleted += (s, e) =>
                {
                    try
                    {
                        wb.ShowPrintPreviewDialog();
                    }
                    catch
                    {
                        // Fallback nếu PrintPreview lỗi trên .NET 8 (một số máy bị)
                        wb.Print();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn: " + ex.Message);
            }
        }
    }
}