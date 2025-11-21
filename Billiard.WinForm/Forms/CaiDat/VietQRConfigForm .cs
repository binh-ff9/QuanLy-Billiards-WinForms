using Billiard.DAL.Data;
using Billiard.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.CaiDat
{
    public partial class VietQRConfigForm : Form
    {
        private readonly BilliardDbContext _context;
        private VietqrConfig _config;

        // UI Controls
        private TextBox txtTenCauHinh;
        private TextBox txtBankId;
        private TextBox txtAccountNo;
        private TextBox txtAccountName;
        private ComboBox cboTemplate;
        private CheckBox chkMacDinh;
        private Button btnLuu;
        private Button btnTest;

        public VietQRConfigForm(BilliardDbContext context)
        {
            _context = context;
            InitializeComponent();
            InitializeCustomUI();
        }

        private void InitializeCustomUI()
        {
            this.Text = "Cấu hình VietQR";
            this.Size = new Size(600, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            this.Controls.Add(pnlMain);

            int yPos = 20;

            // Title
            var lblTitle = new Label
            {
                Text = "⚙️ Cấu hình VietQR",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(20, yPos),
                AutoSize = true
            };
            pnlMain.Controls.Add(lblTitle);
            yPos += 50;

            // Tên cấu hình
            yPos = AddFormField(pnlMain, "Tên cấu hình:", out txtTenCauHinh, yPos);
            txtTenCauHinh.Text = "Cấu hình mặc định";

            // Bank ID
            yPos = AddFormField(pnlMain, "Mã ngân hàng (Bank ID):", out txtBankId, yPos);
            txtBankId.PlaceholderText = "VD: MB, VCB, TCB, ACB...";

            // Account Number
            yPos = AddFormField(pnlMain, "Số tài khoản:", out txtAccountNo, yPos);
            txtAccountNo.PlaceholderText = "Nhập số tài khoản";

            // Account Name
            yPos = AddFormField(pnlMain, "Tên chủ tài khoản:", out txtAccountName, yPos);
            txtAccountName.PlaceholderText = "Tên người thưởng hưởng";

            // Template
            var lblTemplate = new Label
            {
                Text = "Template QR:",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(20, yPos),
                AutoSize = true
            };
            pnlMain.Controls.Add(lblTemplate);

            cboTemplate = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, yPos + 25),
                Size = new Size(540, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboTemplate.Items.AddRange(new object[] {
                "compact",
                "compact2",
                "qr_only",
                "print"
            });
            cboTemplate.SelectedIndex = 0;
            pnlMain.Controls.Add(cboTemplate);
            yPos += 70;

            // Mặc định
            chkMacDinh = new CheckBox
            {
                Text = "Đặt làm cấu hình mặc định",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(20, yPos),
                AutoSize = true,
                Checked = true
            };
            pnlMain.Controls.Add(chkMacDinh);
            yPos += 40;

            // Buttons
            btnTest = new Button
            {
                Text = "🧪 Test mã QR",
                Location = new Point(20, yPos),
                Size = new Size(260, 45),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.Click += BtnTest_Click;
            pnlMain.Controls.Add(btnTest);

            btnLuu = new Button
            {
                Text = "💾 Lưu cấu hình",
                Location = new Point(300, yPos),
                Size = new Size(260, 45),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;
            pnlMain.Controls.Add(btnLuu);
        }

        private int AddFormField(Panel panel, string label, out TextBox textBox, int yPos)
        {
            var lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(20, yPos),
                AutoSize = true
            };
            panel.Controls.Add(lbl);

            textBox = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(20, yPos + 25),
                Size = new Size(540, 30)
            };
            panel.Controls.Add(textBox);

            return yPos + 70;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadConfig();
        }

        private async System.Threading.Tasks.Task LoadConfig()
        {
            try
            {
                _config = await _context.VietqrConfigs
                    .FirstOrDefaultAsync(c => c.LaMacDinh == true);

                if (_config != null)
                {
                    txtTenCauHinh.Text = _config.TenCauHinh;
                    txtBankId.Text = _config.BankId;
                    txtAccountNo.Text = _config.AccountNo;
                    txtAccountName.Text = _config.AccountName;
                    cboTemplate.SelectedItem = _config.Template;
                    chkMacDinh.Checked = _config.LaMacDinh ?? false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải cấu hình: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var testUrl = $"https://img.vietqr.io/image/{txtBankId.Text}-{txtAccountNo.Text}-{cboTemplate.SelectedItem}.png" +
                    $"?amount=100000&addInfo=Test&accountName={Uri.EscapeDataString(txtAccountName.Text)}";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = testUrl,
                    UseShellExecute = true
                });

                MessageBox.Show("Đã mở mã QR test trong trình duyệt!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (_config == null)
                {
                    _config = new VietqrConfig();
                    _context.VietqrConfigs.Add(_config);
                }

                // Nếu đặt làm mặc định, bỏ mặc định các config khác
                if (chkMacDinh.Checked)
                {
                    var otherConfigs = await _context.VietqrConfigs
                        .Where(c => c.Id != _config.Id)
                        .ToListAsync();

                    foreach (var config in otherConfigs)
                    {
                        config.LaMacDinh = false;
                    }
                }

                _config.TenCauHinh = txtTenCauHinh.Text.Trim();
                _config.BankId = txtBankId.Text.Trim().ToUpper();
                _config.AccountNo = txtAccountNo.Text.Trim();
                _config.AccountName = txtAccountName.Text.Trim();
                _config.Template = cboTemplate.SelectedItem.ToString();
                _config.LaMacDinh = chkMacDinh.Checked;
                _config.TrangThai = true;
                _config.NgayCapNhat = DateTime.Now;

                if (_config.NgayTao == DateTime.MinValue)
                {
                    _config.NgayTao = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                this.Cursor = Cursors.Default;

                MessageBox.Show("✓ Đã lưu cấu hình VietQR thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi lưu cấu hình: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenCauHinh.Text))
            {
                MessageBox.Show("Vui lòng nhập tên cấu hình!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCauHinh.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBankId.Text))
            {
                MessageBox.Show("Vui lòng nhập mã ngân hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBankId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAccountNo.Text))
            {
                MessageBox.Show("Vui lòng nhập số tài khoản!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccountNo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAccountName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên chủ tài khoản!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccountName.Focus();
                return false;
            }
            var validBanks = new[] { "MB", "VCB", "TCB", "ACB", "VPB", "TPB", "STB", "BIDV" };
            if (!validBanks.Contains(txtBankId.Text.Trim().ToUpper()))
            {
                var result = MessageBox.Show(
                    $"Mã ngân hàng '{txtBankId.Text}' có thể không được hỗ trợ.\n\n" +
                    "Các mã phổ biến: MB, VCB, TCB, ACB, VPB, TPB, STB, BIDV\n\n" +
                    "Bạn có muốn tiếp tục không?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return false;
            }

            return true;
        }
    }
}