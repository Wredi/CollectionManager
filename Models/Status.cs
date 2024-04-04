using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public enum Status
    {
        New,
        Used,
        ForSale,
        Sold,
        WantToBuy
    }

    public static class StatusExtensions
    {
        public static string ToText(this Status status)
        {
            switch (status)
            {
                case Status.New:
                    return "New";
                case Status.Used:
                    return "Used";
                case Status.ForSale:
                    return "For sale";
                case Status.Sold:
                    return "Sold";
                case Status.WantToBuy:
                    return "Want to buy";
                default:
                    return "idk";
            }
        }
    }
}
