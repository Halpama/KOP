using Contract;
using Contract.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImplApp
{
    public partial class ExtensionsFormVariant : Form
    {
        private readonly string _pluginsFolder;

        private readonly List<IReportDocumentWithContextTablesContract> _pdfPlugins = new();
        private readonly List<IReportDocumentWithChartHistogramContract> _wordPlugins = new();

        public ExtensionsFormVariant(string pluginsFolder)
        {
            _pluginsFolder = pluginsFolder;
            InitializeComponent();
            LoadPlugins();
        }

        // === Загрузка плагинов ===
        private void LoadPlugins()
        {
            comboBoxPdf.Items.Clear();
            comboBoxWord.Items.Clear();
            _pdfPlugins.Clear();
            _wordPlugins.Clear();

            if (!Directory.Exists(_pluginsFolder))
            {
                MessageBox.Show($"Папка с плагинами не найдена:\n{_pluginsFolder}");
                return;
            }

            foreach (string dllPath in Directory.GetFiles(_pluginsFolder, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dllPath);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsInterface || type.IsAbstract)
                            continue;

                        if (typeof(IReportDocumentWithContextTablesContract).IsAssignableFrom(type))
                        {
                            var instance = (IReportDocumentWithContextTablesContract)Activator.CreateInstance(type);
                            _pdfPlugins.Add(instance);
                            comboBoxPdf.Items.Add(instance.DocumentFormat);
                            continue;
                        }

                        if (typeof(IReportDocumentWithChartHistogramContract).IsAssignableFrom(type))
                        {
                            var instance = (IReportDocumentWithChartHistogramContract)Activator.CreateInstance(type);
                            _wordPlugins.Add(instance);
                            comboBoxWord.Items.Add(instance.DocumentFormat);
                            continue;
                        }
                    }
                }
                catch (ReflectionTypeLoadException rex)
                {
                    string details = string.Join("\n", rex.LoaderExceptions.Select(x => x.Message));
                    MessageBox.Show($"Ошибка загрузки типов из {Path.GetFileName(dllPath)}:\n{details}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке {Path.GetFileName(dllPath)}:\n{ex.Message}");
                }
            }

            if (comboBoxPdf.Items.Count > 0) comboBoxPdf.SelectedIndex = 0;
            if (comboBoxWord.Items.Count > 0) comboBoxWord.SelectedIndex = 0;
        }

        // === Подготовка данных для PDF таблицы ===
        private List<string[,]> PrepareOrdersTable()
        {
            var result = new List<string[,]>();
            string ordersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");

            if (!File.Exists(ordersPath))
            {
                MessageBox.Show($"Файл заказов не найден:\n{ordersPath}");
                return result;
            }

            // Заголовок таблицы — этапы движения
            string[,] table = new string[1, 6];
            for (int i = 0; i < 6; i++)
                table[0, i] = $"Этап {i + 1}";

            var lines = File.ReadAllLines(ordersPath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    // 3-я позиция — отметки движения (например: «принят, собран, отправлен»)
                    var stages = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    string[,] row = new string[1, 6];
                    for (int i = 0; i < 6; i++)
                        row[0, i] = i < stages.Length ? stages[i] : "";
                    result.Add(row);
                }
            }

            return result;
        }

        // === Подготовка данных для линейной диаграммы в Word ===
        private List<(string Series, string Category, double Value)> PrepareChartData()
        {
            string ordersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");
            var result = new List<(string, string, double)>();

            if (!File.Exists(ordersPath))
            {
                MessageBox.Show($"Файл заказов не найден:\n{ordersPath}");
                return result;
            }

            var data = new Dictionary<string, Dictionary<DateTime, int>>();

            foreach (var line in File.ReadAllLines(ordersPath))
            {
                var parts = line.Split('|');
                if (parts.Length < 5) continue;

                string city = parts[1].Trim();
                if (!DateTime.TryParse(parts[3].Trim(), out DateTime date))
                    continue;

                if (!data.ContainsKey(city))
                    data[city] = new Dictionary<DateTime, int>();

                if (!data[city].ContainsKey(date))
                    data[city][date] = 0;

                data[city][date]++;
            }

            // Преобразуем в список (серия, категория, значение)
            foreach (var city in data)
            {
                foreach (var kv in city.Value.OrderBy(x => x.Key))
                {
                    result.Add((city.Key, kv.Key.ToShortDateString(), kv.Value));
                }
            }

            return result;
        }

        // === Создание PDF отчёта ===
        private async void buttonCreatePdf_Click(object sender, EventArgs e)
        {
            if (comboBoxPdf.SelectedIndex < 0 || _pdfPlugins.Count == 0)
            {
                MessageBox.Show("Выберите плагин для PDF отчёта.");
                return;
            }

            var plugin = _pdfPlugins[comboBoxPdf.SelectedIndex];
            var tables = PrepareOrdersTable();

            if (tables.Count == 0)
            {
                MessageBox.Show("Нет данных для отчёта.");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF файлы (*.pdf)|*.pdf";
                sfd.FileName = "Отчёт_по_движению_заказов.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await plugin.CreateDocumentAsync(sfd.FileName, "Отчёт по продвижению заказов", tables);
                        MessageBox.Show("PDF отчёт успешно создан!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании отчёта:\n{ex.Message}");
                    }
                }
            }
        }

        // === Создание Word отчёта с линейной диаграммой ===
        private async void buttonCreateWord_Click(object sender, EventArgs e)
        {
            if (comboBoxWord.SelectedIndex < 0 || _wordPlugins.Count == 0)
            {
                MessageBox.Show("Выберите плагин для Word отчёта.");
                return;
            }

            var plugin = _wordPlugins[comboBoxWord.SelectedIndex];
            var data = PrepareChartData();

            if (data.Count == 0)
            {
                MessageBox.Show("Нет данных для построения диаграммы.");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Word файлы (*.docx)|*.docx";
                sfd.FileName = "Отчёт_по_заказам_по_городам.docx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await plugin.CreateDocumentAsync(
                            sfd.FileName,
                            "Отчёт по заказам",
                            "Количество заказов по городам и датам",
                            data
                        );
                        MessageBox.Show("Word отчёт с линейной диаграммой успешно создан!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании Word отчёта:\n{ex.Message}");
                    }
                }
            }
        }
    }
}
