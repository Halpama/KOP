using ComponentCities;
using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ComponentOrdersList
{
    public partial class OrdersListControl : UserControl, IComponentContract
    {
        private List<Order> orders = new List<Order>();
        private CitiesControl citiesSource;

        public string Id => "OrdersList";
        public string Title => "Список заказов";
        public string Category => "Directory";

        public OrdersListControl()
        {
            InitializeComponent();
            this.TabStop = true;
            InitializeDataGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
                LoadOrdersFromFile();
        }

        private void InitializeDataGridView()
        {
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.ReadOnly = true;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrders.MultiSelect = false;
            dgvOrders.AutoGenerateColumns = false;
            SetupDataGridViewColumns();
        }

        private void SetupDataGridViewColumns()
        {
            dgvOrders.Columns.Clear();

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                HeaderText = "ID",
                DataPropertyName = "OrderId",
                Visible = false
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DestinationCity",
                HeaderText = "Город назначения",
                DataPropertyName = "DestinationCity"
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                HeaderText = "ФИО заказчика",
                DataPropertyName = "CustomerName"
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReceiptDate",
                HeaderText = "Дата получения",
                DataPropertyName = "ReceiptDate"
            });

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MovementMarksDisplay",
                HeaderText = "Отметки о движении",
                DataPropertyName = "MovementMarksDisplay"
            });
        }

        public void SetCitiesSource(CitiesControl source)
        {
            this.citiesSource = source;
        }

        public UserControl GetControl() => this;

        // --- Горячие клавиши: Ctrl+A/U/D ---
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.A))
            {
                OpenOrderEditor(null);
                return true;
            }
            if (keyData == (Keys.Control | Keys.U))
            {
                var selected = GetSelectedOrder();
                if (selected != null)
                    OpenOrderEditor(selected);
                else
                    MessageBox.Show("Выберите заказ для редактирования.");
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                DeleteOrder();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpenOrderEditor(Order order = null)
        {
            var availableCities = citiesSource?.GetCities() ?? new List<string>();
            if (availableCities.Count == 0)
            {
                MessageBox.Show("Сначала добавьте города в справочник!", "Нет городов",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var form = new Form
            {
                Text = order == null ? "Новый заказ" : $"Редактирование заказа №{order.OrderId}",
                Size = new System.Drawing.Size(620, 450),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            var editor = new OrderEditForm();
            editor.Initialize(order, availableCities);
            editor.OrderSaved += (savedOrder) =>
            {
                AddOrUpdateOrder(savedOrder);
                form.DialogResult = DialogResult.OK;
                form.Close();
            };

            editor.Dock = DockStyle.Fill;
            form.Controls.Add(editor);
            form.ShowDialog();
        }

        public void AddOrUpdateOrder(Order order)
        {
            if (order == null) return;

            if (order.OrderId == 0)
            {
                order.OrderId = orders.Count > 0 ? orders.Max(o => o.OrderId) + 1 : 1;
                orders.Add(order);
            }
            else
            {
                var index = orders.FindIndex(o => o.OrderId == order.OrderId);
                if (index >= 0)
                    orders[index] = order;
                else
                    orders.Add(order);
            }

            SaveOrdersToFile();
            RefreshGrid();
        }

        private void SaveOrdersToFile()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "orders.txt");
                var lines = orders.Select(o =>
                    $"{o.OrderId}|{o.CustomerName}|{o.DestinationCity}|{o.ReceiptDate:yyyy-MM-dd}|{string.Join(", ", o.MovementMarks)}");
                File.WriteAllLines(path, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrdersFromFile()
        {
            orders.Clear();
            string path = Path.Combine(Application.StartupPath, "orders.txt");
            if (!File.Exists(path)) return;

            try
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('|');
                    if (parts.Length >= 4)
                    {
                        var order = new Order
                        {
                            OrderId = int.Parse(parts[0]),
                            CustomerName = parts[1],
                            DestinationCity = parts[2],
                            ReceiptDate = DateTime.Parse(parts[3]),
                            MovementMarks = parts.Length > 4
                                ? parts[4].Split(',').Select(s => s.Trim()).ToList()
                                : new List<string>()
                        };
                        orders.Add(order);
                    }
                }
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGrid()
        {
            dgvOrders.DataSource = null;
            dgvOrders.DataSource = orders;
        }

        public Order GetSelectedOrder()
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                var selectedRow = dgvOrders.SelectedRows[0];
                if (selectedRow.DataBoundItem is Order order)
                    return order;
            }
            return null;
        }

        public void DeleteOrder()
        {
            var selectedOrder = GetSelectedOrder();
            if (selectedOrder != null)
            {
                var result = MessageBox.Show(
                    $"Удалить заказ №{selectedOrder.OrderId}?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    orders.Remove(selectedOrder);
                    SaveOrdersToFile();
                    RefreshGrid();
                    MessageBox.Show("Заказ удалён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RefreshFromFile() => LoadOrdersFromFile();
    }
}
