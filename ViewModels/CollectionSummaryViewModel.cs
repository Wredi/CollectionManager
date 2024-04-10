using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.ViewModels
{
    class CollectionSummaryViewModel
    {
        public int SoldItems {  get; set; }
        public int ForSaleItems {  get; set; }
        public int OwnedItems {  get; set; }

        public CollectionSummaryViewModel(string collectionName)
        {
            InitItemCounts(collectionName);
        }

        private void InitItemCounts(string collectionName)
        {
            Models.Collection coll = App.CollectionRepo.LoadCollection(collectionName);
            foreach (var item in coll.Items)
            {
                if (item.GetStatus() == "Sold")
                {
                    SoldItems++;
                }else if(item.GetStatus() == "For sale")
                {
                    ForSaleItems++;
                    OwnedItems++;
                }
                else
                {
                    OwnedItems++;
                }
            }
        }
    }
}
