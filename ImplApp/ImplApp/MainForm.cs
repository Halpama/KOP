using Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ImplApp
{
    public partial class MainForm : Form
    {
        private readonly List<IComponentContract> loadedComponents = new();
        private string _componentsFolder = @"C:\\KOP\\ComponentsDll";
        private string licenseLevel = "ADVANCED";

        public MainForm()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            InitializeComponent();

            if (tabControl1 == null)
            {
                tabControl1 = new TabControl { Dock = DockStyle.Fill };
                Controls.Add(tabControl1);
            }
            tabControl1.TabPages.Clear();
            tabControl1.DoubleClick += TabControl1_DoubleClick;

            Text = "Система управления заказами (вариант 21)";
            Size = new Size(1300, 800);

            LoadComponents();
            BuildDynamicMenu();
            ApplyLicenseRestrictions();
        }

        private void TabControl1_DoubleClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;
            string assemblyPath = Path.Combine(_componentsFolder, assemblyName + ".dll");
            return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
        }

        private void LoadComponents()
        {
            string configPath = Path.Combine(Application.StartupPath, "config.json");
            if (!File.Exists(configPath))
            {
                MessageBox.Show($"Файл конфигурации не найден:\n{configPath}");
                return;
            }

            try
            {
                dynamic config = JsonConvert.DeserializeObject(File.ReadAllText(configPath));
                string componentsFolder = config.ComponentsFolder ?? _componentsFolder;
                string licenseFileValue = config.LicenseFile ?? "license.txt";
                string licenseFile = Path.IsPathRooted(licenseFileValue)
                    ? licenseFileValue
                    : Path.Combine(componentsFolder, licenseFileValue);

                _componentsFolder = componentsFolder;

                if (!Directory.Exists(componentsFolder) || !File.Exists(licenseFile))
                {
                    MessageBox.Show("Папка компонентов или файл лицензии не найден.");
                    return;
                }

                string licenseText = File.ReadAllText(licenseFile).ToUpperInvariant();
                licenseLevel = licenseText.Contains("LEVEL=ADVANCED") ? "ADVANCED"
                            : licenseText.Contains("LEVEL=BASE") ? "BASE"
                            : "MINIMAL";

                // уровни доступа теперь привязаны к городам и отчётам по дате
                var allowedComponentIds = licenseLevel switch
                {
                    "MINIMAL" => new List<string> { "Cities" },
                    "BASE" => new List<string> { "Cities", "OrdersList" },
                    "ADVANCED" => new List<string> { "Cities", "OrdersList", "OrdersReceiptDateReport" },
                    _ => new List<string>()
                };

                loadedComponents.Clear();
                foreach (string dllPath in Directory.GetFiles(componentsFolder, "*.dll"))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dllPath);
                        var types = assembly.GetTypes()
                            .Where(t => typeof(IComponentContract).IsAssignableFrom(t)
                                        && !t.IsInterface && !t.IsAbstract && t.IsPublic);

                        foreach (var type in types)
                        {
                            IComponentContract component = (IComponentContract)Activator.CreateInstance(type);
                            if (allowedComponentIds.Any(x => x.Equals(component.Id, StringComparison.OrdinalIgnoreCase)))
                                loadedComponents.Add(component);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Ошибка загрузки {Path.GetFileName(dllPath)}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки компонентов:\n{ex.Message}");
            }
        }

        private void BuildDynamicMenu()
        {
            directoriesToolStripMenuItem.DropDownItems.Clear();
            reportsToolStripMenuItem.DropDownItems.Clear();

            foreach (var component in loadedComponents)
            {
                var menuItem = new ToolStripMenuItem { Text = component.Title };
                menuItem.Click += (s, e) => OpenComponentInTab(component.Id);

                if (component.Id.Equals("OrdersReceiptDateReport", StringComparison.OrdinalIgnoreCase))
                    reportsToolStripMenuItem.DropDownItems.Add(menuItem);
                else
                    directoriesToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private void ApplyLicenseRestrictions()
        {
            directoriesToolStripMenuItem.Enabled = directoriesToolStripMenuItem.DropDownItems.Count > 0;
            reportsToolStripMenuItem.Enabled = reportsToolStripMenuItem.DropDownItems.Count > 0;
        }

        private void OpenComponentInTab(string componentId)
        {
            var component = loadedComponents.FirstOrDefault(c =>
                c.Id.Equals(componentId, StringComparison.OrdinalIgnoreCase));

            if (component == null)
            {
                MessageBox.Show($"Компонент '{componentId}' недоступен при лицензии {licenseLevel}");
                return;
            }

            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Text == component.Title)
                {
                    tabControl1.SelectedTab = page;
                    return;
                }
            }

            var control = component.GetControl();
            if (control == null)
            {
                MessageBox.Show($"Не удалось создать компонент '{componentId}'");
                return;
            }

            // устанавливаем источник городов
            if (component.Id.Equals("OrdersList", StringComparison.OrdinalIgnoreCase))
            {
                var ordersList = control as ComponentOrdersList.OrdersListControl;
                if (ordersList != null)
                {
                    var citiesComp = loadedComponents.FirstOrDefault(c => c.Id.Equals("Cities", StringComparison.OrdinalIgnoreCase));
                    if (citiesComp != null)
                    {
                        var citiesCtrl = citiesComp.GetControl() as ComponentCities.CitiesControl;
                        if (citiesCtrl != null)
                            ordersList.SetCitiesSource(citiesCtrl);
                    }
                }
            }

            var tabPage = new TabPage { Text = component.Title };
            control.Dock = DockStyle.Fill;
            tabPage.Controls.Add(control);
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
            control.Focus();
        }

        public void RefreshAllComponents()
        {
            try
            {
                var ordersList = loadedComponents.FirstOrDefault(c => c.Id.Equals("OrdersList", StringComparison.OrdinalIgnoreCase));
                ordersList?.GetControl().GetType().GetMethod("RefreshFromFile")?.Invoke(ordersList.GetControl(), null);

                var report = loadedComponents.FirstOrDefault(c => c.Id.Equals("OrdersReceiptDateReport", StringComparison.OrdinalIgnoreCase));
                report?.GetControl().GetType().GetMethod("RefreshFromFile")?.Invoke(report.GetControl(), null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка обновления: {ex.Message}");
            }
        }
    }
}
