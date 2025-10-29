using Contract.Plugins;
using System;
namespace Contract.Plugins;

    public interface IReportDocumentWithContextTablesContract :
IReportDocumentContract
{
    /// <summary> 
    /// Создание документа в асинхронном режиме 
    /// </summary> 
    /// <param name="filePath">Путь до файла</param> 
    /// <param name="header">Заголовок документа</param> 
    /// <param name="tables">Список данных для таблиц</param> 
    /// <exception cref="ArgumentNullException">Не указан путь до файла</exception>
    /// <exception cref="ArgumentNullException">Не задан заголовок документа</exception> 
    /// <exception cref="ArgumentNullException">Список данных для таблиц не задан</exception>
    /// <exception cref="ArgumentOutOfRangeException">Передан пустой список данных для таблиц</exception> 

    /// <exception cref="ArgumentNullException">В списке данных для таблиц имеется запись с не заданным массивом строк</exception>
    /// <exception cref="ArgumentOutOfRangeException">В списке данных для таблиц имеется запись с пустым массивом строк</exception> 
    /// <returns>Задача по созданию документа</returns> 
     Task CreateDocumentAsync(
      string filePath,
      string header,
      List<string[,]> tables
     );
}