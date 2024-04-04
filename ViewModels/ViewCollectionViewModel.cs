using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.ViewModels
{
    public partial class ViewCollectionViewModel : ObservableObject, IQueryAttributable
    {
        private string selectedCollection;

        [ObservableProperty]
        public string collectionName;

        [ObservableProperty]
        public List<string> additionalColumns;

        [ObservableProperty]
        public ObservableCollection<ItemViewModel> items;

        public void LoadCollection()
        {
            Models.Collection c = App.CollectionRepo.LoadCollection(selectedCollection);
            CollectionName = c.Name;
            AdditionalColumns = c.AdditionalColumns;
            Items = new ObservableCollection<ItemViewModel>(c.Items.Select(i => new ItemViewModel(i)));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("selected"))
            {
                selectedCollection = query["selected"].ToString();
                LoadCollection();
            }
        }
    }
}
