using Agence.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public partial class UcRegistre : UserControl
    {
        IMapper Mapper;
        ContextMenuStrip _contextMenuStrip = new ContextMenuStrip();
        public int _elementId { get; set; }

        public UcRegistre()
        {
            InitializeComponent();

            Commun.InitDataGridView(dgList);

            //DataRow dr = dt.NewRow();
            //dr["ActeCategorieID"] = 0;
            //dr["ActeCategorieLibelle"] = "TOUS";
            //dt.Rows.InsertAt(dr, 0);
            //    cbCategory.DisplayMember = "ActeCategorieLibelle";
            //    cbCategory.ValueMember = "ActeCategorieID";
            //    cbCategory.DataSource = dt;
        }

        private void UcRegistre_Load(object sender, EventArgs e)
        {
            nudAnnee.Value = DateTime.Now.Year;
            GridFill();

            ContextMenuStripFill();

            dgList.MouseClick += DataGridView_MouseClick;
            dgList.CellDoubleClick += DataGridView_CellDoubleClick;

            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Registre, Registre>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            }).CreateMapper();
        }

        async void GridFill()
        {
            using (var context = new Context())
            {
                var registres = await context.Registres.OrderByDescending(x => x.Annee).ThenByDescending(x => x.NOrdre).ToListAsync();
                dgList.DataSource = registres;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            switch (button.Name)
            {
                case "btSave":
                    CreateOrUpdate();
                    break;

                case "btCancel":
                    ClearFields();
                    break;
            }
        }

        async void CreateOrUpdate()
        {
            try
            {
                if (nudAnnee.Value >= 2000)
                {
                    if (!string.IsNullOrWhiteSpace(tbNAffaire.Text))
                    {
                        var natureId = cbNature.SelectedValue == null || (int)cbNature.SelectedValue == 0 ? (int?)null : (int)cbNature.SelectedValue;
                        var entrepriseId = cbNomEntreprise.SelectedValue == null || (int)cbNomEntreprise.SelectedValue == 0 ? (int?)null : (int)cbNomEntreprise.SelectedValue;
                        var datePaiement = string.IsNullOrWhiteSpace(dtpDatePaiement.Text) ? (DateTime?)null : dtpDatePaiement.Value;
                        var dateReception = string.IsNullOrWhiteSpace(dtpDateReception.Text) ? (DateTime?)null : dtpDateReception.Value;
                        var dateAffectation = string.IsNullOrWhiteSpace(dtpDateAffectation.Text) ? (DateTime?)null : dtpDateAffectation.Value;
                        var dateRemise = string.IsNullOrWhiteSpace(dtpDateRemise.Text) ? (DateTime?)null : dtpDateRemise.Value;

                        var registre = new Registre
                        {
                            Id = _elementId,

                            NOrdre = (int)nudNOrdre.Value,

                            Annee = (int)nudAnnee.Value,

                            NAffaire = tbNAffaire.Text,

                            NomClient = tbNomClient.Text,

                            AdresseClient = tbAdresseClient.Text,

                            NatureId = natureId,

                            DatePaiement = datePaiement,

                            DateReception = dateReception,

                            EntrepriseId = entrepriseId,

                            DateAffectation = dateAffectation,

                            DateRemise = dateRemise,

                            Realise = cbRealise.Checked,

                            Observation = tbObservation.Text
                        };

                        using (var context = new Context())
                        {
                            if (registre.Id == 0)
                            {
                                context.Registres.Add(registre);
                                await context.SaveChangesAsync();

                                Commun.Alert(Commun.succesSave, Form_Alert.enmType.Success);

                                ClearFields();
                                GridFill();
                            }
                            else
                            {
                                var registreInDB = await context.Registres.FindAsync(registre.Id);
                                if (registreInDB != null)
                                {
                                    Mapper.Map(registre, registreInDB);
                                    await context.SaveChangesAsync();

                                    Commun.Alert(Commun.succesUpdate, Form_Alert.enmType.Success);

                                    ClearFields();
                                    GridFill();
                                }
                                else
                                    Commun.Alert(Commun.elementNotFound + " " + registre.NOrdre, Form_Alert.enmType.Warning);
                            }
                        };
                    }
                    else
                    {
                        MessageBox.Show("Veuillez saisir le Numéro d'affaire !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir l'année SVP !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Commun.WriteLog(ex.Message, GetType().Name, new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            }
        }

        void ClearFields()
        {
            _elementId = 0;

            nudNOrdre.Value = 1;

            tbNAffaire.Text = "";

            tbNomClient.Text = "";

            tbAdresseClient.Text = "";

            dtpDatePaiement.CustomFormat = " ";

            dtpDateAffectation.CustomFormat = " ";

            dtpDateRemise.CustomFormat = " ";

            cbNomEntreprise.SelectedValue = 0;

            cbRealise.Checked = false;

            tbObservation.Text = "";

            btSave.Text = Commun.save;
        }

        private async void DeleteElement()
        {
            try
            {
                if (dgList.SelectedRows.Count > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Voullez vous supprimer ce registre " + dgList.SelectedRows[0].Cells["OrdreAnnee"].Value + " ? ", "Suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var id = Convert.ToInt32(dgList.SelectedRows[0].Cells["Id"].Value);
                        var nOrdre = dgList.SelectedRows[0].Cells["OrdreAnnee"].Value;

                        using (var context = new Context())
                        {
                            var registre = await context.Registres.FindAsync(id);
                            if (registre != null)
                            {
                                context.Remove(registre);
                                await context.SaveChangesAsync();

                                Commun.Alert(Commun.succesDelete, Form_Alert.enmType.Info);
                            }
                            else
                                Commun.Alert(Commun.elementNotFound + " " + nOrdre, Form_Alert.enmType.Warning);

                            ClearFields();
                            GridFill();
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Commun.WriteLog(ex.Message, GetType().Name, new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            }
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowElement();
        }

        private void ContextMenuStripFill()
        {
            try
            {
                _contextMenuStrip.Items.AddRange(new ToolStripItem[] {
                    //new ToolStripSeparator(),
                    new ToolStripMenuItem(){Name = "updateMenuItem", Text = "Modifier", Image = Properties.Resources.update},
                    new ToolStripMenuItem(){Name = "deleteMenuItem", Text = "Supprimer", Image = Properties.Resources.delete}
                });

                _contextMenuStrip.Font = new Font("Segoe UI", 12F);

                _contextMenuStrip.ItemClicked += ContextMenuStrip1_ItemClicked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Commun.WriteLog(ex.Message, GetType().Name, new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            }
        }

        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((ContextMenuStrip)sender).Visible = false; DataGridViewAction(e.ClickedItem.Name);
        }

        private void DataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                var dataGridView = (DataGridView)sender;
                var currentMouseOverRow = dataGridView.HitTest(e.X, e.Y).RowIndex;
                //show contextMenuStrip
                if (currentMouseOverRow >= 0) if (e.Button == MouseButtons.Right)
                    {
                        dataGridView.Rows[currentMouseOverRow].Selected = true;
                        _contextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y);
                    }
                    else
                    {
                        DataGridViewAction(dataGridView.CurrentCell.OwningColumn.Name);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Commun.WriteLog(ex.Message, GetType().Name, new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            }
        }

        private void DataGridViewAction(string val)
        {
            switch (val)
            {
                case "UpdateColumn":
                case "updateMenuItem":
                    ShowElement();
                    break;

                case "DeleteColumn":
                case "deleteMenuItem":
                    DeleteElement();
                    break;

                default:
                    break;
            }
        }

        async void ShowElement()
        {
            try
            {
                if (dgList.SelectedRows.Count > 0)
                {
                    var id = Convert.ToInt32(dgList.SelectedRows[0].Cells["Id"].Value);
                    var NOrdre = dgList.SelectedRows[0].Cells["OrdreAnnee"].Value;

                    using (var context = new Context())
                    {
                        var registre = await context.Registres.FindAsync(id);
                        if (registre != null)
                        {
                            btSave.Text = Commun.update;

                            _elementId = registre.Id;

                            nudNOrdre.Value = registre.NOrdre;

                            nudAnnee.Value = registre.Annee;

                            tbNAffaire.Text = registre.NAffaire;

                            tbNomClient.Text = registre.NomClient;

                            tbAdresseClient.Text = registre.AdresseClient;

                            cbNature.SelectedValue = registre.NatureId ?? 0;

                            if (registre.DatePaiement == null)
                                dtpDatePaiement.CustomFormat = " ";
                            else
                            {
                                dtpDatePaiement.CustomFormat = "dd/MM/yyyy";
                                dtpDatePaiement.Value = registre.DatePaiement.Value;
                            }

                            if (registre.DateReception == null)
                                dtpDateReception.CustomFormat = " ";
                            else
                            {
                                dtpDateReception.CustomFormat = "dd/MM/yyyy";
                                dtpDateReception.Value = registre.DateReception.Value;
                            }

                            if (registre.DateAffectation == null)
                                dtpDateAffectation.CustomFormat = " ";
                            else
                            {
                                dtpDateAffectation.CustomFormat = "dd/MM/yyyy";
                                dtpDateAffectation.Value = registre.DateAffectation.Value;
                            }

                            if (registre.DateRemise == null)
                                dtpDateRemise.CustomFormat = " ";
                            else
                            {
                                dtpDateRemise.CustomFormat = "dd/MM/yyyy";
                                dtpDateRemise.Value = registre.DateRemise.Value;
                            }

                            cbNomEntreprise.SelectedValue = registre.EntrepriseId ?? 0;

                            cbRealise.Checked = registre.Realise;

                            tbObservation.Text = registre.Observation;
                        }
                        else
                        {
                            Commun.Alert(Commun.elementNotFound + " " + NOrdre, Form_Alert.enmType.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Commun.WriteLog(ex.Message, GetType().Name, new System.Diagnostics.StackFrame(0, true).GetFileLineNumber());
            }
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToInt16(Keys.Enter))
            {
                GridFill();
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            ((DateTimePicker)sender).CustomFormat = "dd/MM/yyyy";
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                ((DateTimePicker)sender).CustomFormat = " ";

            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                GridFill();
            }
            else
                ((DateTimePicker)sender).CustomFormat = "dd/MM/yyyy";
        }
    }
}
