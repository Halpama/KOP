namespace ComponentOrdersReport
{
    public partial class OrdersReportControl : UserControl
    {
        private List<Order> allOrders = new List<Order>();
        private List<Order> filteredOrders = new List<Order>();

        public OrdersReportControl()
        {
            InitializeComponent();
            InitializeReport();
            LoadOrdersFromFile();
        }

        private void InitializeReport()
        {
            dgvOrdersReport.AllowUserToAddRows = false;
            dgvOrdersReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrdersReport.ReadOnly = true;
            dgvOrdersReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            SetupDataGridViewColumns();
            SubscribeToEvents();
            ApplyFilters();
        }

        private void SetupDataGridViewColumns()
        {
            dgvOrdersReport.Columns.Clear();

            dgvOrdersReport.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderId",
                HeaderText = "ID",
                DataPropertyName = "OrderId",
                Visible = true
            });

            dgvOrdersReport.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerName",
                HeaderText = "ФИО",
                DataPropertyName = "CustomerName"
            });

            dgvOrdersReport.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DestinationCity",
                HeaderText = "Город назначения",
                DataPropertyName = "DestinationCity"
            });

            dgvOrdersReport.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReceiptDate",
                HeaderText = "Дата получения",
                DataPropertyName = "ReceiptDate"
            });
        }

        private void SubscribeToEvents()
        {
            dtpReceiptFilter.ValueChanged += (s, e) => ApplyFilters();
        }

        private void ApplyFilters()
        {
            DateTime selectedDate = dtpReceiptFilter.Value.Date;
            filteredOrders = allOrders
                .Where(o => o.ReceiptDate.Date == selectedDate)
                .ToList();

            dgvOrdersReport.DataSource = null;
            dgvOrdersReport.DataSource = filteredOrders;

            lblTotalOrders.Text = $"Всего заказов: {filteredOrders.Count}";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrdersFromFile();
        }

        public void RefreshFromFile()
        {
            LoadOrdersFromFile();
        }

        private void LoadOrdersFromFile()
        {
            try
            {
                string ordersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");
                allOrders.Clear();

                if (File.Exists(ordersFilePath))
                {
                    var lines = File.ReadAllLines(ordersFilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 4)
                        {
                            allOrders.Add(new Order
                            {
                                OrderId = int.Parse(parts[0]),
                                CustomerName = parts[1],
                                DestinationCity = parts[2],
                                ReceiptDate = DateTime.Parse(parts[3])
                            });
                        }
                    }
                }

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Order> GetFilteredOrders() => new List<Order>(filteredOrders);
    }
}
