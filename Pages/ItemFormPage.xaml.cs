using CollectionManager.ViewModels;

namespace CollectionManager.Pages;

public partial class ItemFormPage : ContentPage
{
	public ItemFormPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
        ItemFormViewModel viewModel = (ItemFormViewModel)BindingContext;
        inputs.Add(GenerateInputs(viewModel.AdditionalColumns));
    }

	public StackLayout GenerateSingleInput(string labelName)
	{
		Entry entry = new Entry();
        return GenerateSingleInput(labelName, entry);
    }

	public StackLayout GenerateSingleInput(string labelName, View inputElement)
	{
        StackLayout stackLayout = new StackLayout();
        stackLayout.Orientation = StackOrientation.Vertical;

        Label label = new Label();
        label.Text = labelName;

        stackLayout.Add(label);
        stackLayout.Add(inputElement);
        return stackLayout;
    }

	public StackLayout GenerateInputs(List<string> additionalColumns)
	{
        StackLayout stackLayout = new StackLayout();
		stackLayout.Orientation= StackOrientation.Vertical;
		stackLayout.VerticalOptions = LayoutOptions.Start;

		foreach(var c in additionalColumns)
		{
			StackLayout inputLayout = GenerateSingleInput(c);
			stackLayout.Add(inputLayout);	
		}
		return stackLayout;
	}

    private void SaveItem_Clicked(object sender, EventArgs e)
    {
        ItemFormViewModel viewModel = (ItemFormViewModel)BindingContext;
		string imageSource = this.imageSource.Text;
		string name = this.name.Text;
		string status = (string)this.status.SelectedItem;
		List<string> allColumns = new List<string>{ imageSource, name, status };

		foreach(var item in (StackLayout)inputs[inputs.Count - 1])
		{
			StackLayout stackLayout = (StackLayout)item;
			allColumns.Add(((Entry)stackLayout[stackLayout.Count - 1]).Text);
		}

		viewModel.SaveItemCommand.Execute(new Models.Item
		{
			Values = allColumns
		});
    }
}