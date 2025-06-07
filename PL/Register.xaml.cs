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

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public int? Id { get; set; }=null;
        public string Password { get; set; } = "";
        public Register()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Id==null ||Id<100000000 ||Id>999999999)
                {
                    throw new Exception("enter correct id");
                }
                if(Password == "")
                {
                    throw new Exception("enter corect password");
                }
                BO.Position p = s_bl.Volunteer.Login(Id??0,Password);
                if (p == BO.Position.Volunteer)
                {
                    new VolunteerWindow(Id ?? 0,p).Show();
                }
                else {
                    var result = MessageBox.Show(
                        "do you want enter to manager zone?",
                        "choose window",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if( result == MessageBoxResult.Yes){
                        new MainWindow(Id ?? 0).Show(); }
                    else { new VolunteerWindow(Id ?? 0).Show(); };
                }       
            }
            catch(Exception ex) {
                MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
