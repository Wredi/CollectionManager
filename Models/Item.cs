using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public class Item
    {
        public string Image {  get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }

        public List<string> AdditionalValues { get; set; }
        public string ToText()
        {
            return Name + ";" + Image + ";" + Status.ToText() + ";" + string.Join(";", AdditionalValues);
        }

        public static string GetBasicColumnNames()
        {
            return "Name;Image;Status";
        }
    }
}
