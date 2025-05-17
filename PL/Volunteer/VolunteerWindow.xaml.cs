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
    /// Interaction logic for VolunteerWindow.xaml
    /// </summary>
    public partial class VolunteerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private string AddOrUpdate = "Add";
        private int id = 0;
        public VolunteerWindow(int id)
        {
            AddOrUpdate = id == 0 ? "Add" : "Update";

            InitializeComponent();
            this.id = id;
            CurrentVolunteer = (id != 0) ? s_bl.Volunteer.Read(id)! :
                new BO.Volunteer() {
                        Id                                 = 0,
                       Name                                 = "",
                       PhoneNumber                          = "",
                       Email                                = "",
                       Position                             = BO.Position.Volunteer,
                       Password                             = "",
                       Active                               = true,
                       CurrentAddress                       = "",
                       Latitude                             = 0,
                       Longitude                            = 0,
                       MaximumDistanceForReading            = 0,
                       TypeOfDistance                       = BO.TypeOfDistance.Aerial,
                       SumCancledCalls                      = 0,
                       SumCaredCalls                        = 0,
                       CallInProgress                       =null,
                       SumIrelevantCalls                    = 0

                 };

        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentCourse", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));
    }
}
