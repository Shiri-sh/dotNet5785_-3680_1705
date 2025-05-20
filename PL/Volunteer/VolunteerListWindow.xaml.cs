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
    /// Interaction logic for VolunteerListWindow.xaml
    /// </summary>
    public partial class VolunteerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.KindOfCall KindOfCall { get; set; } = BO.KindOfCall.None;
        
        public BO.VolunteerInList? SelectedVolunteer { get; set; }
        public VolunteerListWindow()
        {
            InitializeComponent();
        }
        public IEnumerable<BO.VolunteerInList> VolunteerList
        {
            get { return (IEnumerable<BO.VolunteerInList>)GetValue(VolunteerListProperty); }
            set { SetValue(VolunteerListProperty, value); }
        }

        public static readonly DependencyProperty VolunteerListProperty =
            DependencyProperty.Register("VolunteerList", typeof(IEnumerable<BO.VolunteerInList>), typeof(VolunteerListWindow), new PropertyMetadata(null));

        private void FilterListByKindOfCall(object sender, SelectionChangedEventArgs e)
           =>
            VolunteerList = (KindOfCall == BO.KindOfCall.None) ?
                  s_bl?.Volunteer.ReadAll()! : s_bl?.Volunteer.ReadAll(null, BO.VoluteerInListObjects.KindOfCall,KindOfCall)!;

        private void queryVolunteerList()
            => VolunteerList = (KindOfCall == BO.KindOfCall.None) ?
                s_bl?.Volunteer.ReadAll()! : s_bl?.Volunteer.ReadAll(null, BO.VoluteerInListObjects.KindOfCall,KindOfCall)!;

        private void volunteerListObserver()
            => queryVolunteerList();
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
            => s_bl.Volunteer.AddObserver(volunteerListObserver);

        private void Window_Closed(object sender, EventArgs e)
            => s_bl.Volunteer.RemoveObserver(volunteerListObserver);

        private void AddNewVolunteer(object sender, RoutedEventArgs e)
        {
            new VolunteerWindow().Show();
        }

        private void ChooseVolunteerToUpdate(object sender, MouseButtonEventArgs e)
        {
            if(SelectedVolunteer!=null)
              new VolunteerWindow(SelectedVolunteer.Id).Show();
        }

        private void DeleteVolunteerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("are you sure you want to delete?");
            try
            {
                s_bl.Volunteer.DeleteVolunteer(SelectedVolunteer!.Id);
            }
            catch (Exception ex) {
                MessageBox.Show($"error: {ex.Message}");
            }
        }
    }
}
