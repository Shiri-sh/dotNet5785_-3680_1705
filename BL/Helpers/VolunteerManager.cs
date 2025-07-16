//using BlImplementation;
using BO;
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
    internal static ObserverManager Observers = new(); //stage 5 

    private static IDal s_dal = Factory.Get; //stage 4
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private static readonly Random s_rand = new();

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

        if (Tools.GetCoordinates(boVolunteer.CurrentAddress??null) == null)
        {
            throw new BO.BlInvalidDataException("Address not exist");
        }
    }
    /// <summary>
    /// Validates whether a given Israeli ID is valid according to the Luhn algorithm.
    /// </summary>
    /// <param name="id">The Israeli ID number to validate.</param>
    /// <returns>True if the ID is valid, otherwise false.</returns>
    public static bool IsValidIsraeliID(int id)
    {
        // המרת המספר למחרוזת והשלמה ל-9 ספרות עם אפסים מובילים
        string idStr = id.ToString().PadLeft(9, '0');
        // אם האורך אינו 9 לאחר ההשלמה – לא תקין
        if (idStr.Length != 9)
            return false;
        // בדיקת תקינות - סכום הספרות חייב להתחלק ב-10 ללא שארית
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    internal static void SimulatorActivityOfVolunteers()
    {
        List<BO.VolunteerInList> volunteers;
        lock (AdminManager.BlMutex) //stage 7
            volunteers = s_bl.Volunteer.ReadAll(true).ToList();
        foreach (var volunteer in volunteers)
        {
            if (volunteer.IdOfCall == null)
            {
                List<OpenCallInList> openCalls;
                lock (AdminManager.BlMutex)
                    openCalls = s_bl.Call.GetOpenCallByVolunteer(volunteer.Id).ToList();
                if (openCalls.Any())
                {
                    int callId = openCalls.Skip(s_rand.Next(0, openCalls.Count())).First()!.Id;
                        lock (AdminManager.BlMutex)
                            CallManager.CooseCall(volunteer.Id, callId);
                }
            }
            else
            {
                DO.Call call;
                DO.Volunteer volunteer1;
                DO.Assignment assignment;
                lock (AdminManager.BlMutex)
                {
                    call = s_dal.Call.Read(c => c.Id == volunteer.IdOfCall)!;
                    volunteer1 = s_dal.Volunteer.Read(v => v.Id == volunteer.Id)!;
                    assignment = s_dal.Assignment.Read(a => a.VolunteerId == volunteer.Id && a.TreatmentEndTime == null)!;
                }
               double dis = Tools.GetDistance(volunteer1, call);
                if (assignment.TreatmentEntryTime.AddMinutes(dis * 3 + 10) < s_dal.Config.Clock)
                    //עבר מספיק זמן
                    lock (AdminManager.BlMutex)
                        CallManager.UpdateEndCall(volunteer.Id, assignment.Id);
                //להוסיף את הההסתברות
                else
                {
                    if (s_rand.NextDouble() < 0.1)

                        lock (AdminManager.BlMutex)
                            CallManager.UpdateCancelCall(volunteer.Id, assignment.Id);
                }
            }
        }
    }
    public static async Task UpdateCoordinatesForVolunteerAddressAsync(DO.Volunteer dovol)
    {
        if(dovol.CurrentAddress is not null)
        {
            double[]? loc = await Tools.GetCoordinates(dovol.CurrentAddress);
            if (loc is not null)
            {
                dovol = dovol with { Latitude = loc[0], Longitude = loc[1] };
                lock (AdminManager.BlMutex)
                    s_dal.Volunteer.Update(dovol);
                Observers.NotifyListUpdated();
                Observers.NotifyItemUpdated(dovol.Id);
            }

        }
    }
}

