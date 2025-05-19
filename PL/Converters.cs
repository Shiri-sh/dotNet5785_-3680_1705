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
        if(status =="Add") return Visibility.Collapsed; //בהוספה אין לו מקום בכלל לערך הזה 
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
