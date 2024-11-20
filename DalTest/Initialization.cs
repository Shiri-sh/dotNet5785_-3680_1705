namespace DalTest;

using DalApi;
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
    private static int MIN_ID=200000000;
    public static int MAX_ID = 400000000;
    public static int MIN_P = 10000000;
    public static int MAX_P = 99999999;
    public static int MIN_S = 100000;
    public static int MAX_S = 999999;
    //...
    private static void createVolunteer()
    {
        string[] volunteerNames ={ "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof" };
        foreach (string volunteerName in volunteerNames)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(id) != null);
            string phoneNumber = "05"+Convert.ToString(s_rand.Next(MIN_P, MAX_P));
            string password = Convert.ToString(s_rand.Next(MIN_S,MAX_S))+ (char)s_rand.Next('A', 'Z' + 1)+(char)s_rand.Next(32, 127);;
            string passwordHidden="";
            foreach(char var in password)
            {
                passwordHidden += var + (char)s_rand.Next('A', 'Z' + 1);
            }
           s_dalVolunteer!.Create(new(id, volunteerName, phoneNumber, "",Position.Volunteer,passwordHidden));
        }
    }
    private static void createCall()
    {
        DateTime start = new DateTime(s_dalConfig.Clock.Year - 3, 1, 1);
        s_dalCall!.Create(new(0, KindOfCall.CableAssistance, "אבן גבירול 11, אלעד, ישראל", 32.0579, 34, start ));
        start = start.AddHours(s_rand.Next(1, 13));
        s_dalCall!.Create(new(0, KindOfCall.fuelOilWater,"רבי חיא,אלעד,ישראל", 32.049344, 34.963798, start));
        start = start.AddHours(s_rand.Next(1, 13));
        s_dalCall!.Create(new(0, KindOfCall.changeWheel,"דרך מנחם בגין, פתח תקווה, ישראל", 32.069869, 34.914232, start));
        start = start.AddHours(s_rand.Next(1, 13));
        s_dalCall!.Create(new(0, KindOfCall.CableAssistance,"פנחס רוזן 12, תל אביב-יפו, ישראל", 32.108024, 34.827305, start,null,"נגמר לרכב חשמלי הבטריה"));
        start = start.AddHours(s_rand.Next(1, 13));
        s_dalCall!.Create(new(0, KindOfCall.FirstAid,"באר מים חיים 12, בני ברק, ישראל", 32.083079, 34.841832, start, start.AddHours(1),"ילד בן שנתיים נשכח ברכב"));
    }
    private static void createAssigments()
    {

    }

}
