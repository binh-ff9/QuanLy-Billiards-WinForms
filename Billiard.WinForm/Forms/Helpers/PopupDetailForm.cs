using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billiard.WinForm.Forms.Helpers
{
    public partial class PopupDetailForm : Form
    {
        public PopupDetailForm(Control content, string title)
        {
            this.Text = title;
            this.Size = new Size(550, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            content.Dock = DockStyle.Fill;
            this.Controls.Add(content);
        }
    }
}
