using CollectionManager.Pages;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

        public event EventHandler ForceReRender;

        public ViewCollectionViewModel()
        {
            columns = new List<string>();
            items = new ObservableCollection<Models.Item>();
            collectionName = string.Empty;
        }

        [RelayCommand]
        public async Task ExportCollection()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            using var stream = new MemoryStream();
            App.CollectionRepo.ZipCollection(stream, CollectionName);
            var fileSaverResult = await FileSaver.Default.SaveAsync($"{CollectionName}.zip", stream, cancellationTokenSource.Token);
        }

        [RelayCommand]
        public async Task ImportCollection()
        {
            var zipFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] { ".zip" } },
                { DevicePlatform.Android, new[] { ".zip" } },
                { DevicePlatform.WinUI, new[] { ".zip" } },
                { DevicePlatform.Tizen, new[] { ".zip" } },
                { DevicePlatform.macOS, new[] { ".zip" } },
                });
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose ZIP file to import",
                FileTypes = zipFileType
            });

            if (result == null)
                return;

            string res = App.CollectionRepo.UnzipAndMergeCollection(result.FullPath, CollectionName);
            if (!string.IsNullOrEmpty(res))
            {
                await Shell.Current.DisplayAlert("Error", res, "OK");
                return;
            }

            LoadCollection();
            ForceReRender.Invoke(this, EventArgs.Empty);
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
            Items.Remove(item);
            App.CollectionRepo.SaveCollection(new Models.Collection
            {
                Name = CollectionName,
                Columns = Columns,
                Items = Items.ToList(),
            });
            ForceReRender.Invoke(this, EventArgs.Empty);
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

            List<Models.Item> sold = new List<Models.Item>();
            List<Models.Item> other = new List<Models.Item>();
            foreach (Models.Item item in c.Items)
            {
                if(item.GetStatus() == "Sold")
                {
                    sold.Add(item);
                }
                else
                {
                    other.Add(item);
                }
            }
            Items = new ObservableCollection<Models.Item>(other.Concat(sold));
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
