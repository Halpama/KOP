using Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ComponentOrdersList
{
    public partial class OrderEditForm : UserControl
    {
        private Order currentOrder;
        private string _citiesFilePath;

        public OrderEditForm()
        {
            InitializeComponent();
            currentOrder = new Order();
            _citiesFilePath = Path.Combine(Application.StartupPath, "cities.txt");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadCitiesOnStartup();
        }

        private void LoadCitiesOnStartup()
        {
            cmbCities.Items.Clear();

            if (!File.Exists(_citiesFilePath))
                return;

            var lines = File.ReadAllLines(_citiesFilePath)
                            .Select(l => l.Trim())
                            .Where(l => !string.IsNullOrEmpty(l))
                            .ToList();

            foreach (var city in lines)
                cmbCities.Items.Add(city);

            if (cmbCities.Items.Count > 0)
                cmbCities.SelectedIndex = 0;
        }

        public void Initialize(Order order, List<string> availableCities)
        {
            currentOrder = order ?? new Order();

            // имя
            txtCustomerName.Text = currentOrder.CustomerName ?? "";

            // города
            if (availableCities != null)
            {
                cmbCities.Items.Clear();
                foreach (var city in availableCities)
                    cmbCities.Items.Add(city);
            }

            if (!string.IsNullOrEmpty(currentOrder.DestinationCity))
                cmbCities.SelectedItem = currentOrder.DestinationCity;

            // отметки движения
            lstMarks.Items.Clear();
            foreach (var m in currentOrder.MovementMarks)
                lstMarks.Items.Add(m);

            // дата
            dtpReceiptDate.Value = currentOrder.ReceiptDate;
        }

        public event Action<Order> OrderSaved;

        private void btnAddMark_Click(object sender, EventArgs e)
        {
            string mark = txtNewMark.Text.Trim();

            if (string.IsNullOrWhiteSpace(mark))
            {
                MessageBox.Show("Введите текст отметки!");
                return;
            }

            if (lstMarks.Items.Count >= 6)
            {
                MessageBox.Show("Нельзя добавить более 6 отметок!");
                return;
            }

            lstMarks.Items.Add(mark);
            txtNewMark.Clear();
        }

        private void btnRemoveMark_Click(object sender, EventArgs e)
        {
            if (lstMarks.SelectedIndex >= 0)
            {
                lstMarks.Items.RemoveAt(lstMarks.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Выберите отметку для удаления!");
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            string customerName = txtCustomerName.Text.Trim();

            if (string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("Введите ФИО заказчика!");
                return;
            }

            if (cmbCities.SelectedItem == null)
            {
                MessageBox.Show("Выберите город назначения!");
                return;
            }

            try
            {
                // Валидация даты
                DateTime selectedDate = dtpReceiptDate.Value.Date;
                DateTime min = DateTime.Now.Date.AddDays(1);
                DateTime max = DateTime.Now.Date.AddDays(3);

                if (selectedDate < min || selectedDate > max)
                {
                    MessageBox.Show("Дата получения должна быть в пределах 1–3 дней от текущей!");
                    return;
                }

                currentOrder.CustomerName = customerName;
                currentOrder.DestinationCity = cmbCities.SelectedItem.ToString();
                currentOrder.MovementMarks = lstMarks.Items.Cast<string>().ToList();
                currentOrder.ReceiptDate = selectedDate;

                SaveOrderToFile();

                OrderSaved?.Invoke(currentOrder);

                if (this.Parent is Form parentForm)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveOrderToFile()
        {
            try
            {
                string ordersFilePath = Path.Combine(Application.StartupPath, "orders.txt");
                var orders = new List<Order>();

                // читаем старые заказы
                if (File.Exists(ordersFilePath))
                {
                    var lines = File.ReadAllLines(ordersFilePath);
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 4)
                            {
                                var order = new Order
                                {
                                    OrderId = int.Parse(parts[0]),
                                    CustomerName = parts[1],
                                    DestinationCity = parts[2],
                                    ReceiptDate = DateTime.Parse(parts[3]),
                                    MovementMarks = parts.Length > 4 ? parts[4].Split(',').Select(s => s.Trim()).ToList() : new List<string>()
                                };
                                orders.Add(order);
                            }
                        }
                    }
                }

                // если новый заказ — присвоить ID
                if (currentOrder.OrderId == 0)
                {
                    currentOrder.OrderId = orders.Count > 0 ? orders.Max(o => o.OrderId) + 1 : 1;
                }

                // заменить, если уже существует
                var existingIndex = orders.FindIndex(o => o.OrderId == currentOrder.OrderId);
                if (existingIndex >= 0)
                {
                    orders[existingIndex] = currentOrder;
                }
                else
                {
                    orders.Add(currentOrder);
                }

                // сохраняем
                var linesToSave = orders.Select(o =>
                    $"{o.OrderId}|{o.CustomerName}|{o.DestinationCity}|{o.ReceiptDate:yyyy-MM-dd}|{string.Join(", ", o.MovementMarks)}");

                File.WriteAllLines(ordersFilePath, linesToSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
