using CollectionManager.ViewModels;

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
        collection.Content = ItemCollectionGrid(viewModel.Columns, viewModel.Items.ToList());
    }

    private Border GridBorderElement(View element)
	{
		Border border = new Border();
		border.Style = (Style)Resources["borderCell"];
		border.Content = element;
		return border;
	}

    string path = @"C:\Users\HARDPC\Desktop\1664828258199964268.gif";
    private Grid ItemCollectionGrid(List<string> header, List<Models.Item> collection)
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
            grid.Add(GridBorderElement(el), c, 0);
        }

        for(int r = 0; r < collection.Count(); ++r)
        {
            Image image = new Image
            {
                Source = ImageSource.FromFile(path),
                WidthRequest = 200,
                HeightRequest = 200,
            };
            grid.Add(GridBorderElement(image), 0, r+1);

            for (int c = 1; c < collection[r].Values.Count; ++c)
            {
                Label el = CellLabel(collection[r].Values[c]);
                grid.Add(GridBorderElement(el), c, r+1);
            }
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