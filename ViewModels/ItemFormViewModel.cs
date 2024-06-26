﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionManager.ViewModels
{
    public partial class ItemFormViewModel : ObservableObject
    {
        public string collectionName;

        public List<string> StatusOptions => Models.Item.GetStatusOptions();

        [ObservableProperty]
        public List<string> additionalColumns;

        public List<string> initialValues;

        [RelayCommand]
        public async Task SaveItem(Models.Item item)
        {
            if (App.CollectionRepo.CheckDuplicates(
                collectionName, item.GetName(), initialValues != null ? initialValues[1] : null
                ))
            {
                await Shell.Current.DisplayAlert("Error", "The item name is already taken", "OK");
                return;
            }

            if(initialValues == null)
            {
                item.Values[0] = App.CollectionRepo.SaveImageFromFile(collectionName, item.Values[0]);
                App.CollectionRepo.AddItem(collectionName, item);
            }
            else
            {
                if (App.CollectionRepo.GetImagePath(collectionName, initialValues[0]) != item.Values[0])
                {
                    //App.CollectionRepo.RemoveItemImage(collectionName, item);
                    item.Values[0] = App.CollectionRepo.SaveImageFromFile(collectionName, item.Values[0]);
                }
                else
                {
                    item.Values[0] = initialValues[0];
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
