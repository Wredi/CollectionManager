using System;
using System.Collections;
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

        public void RemoveAdditionalColumn(int idx)
        {
            Values.RemoveAt(idx + 3);
        }

        public void AddValue(string value)
        {
            Values.Add(value);
        }

        public string ImagePath(string collectionName)
        {
            return App.CollectionRepo.GetImagePath(collectionName, Values[0]);
        }

        public string GetName()
        {
            return Values[1];
        }

        public string GetStatus()
        {
            return Values[2];
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
