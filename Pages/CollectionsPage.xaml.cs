using CollectionManager.ViewModels;

namespace CollectionManager.Pages;

public partial class CollectionsPage : ContentPage
{
	public CollectionsPage()
	{
		InitializeComponent();
	}

    private void OnItemSelectedChanged(object sender, SelectedItemChangedEventArgs e)
    {
		CollectionsViewModel vm = (CollectionsViewModel)BindingContext;
		vm.SelectCollectionCommand.Execute(e.SelectedItem);
    }

    protected override void OnAppearing()
    {
		CollectionsViewModel vm = (CollectionsViewModel)BindingContext;
        vm.LoadCollections();
        base.OnAppearing();
    }
}