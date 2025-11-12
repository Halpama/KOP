using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Plugins;

public interface IReportDocumentWithChartLineContract : IReportDocumentContract
{
    /// <summary> 
    /// Создание документа в асинхронном режиме 
    /// </summary> 
    /// <param name="filePath">Путь до файла</param> 
    /// <param name="header">Заголовок документа</param> 
    /// <param name="chartTitle">Заголовок диаграммы</param> 
    /// <param name="series">Список серий с данными для линейной
    Task CreateDocumentAsync(
     string filePath,
     string header,
     string chartTitle,
     Dictionary<string, List<(int Parameter, double Value)>> series
    );
}