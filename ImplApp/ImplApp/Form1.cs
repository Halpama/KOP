using KopLab21;

namespace ImplApp
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            listOfValue.FillList(new List<string> { "Значение1", "Значение2", "Значение3" });

        }
    }
}

