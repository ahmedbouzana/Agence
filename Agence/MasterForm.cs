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
    public partial class MasterForm : Form
    {
        public MasterForm()
        {
            InitializeComponent();
        }

        private void MasterForm_Load(object sender, EventArgs e)
        {
            var uc = new UcRegistre();
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc);
            panel1.Tag = uc;
            uc.Show();

            //panel1.Controls[uc.Name].BringToFront();
    }
    }
}
