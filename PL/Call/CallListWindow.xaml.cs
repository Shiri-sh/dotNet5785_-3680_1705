using PL.Call;
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
        public BO.Status Status { get; set; } = BO.Status.Open;

        public BO.CallInList? SelectedCall { get; set; }
        public CallListWindow(BO.Status statusTofilter = BO.Status.None)
        {
            this.Status = statusTofilter;
            InitializeComponent();
            filterListByStatus();

        }
        public IEnumerable<BO.CallInList> CallList
        {
            get { return (IEnumerable<BO.CallInList>)GetValue(CallListProperty); }
            set { SetValue(CallListProperty, value); }
        }

        public static readonly DependencyProperty CallListProperty =
            DependencyProperty.Register("CallList", typeof(IEnumerable<BO.CallInList>), typeof(CallListWindow), new PropertyMetadata(null));



        private void queryCallList()
            => CallList = (KindOfCall == BO.KindOfCall.None) ?
                s_bl?.Call.CallList()! : s_bl?.Call.CallList(BO.CallInListObjects.KindOfCall, KindOfCall, null)!;
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
                new CallWindow().Show();
        }
        //private void DeleteCallButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("are you sure you want to delete?");
        //    try
        //    {
        //        s_bl.Call.DeleteCall(SelectedCall!.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"error: {ex.Message}");
        //    }
        //}

        private void FilterListByStatus(object sender, SelectionChangedEventArgs e)

              => filterListByStatus();
        private void filterListByStatus()=>
            CallList = (Status == BO.Status.None) ?
                s_bl?.Call.CallList()! : s_bl?.Call.CallList(BO.CallInListObjects.Status, Status.ToString(),null)!;
    }
}
