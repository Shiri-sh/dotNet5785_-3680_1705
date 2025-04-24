using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net;
using System.IO;
using BO;
//using Newtonsoft.Json;
using System.Text.Json;

namespace Helpers;

internal static class Tools
{
    /// <summary>
    /// Converts an object's properties to a string representation.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="t">The object whose properties are to be converted.</param>
    /// <returns>A string that represents the object's properties in a readable format.</returns>

    public static string ToStringProperty<T>(this T t) {
        string str = "";
        foreach (PropertyInfo item in typeof(T).GetProperties())
        {
            var value = item.GetValue(t, null);
            str += item.Name + ": ";
            if (value is not string && value is IEnumerable)
            {
                str += "\n";
                foreach (var it in (IEnumerable<object>)value)
                {
                    str += it.ToString() + '\n';
                }
            }
            else
                str += value?.ToString() + '\n';
        }
        return str;
    }

    public static double[]? GetCoordinates(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return null;
        }
        string url = $"https://geocode.maps.co/search?q={Uri.EscapeDataString(address)}&api_key=679a8da6c01a6853187846vomb04142";
        try
        {
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(url);
                var result = JsonSerializer.Deserialize<GeocodeResponse[]>(response);
                if (result == null || result.Length == 0)
                {
                    throw new BO.BlInvalidDataException("The address is invalid.");
                }
                double latitude = double.Parse(result[0].lat);
                double longitude = double.Parse(result[0].lon);
                return [latitude, longitude];
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public class GeocodeResponse
    {
        public string lat { get; set; }
        public string lon { get; set; }
    }

    /// <summary>
    /// Calculates the distance between a volunteer and a reading based on the coordinates of both.
    /// </summary>
    /// <param name="volunteer">המתנדב</param>
    /// <param name="call">הקריאה</param>
    /// <returns>The distance in kilometers between the volunteer and the reading</returns>
    internal static double GetDistance(DO.Volunteer volunteer, DO.Call call)
    {
        const double EarthRadius = 6371;
        if (volunteer.Latitude == null || volunteer.Longitude == null)
        {
            throw new BlNullPropertyException("Volunteer latitude or longitude cannot be null.");
        }
        double lat1 = volunteer.Latitude.Value * (Math.PI / 180);
        double lon1 = volunteer.Longitude.Value * (Math.PI / 180);
        double lat2 = call.Latitude * (Math.PI / 180);
        double lon2 = call.Longitude * (Math.PI / 180);
        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = EarthRadius * c;
        return distance;
    }

}
