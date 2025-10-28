using System;
using Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentOrdersReport;

namespace ComponentDate;

public class OrdersReportComponent : IComponentContract
{
    public string Id => "OrdersReceiptDateReport";
    public string Title => "Отчет по дате получения заказа";
    public string Category => "Reports";

    public UserControl GetControl()
    {
        return new OrdersReportControl();
    }
}
