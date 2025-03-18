using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers;

internal static class VolunteerManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    public static void ValidateVolunteer(BO.Volunteer boVolunteer)
    {
        // בדיקת תקינות כתובת אימייל
        if (!Regex.IsMatch(boVolunteer.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
        {
            throw new BO.BlInvalidDataException("Invalid email format");
        }

        // בדיקת מספר טלפון (נניח בפורמט ישראלי)
        if (!Regex.IsMatch(boVolunteer.PhoneNumber, @"^\d{10}$"))
        {
            throw new BO.BlInvalidDataException("Invalid phone number format");
        }

        // בדיקת תקינות ת.ז
        if (!IsValidIsraeliID(boVolunteer.Id))
        {
            throw new BO.BlInvalidDataException("Invalid Israeli ID number");
        }

        if (!IsValidAddress(boVolunteer.Latitude, boVolunteer.Longitude))
        {
            throw new BO.BlInvalidDataException("Address cannot be empty");
        }
    }

    public static bool IsValidIsraeliID(int id)
    {

        string idStr = id.ToString().PadLeft(9, '0');
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int num = (idStr[i] - '0') * ((i % 2) + 1);
            sum += num > 9 ? num - 9 : num;
        }
        return sum % 10 == 0;
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
