using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contract.Plugins
{
    /// <summary>
    /// Контракт для создания документа с гистограммой (столбчатой диаграммой).
    /// </summary>
    public interface IReportDocumentWithChartHistogramContract : IReportDocumentContract
    {
        /// <summary>
        /// Асинхронное создание документа с гистограммой.
        /// </summary>
        /// <param name="filePath">Путь до файла</param>
        /// <param name="header">Заголовок документа</param>
        /// <param name="chartTitle">Заголовок диаграммы</param>
        /// <param name="series">Список данных для гистограммы: (Parameter — идентификатор/название, Value — значение)</param>
        /// <exception cref="ArgumentNullException">Не указан путь до файла</exception>
        /// <exception cref="ArgumentNullException">Не задан заголовок документа</exception>
        /// <exception cref="ArgumentNullException">Не задан заголовок диаграммы</exception>
        /// <exception cref="ArgumentNullException">Список серий не задан</exception>
        /// <exception cref="ArgumentOutOfRangeException">Список серий пустой</exception>
        /// <returns>Задача по созданию документа</returns>
        Task CreateDocumentAsync(
            string filePath,
            string header,
            string chartTitle,
            List<(int Parameter, double Value)> series
        );
    }
}