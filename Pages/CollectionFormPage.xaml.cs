using CollectionManager.ViewModels;

namespace CollectionManager.Pages;

public partial class CollectionFormPage : ContentPage
{
	public CollectionFormPage()
	{
		InitializeComponent();
	}

    private void OnItemSelectedChanged(object sender, SelectedItemChangedEventArgs e)
    {
        CollectionFormViewModel vm = (CollectionFormViewModel)BindingContext;
        vm.SelectColumnCommand.Execute(e.SelectedItem);
    }
}