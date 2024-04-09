using CollectionManager.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
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
        public List<string> columns;

        [ObservableProperty]
        public ObservableCollection<Models.Item> items;

        public ViewCollectionViewModel()
        {
            columns = new List<string>();
            items = new ObservableCollection<Models.Item>();
            collectionName = string.Empty;
        }

        [RelayCommand]
        public async Task AddItem()
        {
            await Shell.Current.Navigation.PushAsync(new ItemFormPage
            {
                BindingContext = new ItemFormViewModel(selectedCollection, Columns.GetRange(3, Columns.Count - 3))
            });
            LoadCollection();
        }

        [RelayCommand]
        public void RemoveItem(Models.Item item)
        {
            App.CollectionRepo.RemoveItemImage(CollectionName, item);
            Items.Remove(item);
            App.CollectionRepo.SaveCollection(new Models.Collection
            {
                Name = CollectionName,
                Columns = Columns,
                Items = Items.ToList(),
            });
        }

        [RelayCommand]
        public async Task EditItem(Models.Item item)
        {
            await Shell.Current.Navigation.PushAsync(new ItemFormPage
            {
                BindingContext = new ItemFormViewModel(selectedCollection, Columns.GetRange(3, Columns.Count - 3), item.Values)
            });
            LoadCollection();
        }

        public void OnAppearing()
        {
            LoadCollection();
        }

        public void LoadCollection()
        {
            Models.Collection c = App.CollectionRepo.LoadCollection(selectedCollection);
            CollectionName = c.Name;
            Columns = c.Columns;
            Items = new ObservableCollection<Models.Item>(c.Items);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("selected"))
            {
                selectedCollection = query["selected"].ToString();
            }
        }
    }
}
