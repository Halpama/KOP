using Contract;
using System.Windows.Forms;

namespace ComponentCities
{
    public class CitiesComponent : IComponentContract
    {
        public string Id => "Cities";
        public string Title => "Справочник городов";
        public string Category => "Directory";

        public UserControl GetControl()
        {
            return new CitiesControl(); 
        }
    }
}
