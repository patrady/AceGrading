using System.Windows;
using System.Windows.Controls;

namespace AceGrading
{
    public partial class TestManagement : UserControl
    {
        public TestManagement()
        {
            InitializeComponent();
        }

        private void ViewTest_Click(object sender, RoutedEventArgs e)
        {
            Tests_Grid.Visibility = Visibility.Hidden;
            ViewTest_Grid.Visibility = Visibility.Visible;
        }

        
        private void FinishViewTest_Click(object sender, RoutedEventArgs e)
        {
            Tests_Grid.Visibility = Visibility.Visible;
            ViewTest_Grid.Visibility = Visibility.Hidden;
        }
    }
}
