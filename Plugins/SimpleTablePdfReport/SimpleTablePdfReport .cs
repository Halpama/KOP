using Contract;
using Contract.Plugins;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfSharpCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PluginSimpleTablePdf
{
    public class SimpleTablePdfReport : IReportDocumentWithContextTablesContract
    {
        public string Id => "SimpleTablePdfReport21";
        public string Title => "Отчет по движению заказов";
        public string Category => "Reports";

        // Формат документа
        public string DocumentFormat => "PDF";

        /// <summary>
        /// Асинхронное создание PDF-документа с таблицами по движению заказов.
        /// </summary>
        public async Task CreateDocumentAsync(string filePath, string header, List<string[,]> tables)
        {
            if (tables == null || tables.Count == 0)
                throw new ArgumentException("Нет данных для отчета.");

            await Task.Run(() =>
            {
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 40))
                {
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Заголовок
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var textFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);

                    doc.Add(new Paragraph(header ?? "Отчет по движению заказов", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    });

                    // Перебор всех таблиц из контекста
                    foreach (var tableData in tables)
                    {
                        int rows = tableData.GetLength(0);
                        int cols = tableData.GetLength(1);
                        PdfPTable pdfTable = new PdfPTable(cols)
                        {
                            WidthPercentage = 100
                        };

                        // Заполняем таблицу
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                string cellValue = tableData[i, j] ?? "";
                                var cellFont = i == 0
                                    ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE)
                                    : textFont;

                                var cell = new PdfPCell(new Phrase(cellValue, cellFont))
                                {
                                    Padding = 5,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                };

                                // Шапка — делаем фон
                                if (i == 0)
                                    cell.BackgroundColor = new BaseColor(60, 90, 150);

                                pdfTable.AddCell(cell);
                            }
                        }

                        doc.Add(pdfTable);
                        doc.Add(new Paragraph("\n", textFont)); // промежуток между таблицами
                    }

                    doc.Close();
                }
            });
        }
    }
}
