using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AceGrading
{
    /// <summary>
    /// Interaction logic for TestTakingManagement.xaml
    /// </summary>
    public partial class TestTakingManagement : UserControl
    {
        public TestTakingManagement()
        {
            InitializeComponent();
        }

        private void StartTestSelection_Click(object sender, RoutedEventArgs e)
        {
            ClassroomGrid.Visibility = Visibility.Visible;
            TestSetup_Grid.Visibility = Visibility.Visible;
        }

        private void BackToTestSelection_Click(object sender, RoutedEventArgs e)
        {
            TestSetup_Grid.Visibility = Visibility.Hidden;
            ClassroomGrid.Visibility = Visibility.Hidden;
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            TestSetup_Grid.Visibility = Visibility.Hidden;
            StartTest_Grid.Visibility = Visibility.Visible;
        }

        private void EndTest_Click(object sender, RoutedEventArgs e)
        {
            StartTest_Grid.Visibility = Visibility.Hidden;
            TestSetup_Grid.Visibility = Visibility.Hidden;
            ClassroomGrid.Visibility = Visibility.Hidden;
        }
    }
}
