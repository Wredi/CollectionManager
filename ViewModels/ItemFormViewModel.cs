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

        public List<string> initialValues;

        [RelayCommand]
        public async Task SaveItem(Models.Item item)
        {
            if(initialValues == null)
            {
                item.Values[0] = App.CollectionRepo.SaveImageFromFile(collectionName, item.Values[0]);
                App.CollectionRepo.AddItem(collectionName, item);
            }
            else
            {
                if (initialValues[0] != item.Values[0])
                {
                    App.CollectionRepo.RemoveItemImage(collectionName, item);
                    item.Values[0] = App.CollectionRepo.SaveImageFromFile(collectionName, item.Values[0]);
                }
                App.CollectionRepo.EditItem(collectionName, initialValues[1], item);
            }

            await Shell.Current.Navigation.PopModalAsync();
        }

        public ItemFormViewModel(string collectionName, List<string> additionalColumns, List<string> initialValues=null) 
        {
            this.collectionName = collectionName;
            this.additionalColumns = additionalColumns;
            this.initialValues = initialValues;
        }
    }
}
