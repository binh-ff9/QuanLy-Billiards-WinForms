using Billiard.BLL.Services.NhanVienService;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.NhanVien
{
    public partial class ActivityHistoryForm : Form
    {
        private readonly NhanVienService _nhanVienService;
        private readonly int _maNv;
        private readonly string _tenNv;
        private DataGridView dgvHistory = null!;
        private ComboBox cboFilter = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private Label lblTotal = null!;
        private List<LichSuHoatDong> _allActivities = new();

        public ActivityHistoryForm(int maNv, string tenNv)
        {
            _maNv = maNv;
            _tenNv = tenNv;
            _nhanVienService = new NhanVienService();

            InitializeComponent();
            InitializeCustomControls();
            LoadActivityHistory();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Form properties
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterParent;
            Text = $"Lịch sử hoạt động - {_tenNv}";
            BackColor = Color.FromArgb(248, 250, 252);
            Font = new Font("Segoe UI", 9.75F);
            MinimumSize = new Size(900, 600);

            ResumeLayout(false);
        }

        private void InitializeCustomControls()
        {
            // Header Panel
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblTitle = new Label
            {
                Text = $"📜 Lịch sử hoạt động - {_tenNv}",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 46),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var lblSubtitle = new Label
            {
                Text = $"Mã NV: {_maNv}",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(20, 48)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSubtitle });

            // Filter Panel
            var pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            var lblFilter = new Label
            {
                Text = "Lọc theo:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            cboFilter = new ComboBox
            {
                Location = new Point(95, 12),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cboFilter.Items.AddRange(new object[]
            {
                "Tất cả",
                "Chấm công",
                "Lương",
                "Lịch làm việc",
                "Hệ thống"
            });
            cboFilter.SelectedIndex = 0;
            cboFilter.SelectedIndexChanged += (s, e) => FilterActivities();

            var lblFrom = new Label
            {
                Text = "Từ:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(270, 15),
                AutoSize = true
            };

            dtpFrom = new DateTimePicker
            {
                Location = new Point(310, 12),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };
            dtpFrom.ValueChanged += (s, e) => FilterActivities();

            var lblTo = new Label
            {
                Text = "Đến:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(470, 15),
                AutoSize = true
            };

            dtpTo = new DateTimePicker
            {
                Location = new Point(515, 12),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };
            dtpTo.ValueChanged += (s, e) => FilterActivities();

            //var btnExport = new Button
            //{
            //    Text = "📥 Xuất Excel",
            //    Location = new Point(680, 10),
            //    Size = new Size(130, 35),
            //    BackColor = Color.FromArgb(40, 167, 69),
            //    ForeColor = Color.White,
            //    FlatStyle = FlatStyle.Flat,
            //    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            //    Cursor = Cursors.Hand
            //};
            //btnExport.FlatAppearance.BorderSize = 0;
            //btnExport.Click += BtnExport_Click;

            var btnRefresh = new Button
            {
                Text = "🔄",
                Location = new Point(820, 10),
                Size = new Size(40, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadActivityHistory();

            lblTotal = new Label
            {
                Text = "Tổng: 0 hoạt động",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(102, 126, 234),
                Location = new Point(20, 50),
                AutoSize = true
            };

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblFilter, cboFilter, lblFrom, dtpFrom, lblTo, dtpTo,
                 btnRefresh, lblTotal
            });

            // DataGridView
            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                ColumnHeadersHeight = 45,
                RowTemplate = { Height = 40 },
                Font = new Font("Segoe UI", 9.5F),
                GridColor = Color.FromArgb(226, 232, 240),
                EnableHeadersVisualStyles = false
            };

            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(102, 126, 234);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHistory.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            dgvHistory.DefaultCellStyle.SelectionForeColor = Color.FromArgb(26, 26, 46);
            dgvHistory.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            // Add columns
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThoiGian",
                HeaderText = "Thời gian",
                Width = 180,
                DataPropertyName = "ThoiGian"
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HanhDong",
                HeaderText = "Hành động",
                Width = 200,
                DataPropertyName = "HanhDong"
            });

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ChiTiet",
                HeaderText = "Chi tiết",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DataPropertyName = "ChiTiet"
            });

            // Format datetime column
            dgvHistory.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == 0 && e.Value is DateTime dt)
                {
                    e.Value = dt.ToString("dd/MM/yyyy HH:mm:ss");
                    e.FormattingApplied = true;
                }
            };

            // Row styling based on action type
            dgvHistory.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvHistory.Rows.Count)
                {
                    var row = dgvHistory.Rows[e.RowIndex];
                    var action = row.Cells["HanhDong"].Value?.ToString() ?? "";

                    Color iconColor = action.ToLower() switch
                    {
                        var a when a.Contains("check-in") || a.Contains("vào") => Color.FromArgb(40, 167, 69),
                        var a when a.Contains("check-out") || a.Contains("ra") => Color.FromArgb(220, 53, 69),
                        var a when a.Contains("lương") => Color.FromArgb(255, 193, 7),
                        var a when a.Contains("lịch") => Color.FromArgb(102, 126, 234),
                        _ => Color.FromArgb(108, 117, 125)
                    };

                    // Add colored indicator
                    row.Cells["HanhDong"].Style.ForeColor = iconColor;
                    row.Cells["HanhDong"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
            };

            // Main container
            var pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            pnlMain.Controls.Add(dgvHistory);

            // Add separator lines
            var separator1 = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            var separator2 = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Add all to form
            Controls.Add(pnlMain);
            Controls.Add(separator2);
            Controls.Add(pnlFilter);
            Controls.Add(separator1);
            Controls.Add(pnlHeader);
        }

        private void LoadActivityHistory()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Load all activities (last 500)
                _allActivities = _nhanVienService.GetActivityHistory(_maNv, 500);

                FilterActivities();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi tải lịch sử hoạt động: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterActivities()
        {
            try
            {
                var filtered = _allActivities.AsEnumerable();

                // Filter by date range
                var fromDate = dtpFrom.Value.Date;
                var toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);
                filtered = filtered.Where(a => a.ThoiGian >= fromDate && a.ThoiGian <= toDate);

                // Filter by action type
                var filterType = cboFilter.SelectedItem?.ToString() ?? "Tất cả";
                if (filterType != "Tất cả")
                {
                    filtered = filterType switch
                    {
                        "Chấm công" => filtered.Where(a =>
                            a.HanhDong.Contains("Check-in", StringComparison.OrdinalIgnoreCase) ||
                            a.HanhDong.Contains("Check-out", StringComparison.OrdinalIgnoreCase) ||
                            a.HanhDong.Contains("Chấm công", StringComparison.OrdinalIgnoreCase)),
                        "Lương" => filtered.Where(a =>
                            a.HanhDong.Contains("Lương", StringComparison.OrdinalIgnoreCase) ||
                            a.HanhDong.Contains("Salary", StringComparison.OrdinalIgnoreCase)),
                        "Lịch làm việc" => filtered.Where(a =>
                            a.HanhDong.Contains("Lịch", StringComparison.OrdinalIgnoreCase) ||
                            a.HanhDong.Contains("Schedule", StringComparison.OrdinalIgnoreCase)),
                        "Hệ thống" => filtered.Where(a =>
                            a.HanhDong.Contains("Đăng nhập", StringComparison.OrdinalIgnoreCase) ||
                            a.HanhDong.Contains("Đăng xuất", StringComparison.OrdinalIgnoreCase) ||
                            a.HanhDong.Contains("Login", StringComparison.OrdinalIgnoreCase)),
                        _ => filtered
                    };
                }

                var resultList = filtered.OrderByDescending(a => a.ThoiGian).ToList();

                // Update DataGridView
                dgvHistory.DataSource = null;
                dgvHistory.DataSource = resultList;

                // Update total count
                lblTotal.Text = $"Tổng: {resultList.Count} hoạt động";

                // Auto-resize columns
                if (resultList.Any())
                {
                    dgvHistory.Columns["ThoiGian"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvHistory.Columns["ThoiGian"].Width = 180;
                    dgvHistory.Columns["HanhDong"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvHistory.Columns["HanhDong"].Width = 200;
                    dgvHistory.Columns["ChiTiet"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void BtnExport_Click(object? sender, EventArgs e)
        //{
        //    try
        //    {
        //        using var sfd = new SaveFileDialog
        //        {
        //            Filter = "Excel Files|*.xlsx",
        //            FileName = $"LichSuHoatDong_{_tenNv}_{DateTime.Now:yyyyMMdd}.xlsx",
        //            Title = "Xuất lịch sử hoạt động"
        //        };

        //        if (sfd.ShowDialog() == DialogResult.OK)
        //        {
        //            // You would implement Excel export here using a library like EPPlus or ClosedXML
        //            MessageBox.Show("Chức năng xuất Excel đang được phát triển",
        //                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}",
        //            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
    }
}