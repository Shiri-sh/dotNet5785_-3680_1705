//using BlImplementation;
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

    /// <summary>
    /// Validates the data of a volunteer, including email, phone number, Israeli ID, and address.
    /// </summary>
    /// <param name="boVolunteer">The volunteer object to validate.</param>
    /// <exception cref="BO.BlInvalidDataException">Thrown when any of the volunteer's data is invalid, such as incorrect email, phone number, ID, or address.</exception>
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

        //if (!Tools.IsValidAddress(boVolunteer.Latitude, boVolunteer.Longitude))
        //{
        //    throw new BO.BlInvalidDataException("Address not exist");
        //}
    }
    /// <summary>
    /// Validates whether a given Israeli ID is valid according to the Luhn algorithm.
    /// </summary>
    /// <param name="id">The Israeli ID number to validate.</param>
    /// <returns>True if the ID is valid, otherwise false.</returns>
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
}
