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
using System.Windows.Threading;

namespace PL.Call;

/// <summary>
/// Interaction logic for ChooseCallInListWindow.xaml
/// </summary>
public partial class ChooseCallInListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.KindOfCall KindOfCall { get; set; } = BO.KindOfCall.None;
    public string UpdateAddress { get; set; } = "";
    private volatile DispatcherOperation? _observerOperationCall = null;
    private volatile DispatcherOperation? _observerOperationVol = null;

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
        OpenCallList = s_bl?.Call.GetOpenCallByVolunteer(CurrentVolunteer!.Id, KindOfCall==BO.KindOfCall.None?null:KindOfCall, null)!.Where(c => c.DistanceFromVol <= CurrentVolunteer.MaximumDistanceForReading)!;
    }

    private void callListObserver()
    {
        if (_observerOperationCall is null || _observerOperationCall.Status == DispatcherOperationStatus.Completed)
            _observerOperationCall = Dispatcher.BeginInvoke(() =>
            {
                queryCallList();
            });
    }
    private void ObservedVolunteer()
    {
        if (_observerOperationVol is null || _observerOperationVol.Status == DispatcherOperationStatus.Completed)
            _observerOperationVol = Dispatcher.BeginInvoke(() =>
            {

                int id = CurrentVolunteer!.Id;
                CurrentVolunteer = null;
                CurrentVolunteer = s_bl.Volunteer.Read(id);
            });
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentVolunteer!.Id != 0)
            s_bl.Volunteer.AddObserver(CurrentVolunteer!.Id, ObservedVolunteer);
        s_bl.Call.AddObserver(callListObserver);
        LoadVolunteerMap();
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
        queryCallList();
    }

    private void FilterListByKindOfCall(object sender, SelectionChangedEventArgs e)
    
            =>queryCallList();
    private void ChooseCallToTreat(object sender, RoutedEventArgs e)
    {
        try
        {
            if (SelectedCall != null && CurrentVolunteer != null && CurrentVolunteer.CallInProgress == null)
            {
                s_bl.Call.CooseCall(CurrentVolunteer!.Id, SelectedCall!.Id);
                MessageBox.Show($"choosen call succesfully:{CurrentVolunteer.CallInProgress}");
            }
            else
                MessageBox.Show("you cant take another call, first finish with your current call");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"error:{ex}");
        }
    }
    private async void LoadVolunteerMap()
    {
        if (CurrentVolunteer == null)
        {
            MessageBox.Show("אין מתנדב נבחר.");
            return;
        }

        double? lat = CurrentVolunteer.Latitude;
        double? lon = CurrentVolunteer.Longitude;

        string html = $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8' />
  <title>Volunteer Map</title>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <link rel='stylesheet' href='https://unpkg.com/leaflet/dist/leaflet.css' />
  <style>#map {{ height: 100vh; width: 100%; margin: 0; }}</style>
</head>
<body>
  <div id='map'></div>
  <script src='https://unpkg.com/leaflet/dist/leaflet.js'></script>
  <script>
    var map = L.map('map').setView([{lat}, {lon}], 15);
    L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png').addTo(map);
    L.marker([{lat}, {lon}]).addTo(map).bindPopup('אתה כאן').openPopup();
  </script>
</body>
</html>";

        string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "volunteer_map.html");
        System.IO.File.WriteAllText(tempPath, html);

        await MapBrowser.EnsureCoreWebView2Async();
        MapBrowser.Source = new Uri(tempPath);
    }



}
