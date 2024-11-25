namespace DalTest;

using DalApi;
using DalList;
using Dal;
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
   
    static T GetRandomEnumValue<T>(Random random,int range=0) where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length-range));
    }
    /// <summary>
    /// 
    /// </summary>
    private static void createVolunteer()
    {
        int counter = 0;
        s_dalVolunteer!.Create(new(327691758, "kobi dinavetsky", " mimi20054@gmail.com ", "0583235695", Position.Managar, "123456!A"));
        string[] volunteerNames = { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof","Shir Levi" ,"Ron Catz", "Rut Levi","Ayala Cohen","Shiri Shahor","Enana Fedut","Tammy Dabin","Sari Levin","Michal Ortel"};
        string[] volunteerEmail = {" mimigins2005@gmail.com "," frgvvv@gmail.com "," rutrc@gmail.com "," avigail6282@gmail.com "," avitalbortz@gmail.com "," ms0583242931@gmail.com "," miri57566@gmail.com ","noya.boruhov@gmail.com "," naama0052@gmail.com ","zipi.3382@gmail.com "," shirishahor@gmail.com "," t0583235695@gmail.com"," tamar86950@gmail.com "," michalortzel@gmail.com "," ys0583299134@gmail.com " };
        foreach (string volunteerName in volunteerNames)
        {
            int id;
            do
                id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalVolunteer!.Read(id) != null);
            string phoneNumber = "05" + Convert.ToString(s_rand.Next(MIN_P, MAX_P));
            string password = Convert.ToString(s_rand.Next(MIN_S, MAX_S)) + (char)s_rand.Next('A', 'Z' + 1) + (char)s_rand.Next(32, 127);
            string passwordHidden = "";
            foreach (char var in password)
            {
                passwordHidden += var + (char)s_rand.Next('A', 'Z' + 1);
            }
            s_dalVolunteer!.Create(new(id, volunteerName, phoneNumber, volunteerEmail[counter], Position.Volunteer, passwordHidden, MaximumDistanceForReading:s_rand.Next(1,2500)));
            counter++;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private static void createCall()
    {
        string[][] streetsNames = { ["Even Gvirol 11, Elad", "32.0579", "34"], ["Rabbi Hiyya, Elad", "32.049344", "34.963798"], ["Derech Menachem Begin, Petah Tikva", "32.069869", "34.914232"], ["Pinhas Rozen 12, Tel Aviv-Yafo", "32.108024", "34.827305"],[ "Be'er Mayim Chaim 12, Bnei Brak", "32.083079", "34.841832"], ["Herzl Street 25, Tel Aviv", "32.060780", "34.770589"],[ "Jabotinsky 15, Ramat Gan", "32.073667", "34.809965"],[ "Allenby Street, Tel Aviv", "32.067519", "34.771222"],[ "King George Street, Tel Aviv", "32.073428", "34.775037"] };
        DateTime startTime = new DateTime(s_dalConfig.Clock.Year, s_dalConfig.Clock.Month, s_dalConfig.Clock.Day-1);
        for (int i = 0; i < 50; i++)
        {
            string[] detailLocation=streetsNames[s_rand.Next(0,streetsNames.Length)];
            DateTime dateTime = startTime.AddHours(s_rand.Next((s_dalConfig.Clock - startTime).Hours));
            s_dalCall!.Create(new(0, GetRandomEnumValue<KindOfCall>(s_rand), detailLocation[0], double.Parse(detailLocation[1]), double.Parse(detailLocation[2]), dateTime,dateTime.AddHours(s_rand.Next(1,4))));
        }
    }
    /// <summary>
    /// /
    /// </summary>
    private static void createAssigments()
    {
        List<Volunteer> allVolunteer = s_dalVolunteer.ReadAll();
        List<Call> allCall = s_dalCall.ReadAll();

        DateTime start = new DateTime(s_dalConfig.Clock.Year - 3, 1, 1);
        for (int i=0; i<12;i++)
        {
             s_dalAssignment!.Create(new(0, allCall[i].Id, allVolunteer[s_rand.Next(0, allVolunteer.Count - 3)].Id, allCall[i].OpeningTime.AddMinutes(10), allCall[i].FinishTime, GetRandomEnumValue < TypeOfTreatmentTermination >(s_rand,1)));
        }
        for (int i = 12; i < 24; i++)
        {
            s_dalAssignment!.Create(new(0, allCall[i].Id, allVolunteer[s_rand.Next(0, allVolunteer.Count - 3)].Id, allCall[i].OpeningTime.AddMinutes(10), null, null));
        }
        for (int i = 24; i <50; i++)
        {

            s_dalAssignment!.Create(new(0, allCall[i].Id, allVolunteer[s_rand.Next(0, allVolunteer.Count - 3)].Id, allCall[i].OpeningTime.AddHours(5), null, TypeOfTreatmentTermination.CancellationExpired));
        }
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
