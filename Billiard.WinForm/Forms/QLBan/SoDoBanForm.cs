using Billiard.BLL.Services.QLBan;
using Billiard.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.QLBan
{
    public partial class SoDoBanForm : Form
    {
        private readonly BanBiaService _banBiaService;
        private MainForm _mainForm;
        private string _currentFloor = "Tầng 1";
        private bool _isEditMode = false;
        private List<BanBium> _allTables;
        private Dictionary<int, TableControl> _tableControls = new Dictionary<int, TableControl>();

        // Event để thông báo khi chọn bàn
        public event EventHandler<BanBium> OnTableSelected;

        // Kích thước canvas cho mỗi tầng (đơn vị: pixel)
        private readonly Dictionary<string, Size> _floorDimensions = new Dictionary<string, Size>
        {
            { "Tầng 1", new Size(800, 600) },
            { "Tầng 2", new Size(800, 600) },
            { "VIP", new Size(600, 450) }
        };

        public SoDoBanForm(BanBiaService banBiaService)
        {
            _banBiaService = banBiaService;
            InitializeComponent();
            SetupLegendColors();
        }

        public void SetMainForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            SetupPermissions();
        }

        private void SetupPermissions()
        {
            if (_mainForm == null) return;

            var chucVu = _mainForm.ChucVu;
            bool isAdmin = chucVu == "Admin" || chucVu == "Quản lý";

            // Chỉ Admin/Quản lý mới được chỉnh sửa
            pnlEditControls.Visible = isAdmin;
        }

        private void SetupLegendColors()
        {
            // Tạo màu sắc cho legend
            CreateLegendColorBox(pnlLegendTrong, Color.FromArgb(34, 197, 94));
            CreateLegendColorBox(pnlLegendDangChoi, Color.FromArgb(239, 68, 68));
            CreateLegendColorBox(pnlLegendDaDat, Color.FromArgb(234, 179, 8));
        }

        private void CreateLegendColorBox(Panel parent, Color color)
        {
            var colorBox = new Panel
            {
                Size = new Size(24, 24),
                Location = new Point(5, 8),
                BackColor = color,
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(colorBox);
            colorBox.BringToFront();
        }

        private async void SoDoBanForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                await LoadTables();
                RenderDiagram();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task LoadTables()
        {
            _allTables = await _banBiaService.GetAllTablesAsync();

            // Gán vị trí mặc định cho các bàn chưa có vị trí
            int index = 0;
            foreach (var table in _allTables)
            {
                if (table.ViTriX == 0 || table.ViTriY == 0)
                {
                    int row = index / 4;
                    int col = index % 4;
                    table.ViTriX = 10 + (col * 25);
                    table.ViTriY = 10 + (row * 30);
                    index++;
                }
            }
        }

        private void RenderDiagram()
        {
            pnlCanvas.Controls.Clear();
            _tableControls.Clear();

            // Lọc bàn theo tầng hiện tại
            var floorTables = _allTables.Where(t =>
                t.MaKhuVucNavigation?.TenKhuVuc == _currentFloor).ToList();

            if (floorTables.Count == 0)
            {
                ShowEmptyFloor();
                return;
            }

            // Đặt kích thước canvas
            var canvasSize = _floorDimensions[_currentFloor];
            pnlCanvas.AutoScrollMinSize = canvasSize;

            // Hiển thị lưới nếu đang ở chế độ chỉnh sửa
            if (_isEditMode)
            {
                DrawGrid(pnlCanvas, canvasSize);
            }

            // Tạo controls cho mỗi bàn
            foreach (var table in floorTables)
            {
                var tableControl = CreateTableControl(table);
                pnlCanvas.Controls.Add(tableControl);
                _tableControls[table.MaBan] = tableControl;
            }
        }

        private void ShowEmptyFloor()
        {
            var emptyPanel = new Panel
            {
                Size = new Size(600, 300),
                Location = new Point((pnlCanvas.Width - 600) / 2, (pnlCanvas.Height - 300) / 2)
            };

            var lblIcon = new Label
            {
                Text = "🎱",
                Font = new Font("Segoe UI", 48F),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblIcon.Location = new Point((emptyPanel.Width - lblIcon.Width) / 2, 80);

            var lblText = new Label
            {
                Text = "Không có bàn",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblText.Location = new Point((emptyPanel.Width - 200) / 2, 160);

            var lblDesc = new Label
            {
                Text = $"Khu vực {_currentFloor} chưa có bàn nào",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblDesc.Location = new Point((emptyPanel.Width - 300) / 2, 195);

            emptyPanel.Controls.AddRange(new Control[] { lblIcon, lblText, lblDesc });
            pnlCanvas.Controls.Add(emptyPanel);
        }

        private void DrawGrid(Panel canvas, Size canvasSize)
        {
            // Tạo bitmap để vẽ lưới một lần
            if (canvas.BackgroundImage != null)
            {
                canvas.BackgroundImage.Dispose();
            }

            Bitmap gridBitmap = new Bitmap(canvasSize.Width, canvasSize.Height);
            using (Graphics graphics = Graphics.FromImage(gridBitmap))
            {
                // Tô nền trắng
                graphics.Clear(Color.White);

                using (Pen pen = new Pen(Color.FromArgb(30, 226, 232, 240), 1))
                {
                    // Vẽ lưới dọc (mỗi 50px)
                    for (int x = 0; x <= canvasSize.Width; x += 50)
                    {
                        graphics.DrawLine(pen, x, 0, x, canvasSize.Height);
                    }

                    // Vẽ lưới ngang (mỗi 50px)
                    for (int y = 0; y <= canvasSize.Height; y += 50)
                    {
                        graphics.DrawLine(pen, 0, y, canvasSize.Width, y);
                    }
                }
            }

            // Đặt bitmap làm background image
            canvas.BackgroundImage = gridBitmap;
            canvas.BackgroundImageLayout = ImageLayout.None;
        }

        private TableControl CreateTableControl(BanBium table)
        {
            var canvasSize = _floorDimensions[_currentFloor];

            // Chuyển từ phần trăm sang pixel
            int x = (int)((table.ViTriX / 100.0) * canvasSize.Width);
            int y = (int)((table.ViTriY / 100.0) * canvasSize.Height);

            var tableControl = new TableControl(table, _isEditMode)
            {
                Location = new Point(x, y)
            };

            // Sự kiện click
            tableControl.TableClicked += TableControl_Clicked;

            // Sự kiện kéo thả (chỉ khi edit mode)
            if (_isEditMode)
            {
                // QUAN TRỌNG: Đăng ký sự kiện cho cả Panel chính và tất cả controls con
                tableControl.MouseDown += TableControl_MouseDown;
                tableControl.MouseMove += TableControl_MouseMove;
                tableControl.MouseUp += TableControl_MouseUp;

                // Đăng ký sự kiện cho tất cả controls con
                foreach (Control child in tableControl.Controls)
                {
                    child.MouseDown += TableControl_MouseDown;
                    child.MouseMove += TableControl_MouseMove;
                    child.MouseUp += TableControl_MouseUp;
                }
            }

            return tableControl;
        }

        private void TableControl_Clicked(object sender, EventArgs e)
        {
            if (_isEditMode) return; // Không cho click khi đang edit

            var tableControl = sender as TableControl;
            if (tableControl == null) return;

            // Trigger event để QLBanForm xử lý
            OnTableSelected?.Invoke(this, tableControl.Table);

            // Đóng form sơ đồ
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #region Drag and Drop Logic - WITH COLLISION DETECTION

        private bool _isDragging = false;
        private Point _dragStartPoint;
        private TableControl _draggedControl;
        private Point _lastValidPosition; // Lưu vị trí hợp lệ cuối cùng

        private void TableControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_isEditMode || e.Button != MouseButtons.Left) return;

            // Lấy TableControl từ sender (có thể là Panel hoặc Label bên trong)
            Control control = sender as Control;
            _draggedControl = control as TableControl ?? control?.Parent as TableControl;

            if (_draggedControl != null)
            {
                _isDragging = true;

                // Lưu vị trí click tương đối với TableControl
                if (control is TableControl)
                {
                    _dragStartPoint = e.Location;
                }
                else
                {
                    // Nếu click vào control con, chuyển đổi tọa độ
                    _dragStartPoint = _draggedControl.PointToClient(control.PointToScreen(e.Location));
                }

                // Lưu vị trí hợp lệ hiện tại
                _lastValidPosition = _draggedControl.Location;

                _draggedControl.Cursor = Cursors.SizeAll;
                _draggedControl.BringToFront();
            }
        }

        private void TableControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _draggedControl == null) return;

            var canvasSize = _floorDimensions[_currentFloor];

            // Lấy vị trí chuột trong tọa độ của canvas
            Control control = sender as Control;
            Point mousePos;

            if (control is TableControl)
            {
                mousePos = pnlCanvas.PointToClient(_draggedControl.PointToScreen(e.Location));
            }
            else
            {
                // Nếu di chuyển trên control con
                mousePos = pnlCanvas.PointToClient(control.PointToScreen(e.Location));
            }

            // Tính toán vị trí mới
            int newX = mousePos.X - _dragStartPoint.X;
            int newY = mousePos.Y - _dragStartPoint.Y;

            // Giới hạn trong canvas
            newX = Math.Max(0, Math.Min(newX, canvasSize.Width - _draggedControl.Width));
            newY = Math.Max(0, Math.Min(newY, canvasSize.Height - _draggedControl.Height));

            // Snap to grid (mỗi 10px để dễ căn chỉnh)
            newX = (newX / 10) * 10;
            newY = (newY / 10) * 10;

            // Kiểm tra va chạm với các bàn khác
            Rectangle proposedBounds = new Rectangle(newX, newY, _draggedControl.Width, _draggedControl.Height);

            if (!HasCollision(proposedBounds, _draggedControl))
            {
                // Không có va chạm - cho phép di chuyển
                _draggedControl.Location = new Point(newX, newY);
                _lastValidPosition = _draggedControl.Location;

                // Đổi cursor để báo hiệu OK
                _draggedControl.Cursor = Cursors.SizeAll;
            }
            else
            {
                // Có va chạm - đổi cursor để báo hiệu không thể di chuyển
                _draggedControl.Cursor = Cursors.No;
                // Không di chuyển, giữ nguyên vị trí cũ
            }
        }

        private void TableControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_draggedControl != null)
            {
                // Nếu vị trí cuối cùng bị va chạm, đưa về vị trí hợp lệ cuối cùng
                Rectangle finalBounds = new Rectangle(
                    _draggedControl.Left,
                    _draggedControl.Top,
                    _draggedControl.Width,
                    _draggedControl.Height
                );

                if (HasCollision(finalBounds, _draggedControl))
                {
                    _draggedControl.Location = _lastValidPosition;
                }

                _draggedControl.Cursor = Cursors.Hand;
            }

            _isDragging = false;
            _draggedControl = null;
        }

        #endregion

        #region Collision Detection
        private bool HasCollision(Rectangle proposedBounds, TableControl currentControl)
        {
            // Thêm buffer 5px để tránh bàn sát nhau quá
            const int BUFFER = 5;
            Rectangle bufferedBounds = new Rectangle(
                proposedBounds.X - BUFFER,
                proposedBounds.Y - BUFFER,
                proposedBounds.Width + (BUFFER * 2),
                proposedBounds.Height + (BUFFER * 2)
            );

            foreach (var kvp in _tableControls)
            {
                var otherControl = kvp.Value;

                // Bỏ qua bàn đang kéo
                if (otherControl == currentControl)
                    continue;

                Rectangle otherBounds = new Rectangle(
                    otherControl.Left,
                    otherControl.Top,
                    otherControl.Width,
                    otherControl.Height
                );

                // Kiểm tra va chạm
                if (bufferedBounds.IntersectsWith(otherBounds))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Event Handlers

        private void FloorTab_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            _currentFloor = button.Tag.ToString();

            // Cập nhật style cho các tab
            foreach (Control ctrl in pnlFloorTabs.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn == button)
                    {
                        btn.BackColor = Color.FromArgb(99, 102, 241);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(226, 232, 240);
                        btn.ForeColor = Color.FromArgb(51, 65, 85);
                    }
                }
            }

            RenderDiagram();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            _isEditMode = true;
            btnEdit.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            pnlInstructions.Visible = true;

            RenderDiagram();
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                var canvasSize = _floorDimensions[_currentFloor];
                var updates = new List<(int maBan, int viTriX, int viTriY)>();

                // Thu thập vị trí mới của tất cả bàn
                foreach (var kvp in _tableControls)
                {
                    var control = kvp.Value;
                    var table = control.Table;

                    // Chuyển từ pixel sang phần trăm
                    int viTriX = (int)((control.Left / (double)canvasSize.Width) * 100);
                    int viTriY = (int)((control.Top / (double)canvasSize.Height) * 100);

                    updates.Add((table.MaBan, viTriX, viTriY));

                    // Cập nhật trong memory
                    table.ViTriX = viTriX;
                    table.ViTriY = viTriY;
                }

                // Lưu vào database
                foreach (var update in updates)
                {
                    var table = _allTables.FirstOrDefault(t => t.MaBan == update.maBan);
                    if (table != null)
                    {
                        table.ViTriX = update.viTriX;
                        table.ViTriY = update.viTriY;
                        await _banBiaService.UpdateTableAsync(table);
                    }
                }

                this.Cursor = Cursors.Default;
                MessageBox.Show("Đã lưu vị trí bàn thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Thoát chế độ chỉnh sửa
                ExitEditMode();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Hủy các thay đổi chưa lưu?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ExitEditMode();
                RenderDiagram(); // Vẽ lại với dữ liệu cũ
            }
        }

        private void ExitEditMode()
        {
            _isEditMode = false;
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            pnlInstructions.Visible = false;

            RenderDiagram();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                var result = MessageBox.Show("Có thay đổi chưa lưu. Bạn có chắc muốn đóng?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            this.Close();
        }

        #endregion
    }

    #region TableControl - Custom Control cho mỗi bàn

    public class TableControl : Panel
    {
        public BanBium Table { get; private set; }
        public event EventHandler TableClicked;

        private Label _lblName;
        private bool _isEditMode;

        public TableControl(BanBium table, bool isEditMode)
        {
            Table = table;
            _isEditMode = isEditMode;

            InitializeControl();
        }

        private void InitializeControl()
        {
            // ============================================================
            // Panel chính - Simple box design
            // ============================================================
            this.Size = new Size(100, 60);
            this.BackColor = GetBackgroundColor();
            this.Cursor = _isEditMode ? Cursors.SizeAll : Cursors.Hand;

            // Border theo trạng thái
            this.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Border màu theo trạng thái, đậm hơn
                var borderColor = GetBorderColor();
                using (var pen = new Pen(borderColor, 3))
                {
                    g.DrawRectangle(pen, 1, 1, this.Width - 3, this.Height - 3);
                }
            };

            // ============================================================
            // Tên bàn - Centered trong box
            // ============================================================
            _lblName = new Label
            {
                Text = Table.TenBan,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(100, 60),
                Location = new Point(0, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = GetTextColor(),
                BackColor = Color.Transparent
            };

            this.Controls.Add(_lblName);

            // ============================================================
            // Click events (chỉ khi KHÔNG ở chế độ edit)
            // ============================================================
            if (!_isEditMode)
            {
                this.Click += (s, e) => TableClicked?.Invoke(this, e);
                _lblName.Click += (s, e) => TableClicked?.Invoke(this, e);

                // Hover effect
                this.MouseEnter += OnMouseEnterCard;
                this.MouseLeave += OnMouseLeaveCard;
                _lblName.MouseEnter += OnMouseEnterCard;
                _lblName.MouseLeave += OnMouseLeaveCard;
            }
        }

        private void OnMouseEnterCard(object sender, EventArgs e)
        {
            // Làm tối màu nền khi hover
            var currentColor = this.BackColor;
            this.BackColor = Color.FromArgb(
                Math.Max(0, currentColor.R - 20),
                Math.Max(0, currentColor.G - 20),
                Math.Max(0, currentColor.B - 20)
            );
        }

        private void OnMouseLeaveCard(object sender, EventArgs e)
        {
            this.BackColor = GetBackgroundColor();
        }

        private Color GetBackgroundColor()
        {
            return Table.TrangThai switch
            {
                "Trống" => Color.FromArgb(220, 252, 231),      // Light green
                "Đang chơi" => Color.FromArgb(254, 226, 226),  // Light red
                "Đã đặt" => Color.FromArgb(254, 249, 195),     // Light yellow
                _ => Color.White
            };
        }

        private Color GetBorderColor()
        {
            return Table.TrangThai switch
            {
                "Trống" => Color.FromArgb(34, 197, 94),        // Green
                "Đang chơi" => Color.FromArgb(239, 68, 68),    // Red
                "Đã đặt" => Color.FromArgb(234, 179, 8),       // Yellow
                _ => Color.Gray
            };
        }

        private Color GetTextColor()
        {
            return Table.TrangThai switch
            {
                "Trống" => Color.FromArgb(22, 101, 52),        // Dark green
                "Đang chơi" => Color.FromArgb(153, 27, 27),    // Dark red
                "Đã đặt" => Color.FromArgb(146, 64, 14),       // Dark yellow
                _ => Color.FromArgb(30, 41, 59)
            };
        }
    }

    #endregion
}