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

        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(TimePicker), new UIPropertyMetadata(DateTime.Now));

        public bool LimitTo24Hour
        {
            get { return (bool)GetValue(LimitTo24HourProperty); }
            set { SetValue(LimitTo24HourProperty, value); }
        }

        public static readonly DependencyProperty LimitTo24HourProperty =
            DependencyProperty.Register("LimitTo24Hour", typeof(bool), typeof(TimePicker), new PropertyMetadata(false));

        private void TimeIncrementHour()
        {
            this.Time = this.Time.AddHours(1);
            if (this.LimitTo24Hour)
                CheckIncrementTime();
        }
        private void TimeDecrementHours()
        {
            this.Time = this.Time.AddHours(-1);
            if (this.LimitTo24Hour)
                CheckDecrementTime();
        }
        private void TimeIncrementMinutes()
        {
            this.Time = this.Time.AddMinutes(1);
            if (this.LimitTo24Hour)
                CheckIncrementTime();
        }
        private void TimeDecrementMinutes()
        {
            this.Time = this.Time.AddMinutes(-1);
            if (this.LimitTo24Hour)
                CheckDecrementTime();
        }
        private void TimeIncrementAMPM()
        {
            this.Time = this.Time.AddHours(12);
            if (this.LimitTo24Hour)
                CheckIncrementTime();
        }
        private void TimeDecrementAMPM()
        {
            this.Time = this.Time.AddHours(-12);
            if (this.LimitTo24Hour)
                CheckDecrementTime();
        }
        private void CheckDecrementTime()
        {

            if (this.Time < DateTime.Now)
                this.Time = this.Time.AddHours(24);
        }
        private void CheckIncrementTime()
        {
            if (this.Time > DateTime.Now.AddHours(24))
                this.Time = this.Time.AddHours(-24);
        }

        private void UpHourClick(object sender, RoutedEventArgs e)
        {
            TimeIncrementHour();
        }
        private void DownHourClick(object sender, RoutedEventArgs e)
        {
            TimeDecrementHours();
        }
        private void UpMinuteClick(object sender, RoutedEventArgs e)
        {
            TimeIncrementMinutes();
        }
        private void DownMinuteClick(object sender, RoutedEventArgs e)
        {
            TimeDecrementMinutes();
        }
        private void UpAMPMClick(object sender, RoutedEventArgs e)
        {
            TimeIncrementAMPM();
        }
        private void DownAMPMClick(object sender, RoutedEventArgs e)
        {
            TimeDecrementAMPM();
        }
    }
}
