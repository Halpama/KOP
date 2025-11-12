using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Contract;
using Contract.Plugins;
using D = DocumentFormat.OpenXml.Drawing;
using DCharts = DocumentFormat.OpenXml.Drawing.Charts;
using WordDoc = DocumentFormat.OpenXml.Wordprocessing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;

namespace LineChartReportPlugins
{
    public class LineChartReportPlugins : IReportDocumentWithChartLineContract
    {
        public string DocumentFormat => "docx";

        public async Task CreateDocumentAsync(
            string filePath,
            string header,
            string chartTitle,
            Dictionary<string, List<(int Parameter, double Value)>> series)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (string.IsNullOrWhiteSpace(header)) throw new ArgumentNullException(nameof(header));
            if (string.IsNullOrWhiteSpace(chartTitle)) throw new ArgumentNullException(nameof(chartTitle));
            if (series == null) throw new ArgumentNullException(nameof(series));
            if (series.Count == 0) throw new ArgumentOutOfRangeException(nameof(series));

            foreach (var s in series)
            {
                if (s.Key == null) throw new ArgumentNullException("Название серии не может быть null");
                if (s.Value == null) throw new ArgumentNullException($"Список точек серии '{s.Key}' не задан");
                if (s.Value.Count == 0) throw new ArgumentOutOfRangeException($"Список точек серии '{s.Key}' пуст");
            }

            var lengths = series.Values.Select(v => v.Count).Distinct().ToList();
            if (lengths.Count > 1) throw new ArgumentException("В разных сериях различается количество точек");

            await Task.Run(() =>
            {
            using var doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            var mainPart = doc.AddMainDocumentPart();
            mainPart.Document = new WordDoc.Document();
            var body = mainPart.Document.AppendChild(new WordDoc.Body());

            // Заголовок
            body.AppendChild(new WordDoc.Paragraph(new WordDoc.Run(new WordDoc.Text(header))));

            // ChartPart
            var chartPart = mainPart.AddNewPart<ChartPart>();
            chartPart.ChartSpace = new DCharts.ChartSpace();
            var chart = chartPart.ChartSpace.AppendChild(new DCharts.Chart());
            var plotArea = chart.AppendChild(new DCharts.PlotArea());
            var lineChart = plotArea.AppendChild(new DCharts.LineChart());

            uint seriesIndex = 0;
            foreach (var s in series)
            {
                var lineSer = new DCharts.LineChartSeries(
                    new DCharts.Index() { Val = seriesIndex },
                    new DCharts.Order() { Val = seriesIndex },
                    new DCharts.SeriesText(new DCharts.NumericValue() { Text = s.Key })
                );

                var cat = new DCharts.CategoryAxisData();
                var val = new DCharts.Values();

                foreach (var point in s.Value)
                {
                    cat.Append(new DCharts.StringPoint() { Index = (uint)point.Parameter, NumericValue = new DCharts.NumericValue(point.Parameter.ToString()) });
                    val.Append(new DCharts.NumericPoint() { Index = (uint)point.Parameter, NumericValue = new DCharts.NumericValue(point.Value.ToString()) });
                }

                lineSer.Append(cat);
                lineSer.Append(val);
                lineChart.Append(lineSer);
                seriesIndex++;
            }

            chart.Append(new DCharts.Title(new DCharts.ChartText(new DCharts.RichText(new WordDoc.Paragraph(new WordDoc.Run(new WordDoc.Text(chartTitle)))))));
                // Вставка диаграммы в Word через Drawing элемент Wordprocessing
                var drawing = new WordDoc.Drawing(
                    new DW.Inline(
                        new D.Graphic(
                            new D.GraphicData(chartPart.ChartSpace.CloneNode(true))
                            { Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart" }
                        )
                    )
                );
                body.Append(new WordDoc.Paragraph(new WordDoc.Run(drawing)));
            });
        }
    }
}