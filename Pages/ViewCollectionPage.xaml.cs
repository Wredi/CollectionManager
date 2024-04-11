using CollectionManager.ViewModels;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace CollectionManager.Pages;

public partial class ViewCollectionPage : ContentPage
{
	public ViewCollectionPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
        ViewCollectionViewModel viewModel = (ViewCollectionViewModel)BindingContext;
		viewModel.OnAppearing();
        collection.Content = ItemCollectionGrid(
            viewModel.CollectionName,
            viewModel.Columns.Concat(new List<string> { "Controls" }).ToList(),
            viewModel.Items.ToList()
            );
        viewModel.ForceReRender += OnForceReRender;
    }

    private void OnForceReRender(object sender, EventArgs e)
    {
        ViewCollectionViewModel viewModel = (ViewCollectionViewModel)BindingContext;
        collection.Content = ItemCollectionGrid(
            viewModel.CollectionName,
            viewModel.Columns.Concat(new List<string> { "Controls" }).ToList(),
            viewModel.Items.ToList()
            );
    }

    private Border GridBorderElement(View element, Style style)
	{
		Border border = new Border();
		border.Style = style;
		border.Content = element;
		return border;
	}

    private StackLayout ControlButtons(Models.Item item)
    {
        StackLayout stack = new StackLayout();
        stack.Orientation = StackOrientation.Horizontal;
        stack.VerticalOptions = LayoutOptions.Center;
        stack.Padding = 10;

        Button editButton = new Button();
        editButton.SetBinding(Button.CommandProperty, "EditItemCommand");
        editButton.SetValue(Button.CommandParameterProperty, item);
        editButton.Text = "Edit";
        editButton.Background = new Color(0x10, 0xAB, 0xB4);
        editButton.TextColor = new Color(0xff, 0xff, 0xff);

        Button deleteButton = new Button();
        deleteButton.SetBinding(Button.CommandProperty, "RemoveItemCommand");
        deleteButton.SetValue(Button.CommandParameterProperty, item);
        deleteButton.Text = "Delete";
        deleteButton.Background = new Color(0xff, 0, 0x21);
        deleteButton.TextColor = new Color(0xff, 0xff, 0xff);

        stack.Add(editButton);
        stack.Add(deleteButton);
        return stack;
    }

    private Grid ItemCollectionGrid(string collectionName, List<string> header, List<Models.Item> collection)
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions = new ColumnDefinitionCollection(
                Enumerable.Repeat(new ColumnDefinition(GridLength.Auto), header.Count()).ToArray()
            );
        grid.RowDefinitions = new RowDefinitionCollection(
                Enumerable.Repeat(new RowDefinition(GridLength.Auto), collection.Count() + 1).ToArray()
            );

        for (int c = 0; c < header.Count(); ++c)
        {
			Label el = HeaderLabel(header[c]);
            grid.Add(GridBorderElement(el, (Style)Resources["borderCell"]), c, 0);
        }

        for(int r = 0; r < collection.Count(); ++r)
        {
            
            Style style = collection[r].GetStatus() == "Sold" ? (Style)Resources["soldBorderCell"] : (Style)Resources["borderCell"];
            Image image = new Image
            {
                Source = ImageSource.FromFile(collection[r].ImagePath(collectionName)),
                WidthRequest = 200,
                HeightRequest = 200,
                Margin = 10
            };
            
            grid.Add(GridBorderElement(image, style), 0, r+1);

            for (int c = 1; c < collection[r].Values.Count; ++c)
            {
                Label el = CellLabel(collection[r].Values[c]);
                grid.Add(GridBorderElement(el, style), c, r+1);
            }
            grid.Add(GridBorderElement(ControlButtons(collection[r]), (Style)Resources["borderCell"]), collection[r].Values.Count, r + 1);
        }

        return grid;
    }

    private Label HeaderLabel(string text)
    {
        Label label = NormalLabel(text);
        label.Style = (Style)Resources["cellHeaderLabel"];
        return label;
    }

    private Label CellLabel(string text) {
        Label label = NormalLabel(text);
        label.Style = (Style)Resources["cellLabel"];
        return label;
    }

    private Label NormalLabel(string text)
    {
        Label label = new Label();
        label.Text = text;
        label.Style = (Style)Resources["cellLabel"];
        return label;
    }


}