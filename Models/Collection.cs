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
        public List<string> Columns { get; set; }
        public List<Item> Items { get; set; }

        public static List<string> GetDefaultAdditionalColumnsNames()
        {
            return new List<string> { "Price", "Valuation" };
        }

        public List<string> GetAdditionalColumns()
        {
            return Columns.GetRange(3, Columns.Count - 3);
        }

        public string ToText()
        {
            return string.Join(";", Columns)
                + Environment.NewLine 
                + Items.Aggregate("", (s, obj) => s + obj.ToText() + Environment.NewLine);
        }

        public static Collection FromText(string name, string text)
        {
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return new Collection
            {
                Name = name,
                Columns = lines[0].Split(new char[] { ';' }).ToList(),
                Items = new ArraySegment<string>(lines, 1, lines.Length - 1).Select(line => Item.FromText(line)).ToList()
            };
        }
    }
}
