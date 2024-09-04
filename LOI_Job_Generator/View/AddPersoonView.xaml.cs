using LOI_Job_Generator.Events;
using LOI_Job_Generator.ViewModel;
using System.Windows;

namespace LOI_Job_Generator.View
{
    /// <summary>
    /// Interaction logic for AddPersoonView.xaml
    /// </summary>
    public partial class AddPersoonView : Window
    {
        public AddPersoonView(bool exportMode)
        {
            InitializeComponent();

            AddPersoonViewModel addPersoonViewViewModel = new AddPersoonViewModel(exportMode);
            this.DataContext = addPersoonViewViewModel;

            EventService.CloseAddWindow += OnClose;
        }

        private void OnClose(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
