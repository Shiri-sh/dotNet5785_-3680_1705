using PL.Call;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Volunteer;

/// <summary>
/// Interaction logic for VolunteerWindow.xaml
/// </summary>
public partial class VolunteerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public string AddOrUpdate { get; set; } = "Add";
    public BO.Position UserPosition { get; set; }

    private int Id = 0;
    
    public BO.Volunteer? CurrentVolunteer
    {
        get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
        set { SetValue(CurrentVolunteerProperty, value); }
    }

    public static readonly DependencyProperty CurrentVolunteerProperty =
        DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));
    public VolunteerWindow(int id=0,BO.Position position=BO.Position.Managar)
    {
        AddOrUpdate = id == 0 ? "Add" : "Update";
        UserPosition= position == 0 ? BO.Position.Managar : BO.Position.Volunteer;
        InitializeComponent();
        Id = id;
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
                   SumIrelevantCalls                    = 0,
                   CallInProgress                       = null

             };

    }
    private bool formatCheck()
    {
        if (CurrentVolunteer?.Id.ToString().Length != 9)
            throw new Exception("id should be 9 number long");
        if (string.IsNullOrWhiteSpace(CurrentVolunteer.Name))
            throw new Exception("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(CurrentVolunteer.PhoneNumber) || CurrentVolunteer.PhoneNumber.ToString().Length!=10)
            throw new Exception("Phone number must be exactly 10 digits");

        if (string.IsNullOrWhiteSpace(CurrentVolunteer.Email) || !CurrentVolunteer.Email.Contains("@"))
            throw new Exception("Invalid email format");

        if (CurrentVolunteer.MaximumDistanceForReading < 0)
            throw new Exception("Maximum distance must be non-negative");

        if (string.IsNullOrWhiteSpace(CurrentVolunteer.CurrentAddress))
            throw new Exception("Address cannot be empty");
        return true;
    }
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(CurrentVolunteer?.Email, "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        try
        {
            if (formatCheck());
            if (AddOrUpdate == "Add")
            {
                s_bl.Volunteer.AddVolunteer(CurrentVolunteer!);
            }
            else
            {
                s_bl.Volunteer.UpdateVolunteer(Id, CurrentVolunteer!);
            }
            MessageBox.Show("Operation completed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void ObservedVolunteer() {
        int id = CurrentVolunteer!.Id;
        CurrentVolunteer= null;
        CurrentVolunteer=s_bl.Volunteer.Read(id);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentVolunteer!.Id != 0)
            s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, ObservedVolunteer);
    
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, ObservedVolunteer);
    }

    private void updateEndCall_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Call.UpdateEndCall(CurrentVolunteer!.Id,CurrentVolunteer!.CallInProgress!.CallId);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }

    private void chooseCall_Click(object sender, RoutedEventArgs e)
    {
        new ChooseCallInListWindow(CurrentVolunteer!.Id).Show();
    }

    private void updateCancelCall_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Call.UpdateCancelCall(CurrentVolunteer!.Id, CurrentVolunteer!.CallInProgress!.CallId);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"error: {ex.Message}");

        }
    }

    private void showHistoryClosedCalls_Click(object sender, RoutedEventArgs e)
    {
        new HistoryCallOfVolunteer(CurrentVolunteer!.Id).Show();
    }
}
