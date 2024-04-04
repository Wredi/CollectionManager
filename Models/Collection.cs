using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public class Collection
    {
        public string Name { get; set; }
        public List<string> AdditionalColumns { get; set; }
        public List<Item> Items { get; set; }

        public static List<string> GetDefaultAdditionalColumnsNames()
        {
            return new List<string> { "Price", "Valuation" };
        }

        public string ToText()
        {
            return Item.GetBasicColumnNames() + ";" + string.Join(";", AdditionalColumns)
                + Environment.NewLine 
                + Items.Aggregate("", (s, obj) => s + obj.ToText() + Environment.NewLine);
        }
    }
}
