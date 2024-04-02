using CollectionManager.Data;

namespace CollectionManager
{
    public partial class App : Application
    {
        public static CollectionRepository CollectionRepo { get; private set; }
        public App(CollectionRepository repository)
        {
            InitializeComponent();

            MainPage = new AppShell();
            CollectionRepo = repository;
        }
    }
}