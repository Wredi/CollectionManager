using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionManager.ViewModels
{
    public partial class ItemFormViewModel : ObservableObject
    {
        private string collectionName;

        public List<string> StatusOptions => Models.Item.GetStatusOptions();

        [ObservableProperty]
        public List<string> additionalColumns;

        [RelayCommand]
        public async Task SaveItem(Models.Item item)
        {
            //Models.Item item = new Models.Item
            //{
            //    Values = new List<string> { Image, Name, Status }.Concat()
            //};
            item.Values[0] = App.CollectionRepo.SaveImageFromFile(collectionName, item.Values[0]);
            App.CollectionRepo.AddItem(collectionName, item);
            await Shell.Current.Navigation.PopModalAsync();
        }

        public ItemFormViewModel(string collectionName, List<string> additionalColumns) 
        {
            this.collectionName = collectionName;
            this.additionalColumns = additionalColumns;
        }
    }
}
