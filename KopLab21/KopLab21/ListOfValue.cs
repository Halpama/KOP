using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Components
{
    public partial class ListOfValue : UserControl
    {
        public ListOfValue()
        {
            InitializeComponent();

            listBox.SelectedIndexChanged += (s, e) =>
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public void FillItems(List<string> values)
        {
            listBox.Items.Clear();

            var uniqueValues = values
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .ToList();

            listBox.Items.AddRange(uniqueValues.ToArray());
        }

        public void ClearItems()
        {
            listBox.Items.Clear();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedValue
        {
            get => listBox.SelectedItem?.ToString() ?? string.Empty;
            set
            {
                if (listBox.Items.Contains(value))
                    listBox.SelectedItem = value;
                else
                    listBox.ClearSelected();
            }
        }

        public event EventHandler ValueChanged;
    }
}
