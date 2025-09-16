using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KopLab21
{
    public partial class ListOfValue : UserControl
    {
        private ListBox listBox;

        public ListOfValue()
        {
            listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.SelectedIndexChanged += (s, e) =>
                OnSelectedItemChanged?.Invoke(this, EventArgs.Empty);

            Controls.Add(listBox);
            InitializeComponent();
        }

        public void FillList(List<string> items)
        {
            if (items == null) return;

            listBox.Items.Clear();

            foreach (var item in items.Distinct())
            {
                if (!string.IsNullOrWhiteSpace(item))
                    listBox.Items.Add(item);
            }
        }

        public void Clear()
        {
            listBox.Items.Clear();
        }

        public int Count => listBox.Items.Count;
        public int SelectedIndex => listBox.SelectedIndex;
        public string SelectedItem => listBox.SelectedItem?.ToString();

        // События
        public event EventHandler OnSelectedItemChanged;
    }
}
