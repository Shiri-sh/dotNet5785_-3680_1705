using BlApi;
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
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //public int[]? CallByStatus { get; set; }
        public int Id { get; set; }
        private volatile DispatcherOperation? _observerOperationConfig = null;
        private volatile DispatcherOperation? _observerOperationClock = null;
        private volatile DispatcherOperation? _observerOperationCallStatus = null;

        public MainWindow(int id)
        {
            CallByStatus = s_bl.Call.CallByStatus();
            Id = id;    
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
            if (_observerOperationClock is null || _observerOperationClock.Status == DispatcherOperationStatus.Completed)
                _observerOperationClock = Dispatcher.BeginInvoke(() =>
                {
                    CurrentTime = s_bl.Admin.GetClock();
                });
        }
        private void configObserver()
        {
            if (_observerOperationConfig is null || _observerOperationConfig.Status == DispatcherOperationStatus.Completed)
                _observerOperationConfig = Dispatcher.BeginInvoke(() =>
                {
                    RiskRange = s_bl.Admin.GetRiskRange();
                });
        }
        private void callByStatusObserver()
        {
            if (_observerOperationCallStatus is null || _observerOperationCallStatus.Status == DispatcherOperationStatus.Completed)
                _observerOperationCallStatus = Dispatcher.BeginInvoke(() =>
                {
                    CallByStatus = s_bl.Call.CallByStatus();
                });
        }
        public int[] CallByStatus
        {
            get { return (int[])GetValue(CallByStatusProperty); }
            set { SetValue(CallByStatusProperty, value); }
        }

        public static readonly DependencyProperty CallByStatusProperty =
            DependencyProperty.Register("CallByStatus", typeof(int[]), typeof(MainWindow));

        public bool RunSimulator
        {
            get { return (bool)GetValue(RunSimulatorProperty); }
            set { SetValue(RunSimulatorProperty, value); }
        }

        public static readonly DependencyProperty RunSimulatorProperty =
            DependencyProperty.Register("RunSimulator", typeof(bool), typeof(MainWindow));

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(MainWindow));

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
            s_bl.Call.AddObserver(callByStatusObserver);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            s_bl.Admin.RemoveClockObserver(clockObserver);
            s_bl.Admin.RemoveConfigObserver(configObserver);
            s_bl.Call.RemoveObserver(callByStatusObserver);
            RunSimulator = false;
            s_bl.Admin.StopSimulator();
        }

        private void btnShowVolunteers_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerListWindow().Show();
        }

        private void btnShowCalls_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string roleTag)
            {
                new CallListWindow(Id,(BO.Status)Enum.Parse(typeof(BO.Status), roleTag)).Show();
            }
            else
            {
                new CallListWindow(Id).Show();
            }
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
        private void btnSimulatorAction_click(object sender, RoutedEventArgs e)
        {
            if (RunSimulator)
            {
                RunSimulator = false;
                s_bl.Admin.StopSimulator();
            }
            else
            {
                RunSimulator=true;
                s_bl.Admin.StartSimulator(Interval); //stage 7
            }
        }
    }
}