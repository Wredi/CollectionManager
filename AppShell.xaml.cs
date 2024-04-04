namespace CollectionManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Pages.ViewCollectionPage), typeof(Pages.ViewCollectionPage));
        }
    }
}