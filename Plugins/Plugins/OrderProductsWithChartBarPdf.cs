using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Contract.Plugins;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace PluginOrderProductsWithChartBarPdf;

public class OrderProductsWithChartBarPdf : IReportDocumentWithChartHistogramContract
{
    public string DocumentFormat => "pdf";

    public async Task CreateDocumentAsync(string filePath, string header, string chartTitle, List<(int Parameter, double Value)> _)
    {
        await Task.Run(() =>
        {
            string ordersPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.txt");
            if (!File.Exists(ordersPath))
                throw new FileNotFoundException($"Файл заказов не найден: {ordersPath}");

            // читаем все заказы и считаем количество каждого товара
            var productCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadAllLines(ordersPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    string productName = parts[3].Trim();
                    if (!string.IsNullOrEmpty(productName))
                    {
                        if (productCounts.ContainsKey(productName))
                            productCounts[productName]++;
                        else
                            productCounts[productName] = 1;
                    }
                }
            }

            if (productCounts.Count == 0)
                throw new Exception("Не удалось получить данные о товарах для отчёта.");

            // создаем PDF-документ
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontText = new XFont("Arial", 12, XFontStyle.Regular);

                // Заголовок
                gfx.DrawString(header, fontTitle, XBrushes.Black,
                    new XRect(0, 20, page.Width, 40), XStringFormats.TopCenter);

                gfx.DrawString(chartTitle, fontText, XBrushes.Black,
                    new XRect(0, 60, page.Width, 40), XStringFormats.TopCenter);

                // Параметры гистограммы
                int leftMargin = 100;
                int bottomMargin = 100;
                int barWidth = 40;
                int maxBarHeight = 300;

                int maxCount = productCounts.Values.Max();

                int x = leftMargin;
                int yBase = (int)page.Height - bottomMargin;

                // Рисуем оси
                gfx.DrawLine(XPens.Black, leftMargin - 20, yBase, page.Width - 50, yBase);
                gfx.DrawLine(XPens.Black, leftMargin - 20, yBase, leftMargin - 20, yBase - maxBarHeight - 50);

                // Рисуем столбцы
                foreach (var kvp in productCounts)
                {
                    double normalizedHeight = (kvp.Value / (double)maxCount) * maxBarHeight;
                    gfx.DrawRectangle(XBrushes.SkyBlue, x, yBase - normalizedHeight, barWidth, normalizedHeight);

                    // Подписи под столбцами
                    gfx.DrawString(kvp.Key, fontText, XBrushes.Black, new XRect(x - 10, yBase + 5, barWidth + 20, 30), XStringFormats.TopCenter);

                    // Числовое значение над столбцом
                    gfx.DrawString(kvp.Value.ToString(), fontText, XBrushes.Black, new XRect(x, yBase - normalizedHeight - 20, barWidth, 20), XStringFormats.TopCenter);

                    x += barWidth + 30;
                }

                // сохраняем PDF
                document.Save(filePath);
            }
        });
    }
}