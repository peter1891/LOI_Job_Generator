using LOI_Job_Generator.Model;
using LOI_Job_Generator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LOI_Job_Generator.View
{
    /// <summary>
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : UserControl
    {
        public ItemView(PersoonModel persoonModel)
        {
            InitializeComponent();

            ItemViewModel itemViewModel = new ItemViewModel(persoonModel);
            this.DataContext = itemViewModel;
        }
    }
}
