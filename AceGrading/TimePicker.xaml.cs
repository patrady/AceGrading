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
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
        }
    }

    public class TimePickerCustom : TimePicker
    {
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TimePickerCustom), new UIPropertyMetadata(DateTime.Now, new PropertyChangedCallback(UpdateTime)));

        protected static void UpdateTime(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimePickerCustom timePicker = d as TimePickerCustom;
            timePicker.InvalidateVisual();
        }
    }
}
