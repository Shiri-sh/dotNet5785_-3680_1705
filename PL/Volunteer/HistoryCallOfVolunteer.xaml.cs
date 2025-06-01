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

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for HistoryCallOfVolunteer.xaml
    /// </summary>
    public partial class HistoryCallOfVolunteer : Window
    {
        
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int Id { get; set; } = 0;
        public BO.KindOfCall KindOfCall { get; set; } = BO.KindOfCall.None;
        public BO.CloseCallInListObjects? CloseCallInListObjects { get; set; } =null;

        public HistoryCallOfVolunteer(int id)
        {
            this.Id = id;
            InitializeComponent();
        }

        public IEnumerable<BO.ClosedCallInList> ClosedCallInList
        {
            get { return (IEnumerable<BO.ClosedCallInList>)GetValue(ClosedCallInListProperty); }
            set { SetValue(ClosedCallInListProperty, value); }
        }

        public static readonly DependencyProperty ClosedCallInListProperty =
            DependencyProperty.Register("ClosedCallInList", typeof(IEnumerable<BO.ClosedCallInList>), typeof(HistoryCallOfVolunteer), new PropertyMetadata(null));

        //סינונים ומיונים

        private void queryClosedCallInList()
        {
            ClosedCallInList =  s_bl?.Call.GetCloseCallByVolunteer(Id,KindOfCall,CloseCallInListObjects)! ;
        }

        private void FilterListByKindOfCall(object sender, SelectionChangedEventArgs e)
          => queryClosedCallInList();
        private void FilterListByCloseCallInListCollection(object sender, SelectionChangedEventArgs e)
        => queryClosedCallInList();
        private void ClosedCallInListObserver()
            => queryClosedCallInList();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Call.AddObserver(ClosedCallInListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Call.RemoveObserver(ClosedCallInListObserver);

       
    }
}
