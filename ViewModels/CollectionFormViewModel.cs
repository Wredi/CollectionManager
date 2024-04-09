using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CollectionManager.ViewModels
{
    partial class CollectionFormViewModel
    {
        public string NameToEdit { get; set; }

        public ObservableCollection<string> Columns { get; set; }

        public List<Models.Item> Items { get; set; }

        public string ColumnToEdit { get; set; }

        public CollectionFormViewModel()
        {
            NameToEdit = string.Empty;
            Columns = new ObservableCollection<string>(Models.Collection.GetDefaultAdditionalColumnsNames());

        }

        public CollectionFormViewModel(Models.Collection collection)
        {
            NameToEdit = collection.Name;
            Columns = new ObservableCollection<string>(collection.GetAdditionalColumns());
            Items = collection.Items;
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
                if (!string.IsNullOrEmpty(NameToEdit))
                {
                    foreach(var item in Items)
                    {
                        item.AddValue("");
                    }
                }
            }
        }

        [RelayCommand]
        private async Task EditColumn()
        {
            if (ColumnToEdit == null) return;
            string result = await Shell.Current.DisplayPromptAsync("Edit column", "Edit name:", initialValue: ColumnToEdit);
            if (!string.IsNullOrEmpty(result))
            {
                var idx = Columns.IndexOf(ColumnToEdit);
                Columns.RemoveAt(idx);
                Columns.Insert(idx, result);
            }
        }

        [RelayCommand]
        private void DeleteColumn()
        {
            var idx = Columns.IndexOf(ColumnToEdit);
            Columns.RemoveAt(idx);
            if (!string.IsNullOrEmpty(NameToEdit))
            {
                foreach (var item in Items)
                {
                    item.RemoveAdditionalColumn(idx);
                }
            }
        }

        [RelayCommand]
        private async Task ApproveForm(string newName)
        {
            if (string.IsNullOrEmpty(NameToEdit))
            {
                App.CollectionRepo.SaveCollection(
                        new Models.Collection
                        {
                            Name = newName,
                            Columns = Models.Item.GetBasicColumnNames().Concat(Columns).ToList(),
                            Items = new List<Models.Item>()
                        }
                    );
            }
            else
            {
                if(NameToEdit != newName){
                    App.CollectionRepo.RenameCollection(NameToEdit, newName);
                }
                App.CollectionRepo.SaveCollection(
                        new Models.Collection
                        {
                            Name = newName,
                            Columns = Models.Item.GetBasicColumnNames().Concat(Columns).ToList(),
                            Items = Items.ToList()
                        }
                    );
            }
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
