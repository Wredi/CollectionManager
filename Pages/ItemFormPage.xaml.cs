using CollectionManager.ViewModels;
using System.Collections;

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
		List<string> initialValues = viewModel.initialValues;
		if(initialValues != null)
		{
            imageSource.Text = App.CollectionRepo.GetImagePath(viewModel.collectionName, initialValues[0]);
            name.Text = initialValues[1];
            status.SelectedItem = initialValues[2];
			StackLayout layout = GenerateInputs(viewModel.AdditionalColumns, initialValues.GetRange(3, initialValues.Count - 3));
			layout.Spacing = 10;
            inputs.Add(layout);
		}
		else
		{
            inputs.Add(GenerateInputs(viewModel.AdditionalColumns, null));
        }

    }

    public StackLayout GenerateSingleInput(string labelName, string initialValue)
	{
		Entry entry = new Entry();
		entry.Text = initialValue;
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

	public StackLayout GenerateInputs(List<string> additionalColumns, List<string> initialValues)
	{
        StackLayout stackLayout = new StackLayout();
		stackLayout.Orientation= StackOrientation.Vertical;
		stackLayout.VerticalOptions = LayoutOptions.Start;

		for(int i = 0; i < additionalColumns.Count; ++i)
		{
			StackLayout inputLayout = GenerateSingleInput(additionalColumns[i], initialValues != null ? initialValues[i] : "");
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