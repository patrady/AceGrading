using System.Windows;
using System.Windows.Controls;

namespace AceGrading
{
    public partial class RosterManagement : UserControl
    {
        public RosterManagement()
        {
            InitializeComponent();
        }

        private void EditClass_Click(object sender, RoutedEventArgs e)
        {
            ClassProfileGrid.Visibility = Visibility.Hidden;
            EditClassProfileGrid.Visibility = Visibility.Visible;
        }

        private void EditClassFinish_Click(object sender, RoutedEventArgs e)
        {
            ClassProfileGrid.Visibility = Visibility.Visible;
            EditClassProfileGrid.Visibility = Visibility.Hidden;
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            EditStudentProfileGrid.Visibility = Visibility.Visible;
            EditClassProfileGrid.Visibility = Visibility.Hidden;
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            EditStudentProfileGrid.Visibility = Visibility.Hidden;
            EditClassProfileGrid.Visibility = Visibility.Visible;
        }

        private void EditStudentFinish_Click(object sender, RoutedEventArgs e)
        {
            EditStudentProfileGrid.Visibility = Visibility.Hidden;
            EditClassProfileGrid.Visibility = Visibility.Visible;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            EditClassProfileGrid.Visibility = Visibility.Hidden;
            AddStudentProfileGrid.Visibility = Visibility.Visible;
        }

        private void AddStudentFinish_Click(object sender, RoutedEventArgs e)
        {
            EditClassProfileGrid.Visibility = Visibility.Visible;
            AddStudentProfileGrid.Visibility = Visibility.Hidden;
        }

        private void AddNewClass_Click(object sender, RoutedEventArgs e)
        {
            ClassProfileGrid.Visibility = Visibility.Hidden;
            AddClassProfileGrid.Visibility = Visibility.Visible;

        }

        private void FinishAddClass_Click(object sender, RoutedEventArgs e)
        {
            ClassProfileGrid.Visibility = Visibility.Visible;
            AddClassProfileGrid.Visibility = Visibility.Hidden;
        }

        private void AddClassStudent_Click(object sender, RoutedEventArgs e)
        {
            AddClassProfileGrid.Visibility = Visibility.Hidden;
            AddStudentProfileGrid.Visibility = Visibility.Visible;
        }

        private void DeleteClass_Click(object sender, RoutedEventArgs e)
        {
            EditClassProfileGrid.Visibility = Visibility.Hidden;
            ClassProfileGrid.Visibility = Visibility.Visible;
        }

        private void EditStudentFinish2_Click(object sender, RoutedEventArgs e)
        {
            EditStudentProfileGrid2.Visibility = Visibility.Hidden;
            AddClassProfileGrid.Visibility = Visibility.Visible;
        }

        private void EditStudent2_Click(object sender, RoutedEventArgs e)
        {
            EditStudentProfileGrid2.Visibility = Visibility.Visible;
            AddClassProfileGrid.Visibility = Visibility.Hidden;
        }
    }
}
