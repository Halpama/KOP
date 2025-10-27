using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ComponentCities
{
    public partial class CitiesControl : UserControl
    {
        private readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cities.txt");

        public CitiesControl()
        {
            InitializeComponent();

            dgvCities.AllowUserToAddRows = false;
            dgvCities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCities.Columns.Clear();
            dgvCities.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Город назначения",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            LoadData();

            dgvCities.KeyDown += DgvCities_KeyDown;
            dgvCities.CellEndEdit += DgvCities_CellEndEdit;
        }

        private void LoadData()
        {
            dgvCities.Rows.Clear();
            if (!File.Exists(filePath)) return;

            var lines = File.ReadAllLines(filePath)
                            .Select(l => l.Trim())
                            .Where(l => !string.IsNullOrEmpty(l));

            foreach (var line in lines)
                dgvCities.Rows.Add(line);
        }

        private void SaveData()
        {
            var lines = dgvCities.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.Cells[0].Value?.ToString()?.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .ToArray();

            File.WriteAllLines(filePath, lines);
        }

        private void DgvCities_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var val = dgvCities.Rows[e.RowIndex].Cells[0].Value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(val))
            {
                MessageBox.Show("Пустые записи сохранять нельзя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvCities.Rows.RemoveAt(e.RowIndex);
                return;
            }

            SaveData();
        }

        private void DgvCities_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                dgvCities.Rows.Add();
                var idx = dgvCities.Rows.Count - 1;
                dgvCities.CurrentCell = dgvCities.Rows[idx].Cells[0];
                dgvCities.BeginEdit(true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (dgvCities.IsCurrentCellInEditMode)
                    dgvCities.EndEdit();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (dgvCities.SelectedRows.Count == 0) return;

                var res = MessageBox.Show("Удалить выбранные города?", "Подтвердите", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    var rows = dgvCities.SelectedRows.Cast<DataGridViewRow>()
                                   .OrderByDescending(r => r.Index)
                                   .ToList();

                    foreach (var r in rows)
                        dgvCities.Rows.RemoveAt(r.Index);

                    SaveData();
                }

                e.Handled = true;
            }
        }

        public List<string> GetCities()
        {
            var cities = new List<string>();
            foreach (DataGridViewRow row in dgvCities.Rows)
            {
                if (row.Cells[0].Value != null)
                    cities.Add(row.Cells[0].Value.ToString());
            }
            return cities;
        }
    }
}
