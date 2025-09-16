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

    public partial class InputInt : UserControl
    {
        public InputInt()
        {
            InitializeComponent();

            // Событие для CheckBox: блокировка TextBox
            checkBox1.CheckedChanged += (s, e) =>
            {
                textBox1.Enabled = !checkBox1.Checked;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            };

            // Событие для TextBox: изменение текста
            textBox1.TextChanged += (s, e) =>
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        // Свойство для установки/получения значения
        private int? _value;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? Value
        {
            get
            {
                if (checkBox1.Checked)
                    return null;

                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    throw new InvalidInputException("Значение не введено.");

                if (!int.TryParse(textBox1.Text, out int result))
                    throw new InvalidInputException("Введённое значение не является целым числом.");

                return result;
            }
            set
            {
                if (value == null)
                {
                    checkBox1.Checked = true;
                    textBox1.Text = string.Empty;
                }
                else
                {
                    checkBox1.Checked = false;
                    textBox1.Text = value.ToString();
                }
            }
        }

        // Событие при изменении значения или состояния CheckBox
        public event EventHandler ValueChanged;
    }
}
