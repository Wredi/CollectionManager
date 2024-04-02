using CollectionManager.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CollectionManager.ViewModels
{
    partial class CollectionFormViewModel
    {
        public string NameToEdit { get; set; }
        public ObservableCollection<string> Columns { get; set; }
        public string ColumnToEdit { get; set; }

        public CollectionFormViewModel()
        {
            NameToEdit = string.Empty;
            Columns = new ObservableCollection<string>(App.CollectionRepo.GetDefaultColumnNames());
        }

        public CollectionFormViewModel(Models.Collection collection)
        {
            NameToEdit = collection.Name;
            Columns = new ObservableCollection<string>(collection.Columns);
        }

        [RelayCommand]
        private void SelectColumn(string selectedColumn)
        {
            ColumnToEdit = selectedColumn;
        }

        [RelayCommand]
        private async Task AddColumn()
        {
            string result = await Shell.Current.DisplayPromptAsync("Add column", "Enter new column name:");
            if (!string.IsNullOrEmpty(result))
            {
                Columns.Add(result);
            }
        }

        [RelayCommand]
        private async Task EditColumn()
        {
            if (ColumnToEdit == null) return;
            string result = await Shell.Current.DisplayPromptAsync("Edit column", "Edit name:", initialValue: ColumnToEdit);
            if (!string.IsNullOrEmpty(result))
            {
            }
        }

        [RelayCommand]
        private void DeleteColumn()
        {
            Columns.Remove(ColumnToEdit);
        }

        [RelayCommand]
        private async Task ApproveForm(string newName)
        {
            if (string.IsNullOrEmpty(NameToEdit))
            {
                App.CollectionRepo.AddCollection(newName);
            }
            else
            {
                //App.CollectionRepo.EditCollectionName(NameToEdit, newName);
            }
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
