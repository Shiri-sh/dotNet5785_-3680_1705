using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;

class ConverterAddCollapsedUpdateVissable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        if(status == "Add") return Visibility.Collapsed; //בהוספה אין לו מקום בכלל לערך הזה 
       return Visibility.Visible;   //בעדכון זה יראה לעין
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
class ConverterAddWriteUpdateRead : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        return status == "Update";   //אם זה עדכון יהיה אופציה רק לקריאה
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
class ConverterClosedIrelavantTreatTreatnInRiskRead : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        return status == "Closed" || status == "Irelavant" || status== "BeingCared" || status== "TreatInRisk"; 
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
class ConverterClosedIrelavantTreatTreatnInRiskIEnable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        return status != "Closed" && status != "Irelavant" && status != "BeingCared" && status != "TreatInRisk";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

class ConverterClosedIrelavantRead : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = (string)value;
        return status == "Closed" || status == "Irelavant";   
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
class ConverterEnumKindOfCallToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return Brushes.White;
        BO.KindOfCall kindOfcall = (BO.KindOfCall)value;

        switch (kindOfcall)
        {
            case BO.KindOfCall.CableAssistance:
                return Brushes.Yellow;
            case BO.KindOfCall.RescueKid:
                return Brushes.Orange;
            case BO.KindOfCall.FirstAid:
                return Brushes.Plum;
            case BO.KindOfCall.changeWheel:
                return Brushes.Pink;
            case BO.KindOfCall.fuelOilWater:
                return Brushes.Purple;
            case BO.KindOfCall.None:
                return Brushes.White;
            default:
                return Brushes.White;

        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    } 
}
class ConverterEnumStatusToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return Brushes.White;
        BO.Status status = (BO.Status)value;

        switch (status)
        {
            case BO.Status.Open:
                return Brushes.Yellow;
            case BO.Status.TreatInRisk:
                return Brushes.Red;
            case BO.Status.Closed:
                return Brushes.Plum;
            case BO.Status.BeingCared:
                return Brushes.Pink;
            case BO.Status.Irelavant:
                return Brushes.LightBlue;
            default:
                return Brushes.White;

        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConverterEnableActive : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null?true:false;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConverterVisibiltyStackPanelForVolunteer : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Position)value == BO.Position.Volunteer ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

public class CallVisibilityMultiConverter : IValueConverter, IMultiValueConverter
{
    // לשימוש עם Binding רגיל
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Collapsed : Visibility.Visible;

    }

    // לשימוש עם MultiBinding
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return Visibility.Collapsed;

        bool isActive = values[0] is bool b1 && b1;
        bool callInProgress = values[1] ==null?true:false;

        return (isActive && callInProgress) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConverterEnableChangePosition:IValueConverter
{
    public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Position)values != BO.Position.Managar;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConverterStartSimulatorToStop : IValueConverter
{
    public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)values)
            return "stop simulator";
        return "start simulator";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConverterEnableButtonSimulator : IValueConverter
{
    public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
    
   => !(bool)values;
    

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
