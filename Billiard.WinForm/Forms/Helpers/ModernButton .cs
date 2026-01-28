using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class ModernButton : Button
{
    public Color BackgroundColor { get; set; } = Color.FromArgb(99, 102, 241);
    public Color HoverColor { get; set; } = Color.FromArgb(79, 70, 229);
    public int BorderRadius { get; set; } = 12;

    private bool isHovering = false;

    public ModernButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 10, FontStyle.Bold);
        Cursor = Cursors.Hand;
        Height = 45;
        Width = 150;

        MouseEnter += (s, e) => { isHovering = true; Invalidate(); };
        MouseLeave += (s, e) => { isHovering = false; Invalidate(); };
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = ClientRectangle;
        using (GraphicsPath path = GetRoundedRect(rect, BorderRadius))
        {
            using (SolidBrush brush = new SolidBrush(isHovering ? HoverColor : BackgroundColor))
            {
                pevent.Graphics.FillPath(brush, path);
            }

            TextRenderer.DrawText(
                pevent.Graphics,
                Text,
                Font,
                rect,
                ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }
    }

    private GraphicsPath GetRoundedRect(Rectangle r, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;

        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();

        return path;
    }
}
