using BO;
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

namespace PL.Call;

/// <summary>
/// Interaction logic for ChooseCallInListWindow.xaml
/// </summary>
public partial class ChooseCallInListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.KindOfCall KindOfCall { get; set; } = BO.KindOfCall.None;
    public string UpdateAddress { get; set; } = "";

    public ChooseCallInListWindow(int id)
    {
        CurrentVolunteer = s_bl.Volunteer.Read(id);
        InitializeComponent();
    }
    public BO.OpenCallInList? SelectedCall
    {
        get { return (BO.OpenCallInList?)GetValue(SelectedCallProperty); }
        set { SetValue(SelectedCallProperty, value); }
    }

    public static readonly DependencyProperty SelectedCallProperty =
        DependencyProperty.Register("SelectedCall", typeof(BO.OpenCallInList), typeof(ChooseCallInListWindow), new PropertyMetadata(null));

    public BO.Volunteer? CurrentVolunteer
    {
        get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
        set { SetValue(CurrentVolunteerProperty, value); }
    }

    public static readonly DependencyProperty CurrentVolunteerProperty =
        DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(ChooseCallInListWindow), new PropertyMetadata(null));

    public IEnumerable<BO.OpenCallInList> OpenCallList
    {
        get { return (IEnumerable<BO.OpenCallInList>)GetValue(OpenCallListProperty); }
        set { SetValue(OpenCallListProperty, value); }
    }

    public static readonly DependencyProperty OpenCallListProperty =
        DependencyProperty.Register("OpenCallList", typeof(IEnumerable<BO.OpenCallInList>), typeof(ChooseCallInListWindow), new PropertyMetadata(null));



    private void queryCallList()
    {
        OpenCallList = s_bl?.Call.GetOpenCallByVolunteer(CurrentVolunteer!.Id, KindOfCall, null)!.Where(c => c.DistanceFromVol <= CurrentVolunteer.MaximumDistanceForReading)!;
    }

    private void callListObserver()
    => queryCallList();
    private void ObservedVolunteer()
    {
        int id = CurrentVolunteer!.Id;
        CurrentVolunteer = null;
        CurrentVolunteer = s_bl.Volunteer.Read(id);
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentVolunteer!.Id != 0)
            s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, ObservedVolunteer);
        s_bl.Call.AddObserver(callListObserver);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (CurrentVolunteer != null && CurrentVolunteer.Id != 0)
            s_bl.Volunteer.RemoveObserver(CurrentVolunteer.Id, ObservedVolunteer);
        s_bl.Call.RemoveObserver(callListObserver);
    }

    private void DisplayDescription(object sender, MouseButtonEventArgs e)
    {
        MessageBox.Show($"description:{SelectedCall!.Description}");
    }

    private void UpdateAddress_click(object sender, RoutedEventArgs e)
    {

        s_bl.Volunteer.UpdateVolunteer(CurrentVolunteer!.Id,
        new BO.Volunteer()
        {
            Id = CurrentVolunteer.Id,
            Name = CurrentVolunteer.Name,
            PhoneNumber = CurrentVolunteer.PhoneNumber,
            Email = CurrentVolunteer.Email,
            Position = CurrentVolunteer.Position,
            Password = CurrentVolunteer.Password,
            Active = CurrentVolunteer.Active,
            CurrentAddress = UpdateAddress,
            Latitude = CurrentVolunteer.Latitude,
            Longitude = CurrentVolunteer.Longitude,
            MaximumDistanceForReading = CurrentVolunteer.MaximumDistanceForReading,
            TypeOfDistance = CurrentVolunteer.TypeOfDistance,
            SumCancledCalls = CurrentVolunteer.SumCancledCalls,
            SumCaredCalls = CurrentVolunteer.SumCaredCalls,
            SumIrelevantCalls = CurrentVolunteer.SumIrelevantCalls,
            CallInProgress = CurrentVolunteer?.CallInProgress,

        });
    }

    private void FilterListByKindOfCall(object sender, SelectionChangedEventArgs e)
    
            =>queryCallList();

    private void ChooseCallToTreat(object sender, RoutedEventArgs e)
    {
        try
        {

            if (SelectedCall != null && CurrentVolunteer != null && CurrentVolunteer.CallInProgress == null)
                s_bl.Call.CooseCall(CurrentVolunteer!.Id, SelectedCall!.Id);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"error:{ex}");
        }
    }
}
