using PL.Call;
using PL.Volunteer;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int[]? CallByStatus { get; set; }
        public MainWindow()
        {
            CallByStatus = s_bl.Call.CallByStatus();

            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void UpdateRiskRange_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.UpdateRiskRange(RiskRange);
        }
        private void btnAddOneMinute_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.UpdateClock(BO.TypeOfTime.Minute);
        }
        private void btnAddOneHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.UpdateClock(BO.TypeOfTime.Hour);
        }
        private void btnAddOneDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.UpdateClock(BO.TypeOfTime.Day);
        }
        private void btnAddOneMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.UpdateClock(BO.TypeOfTime.Month);
        }
        private void btnAddOneYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Admin.UpdateClock(BO.TypeOfTime.Year);
        }

        private void clockObserver()
        {
            CurrentTime = s_bl.Admin.GetClock();
        }
        private void configObserver()
        {
            RiskRange = s_bl.Admin.GetRiskRange();

        }
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));

        public TimeSpan RiskRange 
        {
            get { return (TimeSpan)GetValue(RiskRangeProperty); }
            set { SetValue(RiskRangeProperty, value); }
        }

        public static readonly DependencyProperty  RiskRangeProperty=
            DependencyProperty.Register("RiskRange", typeof(TimeSpan), typeof(MainWindow));

        private void Load_Window(object sender, RoutedEventArgs e)
        {
            RiskRange = s_bl.Admin.GetRiskRange();
            CurrentTime = s_bl.Admin.GetClock();
            s_bl.Admin.AddClockObserver(clockObserver);
            s_bl.Admin.AddConfigObserver(configObserver);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);
            s_bl.Admin.RemoveConfigObserver(configObserver);
        }

        private void btnShowVolunteers_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerListWindow().Show();
        }

        private void btnShowCalls_Click(object sender, RoutedEventArgs e)
        {
            new CallListWindow().Show();
        }
        private void InitializationOrReset(string which)
        {
            var result = MessageBox.Show($"Are you sure you want to {which}?",
                             "Confirmation",
                             MessageBoxButton.OKCancel,
                             MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
            {
                foreach (Window win in Application.Current.Windows) { if (win != this) win.Close(); }
                if (which == "reset")
                    s_bl.Admin.Reset();
                else
                    s_bl.Admin.Initialization();
                MessageBox.Show($"{which} completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"{which} canceled.", "Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnIntialize_Click(object sender, RoutedEventArgs e)
        {
            InitializationOrReset("Initialization");
           
        }

        private void btnResetDB_Click(object sender, RoutedEventArgs e)
        {
            InitializationOrReset("reset");

        }

    }
}