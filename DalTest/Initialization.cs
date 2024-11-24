namespace DalTest;

using DalApi;
using DalList;

using DO;
using System;
using System.Xml.Linq;

public static class Initialization
{

    private static IAssignment? s_dalAssignment;
    private static ICall? s_dalCall;
    private static IVolunteer? s_dalVolunteer;
    private static IConfig? s_dalConfig;
    private static readonly Random s_rand = new();
    private static int MIN_ID = 200000000;
    public static int MAX_ID = 400000000;
    public static int MIN_P = 10000000;
    public static int MAX_P = 99999999;
    public static int MIN_S = 100000;
    public static int MAX_S = 999999;
    static T GetRandomEnumValue<T>(Random random) where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
    private static void createVolunteer()
    {
        s_dalVolunteer!.Create(new(327691758, "kobi dinavetsky", "0583235695", "", Position.Volunteer, "123456!A"));
        string[] volunteerNames = { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof","shir levi" };
        foreach (string volunteerName in volunteerNames)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(id) != null);
            string phoneNumber = "05" + Convert.ToString(s_rand.Next(MIN_P, MAX_P));
            string password = Convert.ToString(s_rand.Next(MIN_S, MAX_S)) + (char)s_rand.Next('A', 'Z' + 1) + (char)s_rand.Next(32, 127); ;
            string passwordHidden = "";
            foreach (char var in password)
            {
                passwordHidden += var + (char)s_rand.Next('A', 'Z' + 1);
            }
            s_dalVolunteer!.Create(new(id, volunteerName, phoneNumber, "", Position.Volunteer, passwordHidden));
        }
    }
    private static void createCall()
    {
        string[] streetsNames = { "Even Gvirol 11, Elad", "Rabbi Hiyya, Elad", "Derech Menachem Begin, Petah Tikva", "Pinhas Rozen 12, Tel Aviv-Yafo", "Be'er Mayim Chaim 12, Bnei Brak", "Herzl Street 25, Tel Aviv", "Jabotinsky 15, Ramat Gan", "Allenby Street, Tel Aviv", "Hanevi'im Street, Jerusalem", "King George Street, Haifa", };
        DateTime start = new DateTime(s_dalConfig.Clock.Year - 3, 1, 1);
        for (int i = 0; i < 50; i++)
        {
            s_dalCall!.Create(new(Config.startCallId, GetRandomEnumValue<KindOfCall>(s_rand), streetsNames[s_rand.Next(streetsNames.Length)], 30 + (s_rand.NextDouble() * (40 - 30)), 30 + (s_rand.NextDouble() * (40 - 30)), start.AddHours(s_rand.Next(1, 5))));
        }
    }
    private static void createAssigments()
    {
        DateTime start = new DateTime(s_dalConfig.Clock.Year - 3, 1, 1);
        s_dalAssignment!.Create(new(0, 1, 327691758, start.AddHours(s_rand.Next(8, 16)), start.AddHours(s_rand.Next(17, 23)), TypeOfTreatmentTermination.Handled));
        s_dalAssignment!.Create(new(0, 3, 327691758, start.AddHours(s_rand.Next(8, 16)), start.AddHours(s_rand.Next(17, 23)), TypeOfTreatmentTermination.CancellationExpired));
        s_dalAssignment!.Create(new(0, 2, 327691758, start.AddHours(s_rand.Next(8, 16)), start.AddHours(s_rand.Next(17, 23)), TypeOfTreatmentTermination.ConcellingAdministrator));
        s_dalAssignment!.Create(new(0, 2, 327691758, start.AddHours(s_rand.Next(8, 16)), start.AddHours(s_rand.Next(17, 23)), TypeOfTreatmentTermination.ConcellingAdministrator));
        s_dalAssignment!.Create(new(0, 2, 327691758, start.AddHours(s_rand.Next(8, 16)), start.AddHours(s_rand.Next(17, 23)), TypeOfTreatmentTermination.Handled));

    }
    public static void Do(IVolunteer? dalVolunteer, ICall? dalCall, IAssignment? dalAssignment, IConfig? dalConfig)
    {
        s_dalVolunteer = dalVolunteer ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalCall = dalCall ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalAssignment = dalAssignment ?? throw new NullReferenceException("DAL object can not be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object can not be null!");

        Console.WriteLine("Reset Configuration values and List values...");
        s_dalConfig.Reset();
        s_dalVolunteer.DeleteAll();
        s_dalCall.DeleteAll();
        s_dalAssignment.DeleteAll();

        Console.WriteLine("Initializing Volunteer list ...");
        createVolunteer();
        Console.WriteLine("Initializing Call list ...");
        createCall();
        Console.WriteLine("Initializing Assigments list ...");
        createAssigments();
    }
}
