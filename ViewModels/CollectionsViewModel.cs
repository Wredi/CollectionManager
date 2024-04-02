using CollectionManager.Pages;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CollectionManager.ViewModels
{
    public partial class CollectionsViewModel
    {
        public ObservableCollection<string> Collections { get; set; }

        private string currSelectedCollection;

        List<string> gradeNames = new List<string> { "sdfdfs", "dsfdfs", "fgasas", "dfsaaa", "dfsdsfaasdaads" };
        List<string> GetCollectionsNames()
        {
            return gradeNames;
        }

        void DeleteCollectionName(string name)
        {
            gradeNames.Remove(name);
        }

        void AddCollection(string grade)
        {
            gradeNames.Add(grade);
        }

        public void LoadCollections()
        {
            Collections.Clear();
            foreach(var grade in GetCollectionsNames())
            {
                Collections.Add(grade);
            }
        }

        [RelayCommand]
        private void SelectCollection(string selectedGrade)
        {
            currSelectedCollection = selectedGrade;
        }

        [RelayCommand]
        private void DeleteSelectedGrade()
        {
            DeleteCollectionName(currSelectedCollection);
            Collections.Remove(currSelectedCollection);
        }

        [RelayCommand]
        private async Task AddCollection()
        {
            //string result = await Shell.Current.DisplayPromptAsync("Add grade", "Enter new grade name:");
            //if(!string.IsNullOrEmpty(result))
            //{
            //    AddCollection(result);
            //    LoadCollections();
            //}

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
            LoadCollections();
        }

        public CollectionsViewModel()
        {
            Collections = new ObservableCollection<string>();
        }
    }
}
