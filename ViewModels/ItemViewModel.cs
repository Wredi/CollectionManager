using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.ViewModels
{
    public class ItemViewModel
    {
        private Models.Item model;
        public ItemViewModel(Models.Item model)
        {
            this.model = model;
        }
    }
}
