using PL.Call;
using PL.Volunteer;
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
using System.Windows.Shapes;

namespace PL.Call
{
    /// <summary>
    /// Interaction logic for CallListWindow.xaml
    /// </summary>
    public partial class CallListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.KindOfCall KindOfCall { get; set; } = BO.KindOfCall.None;
        public BO.Status Status { get; set; } = BO.Status.None;

        public int Id { get; set; }
        public BO.CallInList? SelectedCall { get; set; }
        public CallListWindow(int id,BO.Status statusTofilter = BO.Status.None)
        {
            Id = id;
            this.Status = statusTofilter;
            InitializeComponent();

        }
        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));



        private void queryCallList()
        {
            CallList = (KindOfCall == BO.KindOfCall.None) ?
               s_bl?.Call.CallList()! : s_bl?.Call.CallList(BO.CallInListObjects.KindOfCall, KindOfCall, null)!;
            CallList = (Status == BO.Status.None) ?
               CallList : CallList.Where(c=>c.Status==Status);
        }
    
        private void FilterListByKindOfCall(object sender, SelectionChangedEventArgs e)
          =>
           queryCallList();
        private void callListObserver()
            => queryCallList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Call.AddObserver(callListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(callListObserver);

        private void AddNewCall(object sender, RoutedEventArgs e)
        {
            new CallWindow().Show();
        }

        private void ChooseCallToUpdate(object sender, MouseButtonEventArgs e)
        {
            if (SelectedCall != null)
                new CallWindow(SelectedCall!.CallId).Show();
        }

        private void FilterListByStatus(object sender, SelectionChangedEventArgs e)

              => queryCallList();

        private void DeleteCallButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete this call?",
                          "Confirmation",
                          MessageBoxButton.OKCancel,
                          MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    s_bl.Call.DeleteCall(SelectedCall!.CallId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error: {ex.Message}");
                }
            }
        }

        private void CencelCallButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to cancel this call?",
                          "Confirmation",
                          MessageBoxButton.OKCancel,
                          MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    s_bl.Call.UpdateCancelCall(Id, SelectedCall!.CallId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error: {ex.Message}");
                }
            }
        }
    }
}
