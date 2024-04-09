using CollectionManager.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CollectionManager.ViewModels
{
    public partial class CollectionsViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<string> collections;

        private string currSelectedCollection;

        public void LoadCollections()
        {
            Collections = new ObservableCollection<string>(App.CollectionRepo.GetCollectionNames());
        }

        [RelayCommand]
        private void SelectCollection(string selectedCollection)
        {
            currSelectedCollection = selectedCollection;
        }

        [RelayCommand]
        private void DeleteSelectedCollection()
        {
            App.CollectionRepo.DeleteCollection(currSelectedCollection);
            Collections.Remove(currSelectedCollection);
        }

        [RelayCommand]
        private async Task ApproveSelectedCollection()
        {
            if (currSelectedCollection == null) return;
            await Shell.Current.GoToAsync($"{nameof(Pages.ViewCollectionPage)}?selected={currSelectedCollection}");
        }

        [RelayCommand]
        private async Task AddCollection()
        {
            CollectionFormViewModel vm = new CollectionFormViewModel();
            await Shell.Current.Navigation.PushAsync(new CollectionFormPage
            {
                BindingContext = vm
            });
            LoadCollections();
        }

        [RelayCommand]
        private async Task EditSelectedCollection()
        {
            if (currSelectedCollection == null) return;

            CollectionFormViewModel vm = new CollectionFormViewModel(App.CollectionRepo.LoadCollection(currSelectedCollection));
            await Shell.Current.Navigation.PushAsync(new CollectionFormPage
            {
                BindingContext = vm
            });
            LoadCollections();
        }

        public CollectionsViewModel()
        {
            Collections = new ObservableCollection<string>();
        }
    }
}
