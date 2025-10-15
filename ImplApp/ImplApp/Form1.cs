using KopLab21;
namespace ImplApp
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            listOfValue.FillItems(new List<string> { "��������1", "��������2", "��������3" });
            inputInt1.ValueChanged += (s, e) =>
            {
                try
                {
                    int? val = inputInt1.Value;
                    MessageBox.Show(val.HasValue ? val.Value.ToString() : "null");
                }
                catch (InvalidInputException ex)
                {
                    MessageBox.Show("������: " + ex.Message);
                }
            };

        }
    }
}

