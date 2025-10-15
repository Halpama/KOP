using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Components;

public partial class ObjectListControl : UserControl
{
    private string _template = "!{Email}!";
    private string _startSymbol = "{";
    private string _endSymbol = "}";

    public ObjectListControl()
    {
        InitializeComponent();
        if (!DesignMode)
            listBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
    }

    // Установка шаблона и символов начала/конца свойства
    public void SetTemplate(string template, string startSymbol = "{", string endSymbol = "}")
    {
        if (string.IsNullOrEmpty(template))
            throw new ArgumentException("Шаблон не может быть пустым");

        _template = template;
        _startSymbol = startSymbol;
        _endSymbol = endSymbol;

        ValidateTemplate();
    }

    public event EventHandler? SelectedValueChanged;

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string SelectedValue
    {
        get => listBox.SelectedItem?.ToString() ?? "";
        set
        {
            if (listBox.Items.Contains(value))
                listBox.SelectedItem = value;
            else
                listBox.SelectedItem = null;
        }
    }

    private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        => SelectedValueChanged?.Invoke(this, EventArgs.Empty);

    public void Clear() => listBox.Items.Clear();

    // --- 1. Заполнение списка объектов ---
    public void Fill<T>(List<T> items)
    {
        Clear();
        foreach (var item in items)
            AddItem(item);
    }

    private void AddItem<T>(T item)
    {
        string line = _template;
        foreach (var prop in typeof(T).GetProperties())
        {
            string placeholder = _startSymbol + prop.Name + _endSymbol;
            if (line.Contains(placeholder))
            {
                var value = prop.GetValue(item)?.ToString() ?? "";
                line = line.Replace(placeholder, value);
            }
        }

        if (!string.IsNullOrWhiteSpace(line) && !listBox.Items.Contains(line))
            listBox.Items.Add(line);
    }

    // Получение объекта из выбранной строки
    public T GetSelectedItem<T>() where T : new()
    {
        if (listBox.SelectedIndex == -1) return default!;

        string selected = listBox.SelectedItem.ToString()!;
        var obj = new T();

        var matches = Regex.Matches(_template, @"\{(\w+)\}");
        string pattern = Regex.Escape(_template);
        foreach (Match m in matches)
        {
            string propName = m.Groups[1].Value;
            pattern = pattern.Replace("\\{" + propName + "\\}", "(.*)");
        }

        var matchResult = Regex.Match(selected, pattern);
        if (!matchResult.Success) throw new Exception("Совпадение между свойствами объекта и строкой не было найден!");

        for (int i = 0; i < matches.Count; i++)
        {
            string propName = matches[i].Groups[1].Value;
            var prop = typeof(T).GetProperty(propName);
            if (prop != null && matchResult.Groups.Count > i + 1)
            {
                prop.SetValue(obj, Convert.ChangeType(matchResult.Groups[i + 1].Value, prop.PropertyType));
            }
        }

        return obj;
    }

    // Проверка шаблона
    private void ValidateTemplate()
    {
        var matches = Regex.Matches(_template, Regex.Escape(_startSymbol) + @"(\w+)" + Regex.Escape(_endSymbol));
        if (matches.Count == 0)
            throw new Exception("Шаблон должен содержать хотя бы одно свойство");

        if (matches[0].Index == 0 || matches[^1].Index + matches[^1].Length == _template.Length)
            throw new Exception("Шаблон не должен начинаться или заканчиваться свойством");

        for (int i = 1; i < matches.Count; i++)
        {
            if (matches[i].Index == matches[i - 1].Index + matches[i - 1].Length)
                throw new Exception("В шаблоне не может быть двух свойств подряд без текста между ними");
        }
    }

    [Browsable(false)]
    public ListBox.ObjectCollection Items => listBox.Items;
}
