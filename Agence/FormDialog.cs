using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agence
{
    public partial class FormDialog : Form
    {
        public FormDialog()
        {
            InitializeComponent();

            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
        }

        public void SetMaimized()
        {

            this.WindowState = FormWindowState.Maximized;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            //this.FormBorderStyle = FormBorderStyle.None;
            //this.ShowInTaskbar = false;
        }

        private void FormDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
