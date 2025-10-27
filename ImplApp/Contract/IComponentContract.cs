using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Contract
{
    public interface IComponentContract
    {
        string Id { get; }
        string Title { get; }
        string Category { get; }
        UserControl GetControl();

    }
}
