using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
            Columns = new ObservableCollection<string>(Models.Collection.GetDefaultAdditionalColumnsNames());
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
                //TODO: Edit column
                await Shell.Current.DisplayAlert("TODO", "Edit column not implemented", "OK");
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
                //TODO: Edit collection
                await Shell.Current.DisplayAlert("TODO", "Edit not implemented", "OK");
            }
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
