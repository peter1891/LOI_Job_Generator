using LOI_Job_Generator.Events;
using LOI_Job_Generator.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LOI_Job_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;

            EventService.ItemCollectionChanged += OnItemCollectionChanged;
        }

        private void OnItemCollectionChanged(object? sender, EventArgs e)
        {
            StatusScrollViewer.ScrollToBottom();
        }
    }
}