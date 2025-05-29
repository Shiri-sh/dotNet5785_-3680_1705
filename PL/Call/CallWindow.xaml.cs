using BO;
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

namespace PL.Call;

/// <summary>
/// Interaction logic for CallWindow.xaml
/// </summary>
public partial class CallWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public string StatusOfCall { get; set; } = "None";
    public string AddOrUpdate { get; set; } = "Add";

    private int Id = 0;
    public BO.Call? CurrentCall
    {
        get { return (BO.Call?)GetValue(CurrentCallProperty); }
        set { SetValue(CurrentCallProperty, value); }
    }

    public static readonly DependencyProperty CurrentCallProperty =
        DependencyProperty.Register("CurrentCall", typeof(BO.Call), typeof(CallWindow), new PropertyMetadata(null));
    public CallWindow(int id = 0)
    {
        AddOrUpdate = id == 0 ? "Add" : "Update";
        Id = id;
        CurrentCall = (id != 0) ? s_bl.Call.ReadCall(id)! :
            new BO.Call()
            {
                Id = 0,
                KindOfCall = BO.KindOfCall.None,
                AddressOfCall = "",
                Latitude = 0,
                Longitude = 0,
                OpeningTime = s_bl.Admin.GetClock(),
                FinishTime = null,
                Description = "",
                Status = BO.Status.None,
                CallAssignInList = null
            };
        StatusOfCall = CurrentCall.Status.ToString();
        InitializeComponent();
       
    }
    private bool FormatChecking()
    {
        if (CurrentCall?.KindOfCall == BO.KindOfCall.None)
            throw new Exception("Choose king of call");
        if (CurrentCall?.AddressOfCall != "" || CurrentCall?.AddressOfCall != null)
            throw new Exception("Write address");
        return true;

    }
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        
        try
        {
            if (FormatChecking()) ;
            if (AddOrUpdate == "Add")
            {
                s_bl.Call.AddCall(CurrentCall!);
            }
            else
            {
                s_bl.Call.UpdateCall(CurrentCall!);
            }
            MessageBox.Show("Operation completed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void ObservedCall()
    {
        int id = CurrentCall!.Id;
        CurrentCall = null;
        CurrentCall = s_bl.Call.ReadCall(id);
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentCall!.Id != 0)
            s_bl.Call.AddObserver(CurrentCall!.Id, ObservedCall);

    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (CurrentCall != null && CurrentCall.Id != 0)
            s_bl.Call.RemoveObserver(CurrentCall.Id, ObservedCall);
    }
}