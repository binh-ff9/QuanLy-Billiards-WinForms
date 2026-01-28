using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class AttendanceHistoryForm : Form
    {
        #region Fields
        private readonly NhanVienService _nhanVienService;
        private readonly int _maNV;
        private readonly string _tenNV;
        private int _currentMonth;
        private int _currentYear;
        private List<ChamCong> _attendanceRecords;

        private Panel pnlStats;
        private FlowLayoutPanel flowAttendance;
        private Label lblMonthYear;
        #endregion

        #region Constructor
        public AttendanceHistoryForm(int maNV, string tenNV)
        {
            InitializeComponent();
            _nhanVienService = new NhanVienService();
            _maNV = maNV;
            _tenNV = tenNV;
            _currentMonth = DateTime.Now.Month;
            _currentYear = DateTime.Now.Year;

            InitializeUI();
            LoadAttendanceHistory();
        }
        #endregion

        #region Initialize UI
        private void InitializeUI()
        {
            this.Text = $"📅 Lịch sử chấm công - {_tenNV}";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Header
            var header = CreateHeader();
            mainPanel.Controls.Add(header);

            // Month Navigation
            var navPanel = CreateNavigationPanel();
            navPanel.Location = new Point(20, 100);
            mainPanel.Controls.Add(navPanel);

            // Statistics Panel
            pnlStats = CreateStatsPanel();
            pnlStats.Location = new Point(20, 180);
            mainPanel.Controls.Add(pnlStats);

            // Attendance List
            flowAttendance = new FlowLayoutPanel
            {
                Location = new Point(20, 300),
                Size = new Size(940, 340),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.White
            };
            mainPanel.Controls.Add(flowAttendance);

            this.Controls.Add(mainPanel);
        }

        private Panel CreateHeader()
        {
            var panel = new Panel
            {
                Size = new Size(940, 80),
                Location = new Point(0, 0),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = $"📅 Lịch sử chấm công",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            var lblEmployee = new Label
            {
                Text = $"👤 {_tenNV}",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 50)
            };

            panel.Controls.AddRange(new Control[] { lblTitle, lblEmployee });
            return panel;
        }

        private Panel CreateNavigationPanel()
        {
            var panel = new Panel
            {
                Size = new Size(940, 60),
                BackColor = Color.White
            };

            var btnPrev = new Button
            {
                Text = "◀",
                Size = new Size(40, 40),
                Location = new Point(350, 10),
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand
            };
            btnPrev.FlatAppearance.BorderSize = 0;
            btnPrev.Click += (s, e) => ChangeMonth(-1);

            lblMonthYear = new Label
            {
                Size = new Size(200, 40),
                Location = new Point(400, 10),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            UpdateMonthLabel();

            var btnNext = new Button
            {
                Text = "▶",
                Size = new Size(40, 40),
                Location = new Point(610, 10),
                BackColor = Color.FromArgb(102, 126, 234),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand
            };
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.Click += (s, e) => ChangeMonth(1);

            var btnToday = new Button
            {
                Text = "Hôm nay",
                Size = new Size(100, 40),
                Location = new Point(670, 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnToday.FlatAppearance.BorderSize = 0;
            btnToday.Click += (s, e) => GoToToday();

            panel.Controls.AddRange(new Control[] { btnPrev, lblMonthYear, btnNext, btnToday });
            return panel;
        }

        private Panel CreateStatsPanel()
        {
            var panel = new Panel
            {
                Size = new Size(940, 100),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "📊 Thống kê tháng",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            // Will be populated with actual stats
            panel.Controls.Add(lblTitle);
            return panel;
        }
        #endregion

        #region Load Data
        private void LoadAttendanceHistory()
        {
            try
            {
                _attendanceRecords = _nhanVienService.GetAttendanceByMonth(_maNV, _currentMonth, _currentYear);

                UpdateStats();
                DisplayAttendance();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStats()
        {
            pnlStats.Controls.Clear();

            var lblTitle = new Label
            {
                Text = "📊 Thống kê tháng",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            pnlStats.Controls.Add(lblTitle);

            if (_attendanceRecords == null || !_attendanceRecords.Any())
            {
                var lblEmpty = new Label
                {
                    Text = "Chưa có dữ liệu chấm công",
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 50)
                };
                pnlStats.Controls.Add(lblEmpty);
                return;
            }

            int totalDays = _attendanceRecords.Count;
            decimal totalHours = _attendanceRecords.Sum(r => r.SoGioLam ?? 0);
            int lateDays = _attendanceRecords.Count(r => r.TrangThai == "DiTre");

            // Total Days
            var statDays = CreateStatBox("📅", "Số ngày làm", totalDays.ToString(), 20, 50);
            pnlStats.Controls.Add(statDays);

            // Total Hours
            var statHours = CreateStatBox("⏱️", "Tổng giờ làm", $"{totalHours:F1}h", 240, 50);
            pnlStats.Controls.Add(statHours);

            // Late Days
            var statLate = CreateStatBox("⚠️", "Số lần đi trễ", lateDays.ToString(), 460, 50);
            pnlStats.Controls.Add(statLate);

            // Average
            var avgHours = totalDays > 0 ? totalHours / totalDays : 0;
            var statAvg = CreateStatBox("📈", "Trung bình/ngày", $"{avgHours:F1}h", 680, 50);
            pnlStats.Controls.Add(statAvg);
        }

        private Panel CreateStatBox(string icon, string label, string value, int x, int y)
        {
            var box = new Panel
            {
                Size = new Size(200, 40),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(239, 246, 255)
            };

            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 14F),
                AutoSize = true,
                Location = new Point(10, 8)
            };

            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(40, 5)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 126, 234),
                AutoSize = true,
                Location = new Point(40, 18)
            };

            box.Controls.AddRange(new Control[] { lblIcon, lblLabel, lblValue });
            return box;
        }

        private void DisplayAttendance()
        {
            flowAttendance.Controls.Clear();

            if (_attendanceRecords == null || !_attendanceRecords.Any())
            {
                var empty = new Panel
                {
                    Size = new Size(900, 300),
                    BackColor = Color.White
                };

                var lblIcon = new Label
                {
                    Text = "📅",
                    Font = new Font("Segoe UI", 48F),
                    AutoSize = true,
                    Location = new Point(420, 80)
                };

                var lblText = new Label
                {
                    Text = $"Chưa có dữ liệu chấm công tháng {_currentMonth}/{_currentYear}",
                    Font = new Font("Segoe UI", 12F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(300, 160)
                };

                empty.Controls.AddRange(new Control[] { lblIcon, lblText });
                flowAttendance.Controls.Add(empty);
                return;
            }

            foreach (var record in _attendanceRecords.OrderByDescending(r => r.Ngay))
            {
                var card = CreateAttendanceCard(record);
                flowAttendance.Controls.Add(card);
            }
        }

        private Panel CreateAttendanceCard(ChamCong record)
        {
            var card = new Panel
            {
                Size = new Size(920, 100),
                BackColor = Color.White,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            var date = record.Ngay.ToDateTime(TimeOnly.MinValue);
            var dayOfWeek = date.ToString("dddd", new System.Globalization.CultureInfo("vi-VN"));
            var dateStr = date.ToString("dd/MM/yyyy");

            // Date section
            var datePanel = new Panel
            {
                Size = new Size(150, 80),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(239, 246, 255)
            };

            var lblDay = new Label
            {
                Text = date.Day.ToString(),
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 126, 234),
                Size = new Size(60, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(5, 10)
            };

            var lblDayName = new Label
            {
                Text = dayOfWeek,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(10, 55)
            };

            var lblDate = new Label
            {
                Text = dateStr,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(10, 70)
            };

            datePanel.Controls.AddRange(new Control[] { lblDay, lblDayName, lblDate });

            // Time section
            var gioVao = record.GioVao?.ToString("HH:mm") ?? "--:--";
            var gioRa = record.GioRa?.ToString("HH:mm") ?? "--:--";

            var lblGioVao = new Label
            {
                Text = $"⏰ Vào: {gioVao}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(180, 20)
            };

            var lblGioRa = new Label
            {
                Text = $"🚪 Ra: {gioRa}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(180, 45)
            };

            var lblSoGio = new Label
            {
                Text = $"⏱️ Tổng: {record.SoGioLam?.ToString("F1") ?? "0"}h",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 126, 234),
                AutoSize = true,
                Location = new Point(180, 70)
            };

            // Status
            var statusInfo = GetStatusInfo(record.TrangThai);
            var lblStatus = new Label
            {
                Text = $"{statusInfo.Icon} {statusInfo.Text}",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = statusInfo.Color,
                BackColor = statusInfo.BackColor,
                Padding = new Padding(8, 4, 8, 4),
                AutoSize = true,
                Location = new Point(400, 25)
            };

            // Auth method
            var authIcon = GetAuthIcon(record.XacThucBang);
            var lblAuth = new Label
            {
                Text = $"{authIcon} {record.XacThucBang}",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(400, 60)
            };

            // Note
            if (!string.IsNullOrEmpty(record.GhiChu))
            {
                var lblNote = new Label
                {
                    Text = $"📝 {record.GhiChu}",
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    MaximumSize = new Size(400, 0),
                    Location = new Point(180, 95)
                };
                card.Controls.Add(lblNote);
                card.Height = 120;
            }

            card.Controls.AddRange(new Control[]
            {
                datePanel, lblGioVao, lblGioRa, lblSoGio, lblStatus, lblAuth
            });

            return card;
        }

        private (string Icon, string Text, Color Color, Color BackColor) GetStatusInfo(string status)
        {
            return status switch
            {
                "DungGio" => ("✅", "Đúng giờ", Color.FromArgb(21, 87, 36), Color.FromArgb(212, 237, 218)),
                "DiTre" => ("⚠️", "Đi trễ", Color.FromArgb(133, 100, 4), Color.FromArgb(255, 243, 205)),
                "VeSom" => ("⏰", "Về sớm", Color.FromArgb(12, 84, 96), Color.FromArgb(209, 236, 241)),
                "Vang" => ("❌", "Vắng", Color.FromArgb(114, 28, 36), Color.FromArgb(248, 215, 218)),
                "CaThem" => ("⭐", "Ca thêm", Color.FromArgb(13, 60, 108), Color.FromArgb(207, 226, 243)),
                _ => ("📝", status, Color.Gray, Color.FromArgb(233, 236, 239))
            };
        }

        private string GetAuthIcon(string method)
        {
            return method switch
            {
                "ThuCong" => "✍️",
                "FaceID" => "📸",
                "VanTay" => "👆",
                _ => "📝"
            };
        }
        #endregion

        #region Navigation
        private void ChangeMonth(int delta)
        {
            _currentMonth += delta;

            if (_currentMonth > 12)
            {
                _currentMonth = 1;
                _currentYear++;
            }
            else if (_currentMonth < 1)
            {
                _currentMonth = 12;
                _currentYear--;
            }

            UpdateMonthLabel();
            LoadAttendanceHistory();
        }

        private void GoToToday()
        {
            _currentMonth = DateTime.Now.Month;
            _currentYear = DateTime.Now.Year;

            UpdateMonthLabel();
            LoadAttendanceHistory();
        }

        private void UpdateMonthLabel()
        {
            lblMonthYear.Text = $"Tháng {_currentMonth}/{_currentYear}";
        }
        #endregion
    }
}