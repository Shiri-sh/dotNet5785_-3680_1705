using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Helpers;

internal static class Tools
{
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
    public static bool IsValidAddress(double? lon, double? lat)
    {
        string requestUri = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}";

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = client.Send(new HttpRequestMessage(HttpMethod.Get, requestUri));

        if (!response.IsSuccessStatusCode) return false;

        string jsonResponse = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<OSMGeocodeResponse>(jsonResponse);

        return !string.IsNullOrWhiteSpace(result?.display_name);
    }
    private class OSMGeocodeResponse
    {
        public string display_name { get; set; }
    }
}
