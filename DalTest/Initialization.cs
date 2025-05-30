﻿namespace DalTest;

using DalApi;
using Dal;
using DO;
using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class Initialization
{

    //private static IAssignment? s_dalAssignment;
    //private static ICall? s_dalCall;
    //private static IVolunteer? s_dalVolunteer;
    //private static IConfig? s_dalConfig;
    private static IDal? s_dal;
    private static readonly Random s_rand = new();
    private static int MIN_ID = 200000000;
    public static int MAX_ID = 400000000;
    public static int MIN_P = 10000000;
    public static int MAX_P = 99999999;
    public static int MIN_S = 100000;
    public static int MAX_S = 999999;
   
    

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">kind of Enum</typeparam>
    /// <param name="random"></param>
    /// <param name="range">a number that presents the amount of </param>
    /// <returns>an Enum value </returns>
    static T GetRandomEnumValue<T>(Random random, int range = 0) where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length - range));
    }
    /// <summary>
    /// intilize the array of volunteers in the DataSource with 15 volunteers
    /// </summary>
    private static void createVolunteer()
    {
        int counter = 0;
        string[] volunteerNames = { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Shir Levi", "Ron Catz", "Rut Levi", "Ayala Cohen", "Shiri Shahor", "Enana Fedut", "Tammy Dabin", "Sari Levin", "Michal Ortel" };
        string[] volunteerEmail = { " mimigins2005@gmail.com ", " frgvvv@gmail.com ", " rutrc@gmail.com ", " avigail6282@gmail.com ", " avitalbortz@gmail.com ", " ms0583242931@gmail.com ", " miri57566@gmail.com ", "noya.boruhov@gmail.com ", " naama0052@gmail.com ", "zipi.3382@gmail.com ", " shirishahor@gmail.com ", " t0583235695@gmail.com", " tamar86950@gmail.com ", " michalortzel@gmail.com ", " ys0583299134@gmail.com " };
        string[][] streetsNames = { ["Even Gvirol 11, Elad", "32.0579", "34"], ["Rabbi Hiyya, Elad", "32.049344", "34.963798"], ["Derech Menachem Begin, Petah Tikva", "32.069869", "34.914232"], ["Pinhas Rozen 12, Tel Aviv-Yafo", "32.108024", "34.827305"], ["Be'er Mayim Chaim 12, Bnei Brak", "32.083079", "34.841832"], ["Herzl Street 25, Tel Aviv", "32.060780", "34.770589"], ["Jabotinsky 15, Ramat Gan", "32.073667", "34.809965"], ["Allenby Street, Tel Aviv", "32.067519", "34.771222"], ["King George Street, Tel Aviv", "32.073428", "34.775037"], ["Dizengoff Street 120, Tel Aviv", "32.083687", "34.773541"], ["Ehad Ha'am Street 9, Tel Aviv", "32.065789", "34.776231"], ["Rothschild Boulevard, Tel Aviv", "32.063229", "34.774996"], ["Balfour Street 20, Bat Yam", "32.027325", "34.747369"], ["Shenkar Street 7, Herzliya", "32.164458", "34.836211"], ["Nordau Boulevard, Tel Aviv", "32.091236", "34.785009"], ["Ben Yehuda Street, Tel Aviv", "32.082246", "34.769322"], ["Hashalom Road, Givatayim", "32.069478", "34.808196"], ["Arlozorov Street 78, Tel Aviv", "32.085679", "34.782549"], ["HaYarkon Street 123, Tel Aviv", "32.089678", "34.769126"], ["King George Street, Tel Aviv", "32.073428", "34.775037"], ["Allenby Street, Tel Aviv", "32.067519", "34.771222"] };
        s_dal!.Volunteer!.Create(new(123456789, "Meni managar", "0525665732", "managar@gmail.com", Position.Managar, "man1234", true, "Even Gvirol 11, Elad", double.Parse("32.0579"), double.Parse("34"), MaximumDistanceForReading: s_rand.Next(1, 2500), TypeOfDistance: TypeOfDistance.Aerial));

        foreach (string volunteerName in volunteerNames)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal.Volunteer!.Read(vol => vol.Id == id) != null);
            string phoneNumber = "05" + Convert.ToString(s_rand.Next(MIN_P, MAX_P));
            string password = Convert.ToString(s_rand.Next(MIN_S, MAX_S)) + (char)s_rand.Next('A', 'Z' + 1) + (char)s_rand.Next(32, 127);
            string passwordHidden = "";
            foreach (char var in password)
            {
                passwordHidden += var + (char)s_rand.Next('A', 'Z' + 1);
            }
            string[] detailLocation = streetsNames[s_rand.Next(0, streetsNames.Length)];
            s_dal.Volunteer!.Create(new(id, volunteerName, phoneNumber, volunteerEmail[counter], Position.Volunteer, passwordHidden, true, detailLocation[0], double.Parse(detailLocation[1]), double.Parse(detailLocation[2]), MaximumDistanceForReading: s_rand.Next(1, 2500), TypeOfDistance: TypeOfDistance.Aerial));
            counter++;
        }
    }
    /// <summary>
    /// intilize the array of calls in the DataSource with 50 calls
    /// </summary>
    private static void createCall()
    {
        string[][] streetsNames = { ["Even Gvirol 11, Elad", "32.0579", "34"], ["Rabbi Hiyya, Elad", "32.049344", "34.963798"], ["Derech Menachem Begin, Petah Tikva", "32.069869", "34.914232"], ["Pinhas Rozen 12, Tel Aviv-Yafo", "32.108024", "34.827305"], ["Be'er Mayim Chaim 12, Bnei Brak", "32.083079", "34.841832"], ["Herzl Street 25, Tel Aviv", "32.060780", "34.770589"], ["Jabotinsky 15, Ramat Gan", "32.073667", "34.809965"], ["Allenby Street, Tel Aviv", "32.067519", "34.771222"], ["King George Street, Tel Aviv", "32.073428", "34.775037"], ["Dizengoff Street 120, Tel Aviv", "32.083687", "34.773541"], ["Ehad Ha'am Street 9, Tel Aviv", "32.065789", "34.776231"], ["Rothschild Boulevard, Tel Aviv", "32.063229", "34.774996"], ["Balfour Street 20, Bat Yam", "32.027325", "34.747369"], ["Shenkar Street 7, Herzliya", "32.164458", "34.836211"], ["Nordau Boulevard, Tel Aviv", "32.091236", "34.785009"], ["Ben Yehuda Street, Tel Aviv", "32.082246", "34.769322"], ["Hashalom Road, Givatayim", "32.069478", "34.808196"], ["Arlozorov Street 78, Tel Aviv", "32.085679", "34.782549"], ["HaYarkon Street 123, Tel Aviv", "32.089678", "34.769126"], ["King George Street, Tel Aviv", "32.073428", "34.775037"], ["Allenby Street, Tel Aviv", "32.067519", "34.771222"] };

        string[][] callDescriptionsMatrix =
        {
             [ "RescueKid", "Rescuing a locked child from a vehicle" ],
             [ "RescueKid", "Emergency call to rescue a child" ],
             [ "RescueKid", "Assistance in freeing a stuck child" ],
             [ "changeWheel", "Replacing a damaged wheel" ],
             [ "changeWheel", "Assistance in replacing a faulty tire" ],
             [ "changeWheel", "Help with installing a new wheel" ],
             [ "FirstAid", "Providing first aid to an injured person" ],
             [ "FirstAid", "Emergency treatment in a medical situation" ],
             [ "FirstAid", "First aid before medical team arrival" ],
             [ "CableAssistance", "Jumpstarting a car with cables" ],
             [ "CableAssistance", "Helping with a car that won't start" ],
             [ "CableAssistance", "Starting a car using cables" ],
             [ "fuelOilWater", "Filling fuel for a stranded vehicle" ],
             [ "fuelOilWater", "Adding oil to a car in need" ],
             [ "fuelOilWater", "Refueling a vehicle in an emergency" ],
             [ "RescueKid", "Rescuing a child in special situations" ],
             [ "RescueKid", "Urgent call to save a child" ],
             [ "changeWheel", "Service for replacing a wheel" ],
             [ "changeWheel", "Handling a faulty wheel" ],
             [ "FirstAid", "Providing initial medical assistance" ],
             [ "FirstAid", "Immediate help for the injured" ],
             [ "CableAssistance", "Jumpstarting a car with a dead battery" ],
             [ "CableAssistance", "Temporary fix for a vehicle's electrical system" ],
             [ "fuelOilWater", "Emergency refueling away from a station" ],
             [ "fuelOilWater", "Providing water for an overheated car" ],
             [ "fuelOilWater", "Adding essential fluids to a vehicle" ],
             [ "RescueKid", "Assistance for a locked-in child" ],
             [ "changeWheel", "Repairing a tire on a stranded car" ],
             [ "FirstAid", "Emergency care for injury situations" ],
             [ "CableAssistance", "Technical help to start a car" ],
             [ "fuelOilWater", "Adding emergency oil" ],
             [ "RescueKid", "Action to rescue a child" ],
             [ "changeWheel", "Professional wheel replacement" ],
             [ "FirstAid", "On-the-spot emergency care" ],
             [ "CableAssistance", "Cable-based car jumpstart service" ],
             [ "fuelOilWater", "Adding fuel in unusual circumstances" ],
             [ "RescueKid", "Rescuing children in complex circumstances" ],
             [ "changeWheel", "Emergency tire services" ],
             [ "FirstAid", "Initial care until medical assistance arrives" ],
             [ "CableAssistance", "Help for a car with battery issues" ],
             [ "fuelOilWater", "Filling water for an overheated engine" ],
             [ "RescueKid", "Assistance in child emergency situations" ],
             [ "changeWheel", "Quick replacement of a faulty wheel" ],
             [ "FirstAid", "Immediate medical assistance at the scene" ],
             [ "CableAssistance", "Service for starting a stalled vehicle" ],
             [ "fuelOilWater", "Completing fluids for the engine" ],
             [ "changeWheel", "Help with installing a new wheel" ],
             [ "FirstAid", "Providing first aid to an injured person" ],
             [ "FirstAid", "Emergency treatment in a medical situation" ],
             [ "FirstAid", "First aid before medical team arrival" ],
             [ "CableAssistance", "Jumpstarting a car with cables" ]
        };
        DateTime startTime = new DateTime(s_dal.Config.Clock.Year, s_dal.Config.Clock.Month, s_dal.Config.Clock.Day);
        for (int i = 0; i < 50; i++) 
        {
            string[] detailLocation = streetsNames[s_rand.Next(0, streetsNames.Length)];
            DateTime dateTime = startTime.AddHours(s_rand.Next((s_dal.Config.Clock - startTime).Hours));
            s_dal.Call!.Create(new(0, Enum.Parse<KindOfCall>(callDescriptionsMatrix[i][0]), detailLocation[0], double.Parse(detailLocation[1]), double.Parse(detailLocation[2]), dateTime, dateTime.AddHours(s_rand.Next(1, 4)),callDescriptionsMatrix[i][1]));
        }
    }
    /// <summary>
    /// intilize the array of assignments in the DataSource with 50 assignments
    /// </summary>
    private static void createAssigments()
    {
        IEnumerable<Volunteer> allVolunteer = s_dal.Volunteer.ReadAll();
        IEnumerable<Call> allCall = s_dal.Call.ReadAll();
        List<Volunteer> allVolunteerList = allVolunteer.ToList();
        List<Call> allCallList = allCall.ToList();
        for (int i = 0; i < 12; i++)//clasic assignments which were taken and comlited before the finish time of the call
        {
            s_dal.Assignment!.Create(new(0, allCallList[i].Id, allVolunteerList[s_rand.Next(0, allVolunteerList.Count - 3)].Id, allCallList[i].OpeningTime.AddMinutes(10), allCallList[i].FinishTime?.AddMinutes(-10), GetRandomEnumValue<TypeOfTreatmentTermination>(s_rand, 1)));
        }
        for (int i = 12; i < 24; i++)//assignments that started and didnt end yet
        {
            s_dal.Assignment!.Create(new(0, allCallList[i].Id, allVolunteerList[s_rand.Next(0, allVolunteerList.Count - 3)].Id, allCallList[i].OpeningTime.AddMinutes(10), null, null));
        }
        for (int i = 24; i < 50; i++)//assignments that started after the finish time of the call 
        {
            s_dal.Assignment!.Create(new(0, allCallList[i].Id, allVolunteerList[s_rand.Next(0, allVolunteerList.Count - 3)].Id, allCallList[i].OpeningTime.AddHours(1), allCallList[i].FinishTime?.AddMinutes(1), TypeOfTreatmentTermination.CancellationExpired));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dalVolunteer"></param>
    /// <param name="dalCall"></param>
    /// <param name="dalAssignment"></param>
    /// <param name="dalConfig"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    { 
        s_dal = DalApi.Factory.Get;
        Console.WriteLine("Reset Configuration values and List values...");
        s_dal.ResetDB();
        Console.WriteLine("Initializing Volunteer list ...");
        createVolunteer();
        Console.WriteLine("Initializing Call list ...");
        createCall();
        Console.WriteLine("Initializing Assigments list ...");
        createAssigments();
    }
}