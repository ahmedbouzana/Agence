using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Agence
{
    public class Commun
    {

        static readonly string version = "v1.0";

        public static readonly string save = "Sauvegarder"; 
        public static readonly string update = "Mettre à jour";
        public static readonly string succesSave = "Sauvegarde réussi";
        public static readonly string succesUpdate = "Mise à jour réussie";
        public static readonly string succesDelete = "Suppression avec succès";
        public static readonly string elementNotFound = "Element non trouvé";

        public static void InitDataGridView(DataGridView dataGridView)
        {
            //dataGridView.ReadOnly = true;
            dataGridView.MultiSelect = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.Columns[0].Visible = false;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ReadOnly = true;
            dataGridView.ColumnHeadersHeight = 30;
            dataGridView.ScrollBars = ScrollBars.Both;
            dataGridView.BackgroundColor = Color.WhiteSmoke;
            dataGridView.GridColor = Color.FromArgb(64, 64, 64);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.RowHeadersWidth = 51;
            //dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(64, 64, 64);
            //dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewCellStyle1.SelectionForeColor = Color.WhiteSmoke;
            //dataGridViewCellStyle1.SelectionBackColor = Color.LightSeaGreen;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;

            var dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.BackColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(99, 121, 139);
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
        }

        public static void WriteLog(string strLog, string usercontrol, int stackFrame)
        {
            try
            {
                var logFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                logFilePath = "Log.txt";

                var logFileInfo = new FileInfo(logFilePath);
                var logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                FileStream fileStream = null;
                if (!logFileInfo.Exists)
                    fileStream = logFileInfo.Create();
                else
                    fileStream = new FileStream(logFilePath, FileMode.Append);

                var log = new StreamWriter(fileStream);
                log.WriteLine(string.Join("__", "*", DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "." + stackFrame, version, usercontrol, strLog));
                log.Close();
            }
            catch { }
        }

        public static void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type);
        }
    }
}
