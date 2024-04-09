using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public class Item
    {
        public List<string> Values { get; set; }

        public string ToText()
        {
            return string.Join(";", Values);
        }

        public static Item FromText(string text)
        {
            string[] vars = text.Split(new char[] { ';' });
            return new Item
            {
                Values = vars.ToList()
            };
        }

        public static List<string> GetBasicColumnNames()
        {
            return new List<string> { "Image", "Name", "Status" };
        }

        public static List<string> GetStatusOptions()
        {
            return new List<string> { "New", "Used", "For sale", "Sold", "Want to buy" };
        }

        //public static string GetBasicColumnNames()
        //{
        //    return "Name;Image;Status";
        //}
    }
}
