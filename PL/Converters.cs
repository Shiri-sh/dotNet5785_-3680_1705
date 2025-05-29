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
