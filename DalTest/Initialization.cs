namespace DalTest;

using DalApi;
using DalList;
using Dal;
using DO;
using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        s_dalVolunteer!.Create(new(327691758, "kobi dinavetsky", " mimi20054@gmail.com ", "0583235695", Position.Managar, "123456!A"));
        string[] volunteerNames = { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Shir Levi", "Ron Catz", "Rut Levi", "Ayala Cohen", "Shiri Shahor", "Enana Fedut", "Tammy Dabin", "Sari Levin", "Michal Ortel" };
        string[] volunteerEmail = { " mimigins2005@gmail.com ", " frgvvv@gmail.com ", " rutrc@gmail.com ", " avigail6282@gmail.com ", " avitalbortz@gmail.com ", " ms0583242931@gmail.com ", " miri57566@gmail.com ", "noya.boruhov@gmail.com ", " naama0052@gmail.com ", "zipi.3382@gmail.com ", " shirishahor@gmail.com ", " t0583235695@gmail.com", " tamar86950@gmail.com ", " michalortzel@gmail.com ", " ys0583299134@gmail.com " };
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
            s_dalVolunteer!.Create(new(id, volunteerName, phoneNumber, volunteerEmail[counter], Position.Volunteer, passwordHidden, MaximumDistanceForReading: s_rand.Next(1, 2500), TypeOfDistance: TypeOfDistance.Aerial));
            counter++;
        }
    }
    /// <summary>
    /// intilize the array of calls in the DataSource with 50 calls
    /// </summary>
    private static void createCall()
    {
        string[][] callDescriptionsMatrix =
        {
             [ "RescueKid", "חילוץ ילד נעול ברכב" ],
             [ "RescueKid", "קריאה לחילוץ ילד במצב חירום" ],
             [ "RescueKid", "סיוע בהוצאת ילד תקוע" ],
             [ "changeWheel", "החלפת גלגל שנפגע" ],
             [ "changeWheel", "סיוע בהחלפת צמיג תקול" ],
             [ "changeWheel", "עזרה בהתקנת גלגל חדש" ],
             [ "FirstAid", "מתן עזרה ראשונה לנפגע" ],
             [ "FirstAid", "טיפול חירום במצב רפואי" ],
             [ "FirstAid", "עזרה ראשונה לפני הגעת מד" ],
             [ "CableAssistance", "התנעת רכב בעזרת כבלים" ],
             [ "CableAssistance", "סיוע ברכב שאינו מניע" ],
             [ "CableAssistance", "הפעלת רכב בעזרת כבלים" ],
             [ "fuelOilWater", "מילוי דלק לרכב תקוע" ],
             [ "fuelOilWater", "הוספת שמן לרכב במחסור" ],
             [ "fuelOilWater", "תדלוק רכב במקום חירום" ],
             [ "RescueKid", "חילוץ ילד במצבים מיוחדים" ],
             [ "RescueKid", "קריאה דחופה להצלת ילד" ],
             [ "changeWheel", "שירות להחלפת גלגל" ],
             [ "changeWheel", "טיפול בגלגל לא תקין" ],
             [ "FirstAid", "סיוע רפואי ראשוני" ],
             [ "FirstAid", "עזרה מידית לנפגעים" ],
             [ "CableAssistance", "התנעת רכב ללא סוללה" ],
             [ "CableAssistance", "תיקון זמני למערכת החשמל ברכב" ],
             [ "fuelOilWater", "תדלוק בשטח ללא תחנת דלק" ],
             [ "fuelOilWater", "אספקת מים לרכב רותח" ],
             [ "fuelOilWater", "הוספת נוזלים חיוניים לרכב" ],
             [ "RescueKid", "עזרה במקרה של ילד נעול" ],
             [ "changeWheel", "תיקון צמיג ברכב תקוע" ],
             [ "FirstAid", "טיפול במצבי פציעה דחופה" ],
             [ "CableAssistance", "סיוע טכני להפעלת רכב" ],
             [ "fuelOilWater", "מילוי שמן לצורך חירום" ],
             [ "RescueKid", "פעולה להצלת ילד" ],
             [ "changeWheel", "החלפה מקצועית של גלגל" ],
             [ "FirstAid", "טיפול חירום במקום" ],
             [ "CableAssistance", "שירות התנעה עם כבלים" ],
             [ "fuelOilWater", "הוספת דלק במצבים חריגים" ],
             [ "RescueKid", "חילוץ ילדים בנסיבות מורכבות" ],
             [ "changeWheel", "שירות חירום לצמיגים" ],
             [ "FirstAid", "טיפול ראשוני עד להגעה לטיפול רפואי" ],
             [ "CableAssistance", "עזרה ברכב עם בעיות סוללה" ],
             [ "fuelOilWater", "מילוי מים למנוע שהתחמם" ],
             [ "RescueKid", "סיוע במצבי חירום לילדים" ],
             [ "changeWheel", "החלפה מהירה של גלגל תקול" ],
             [ "FirstAid", "עזרה רפואית מידית בזירה" ],
             [ "CableAssistance", "שירות התנעה לרכב דומם" ],
             [ "fuelOilWater", "השלמת נוזלים למנוע" ],
             [ "fuelOilWater", "מילוי מים למנוע שהתחמם" ],
             [ "RescueKid", "סיוע במצבי חירום לילדים" ],
             [ "changeWheel", "החלפה מהירה של גלגל תקול" ],
             [ "FirstAid", "עזרה רפואית מידית בזירה" ],
             [ "CableAssistance", "שירות התנעה לרכב דומם" ],
             [ "fuelOilWater", "השלמת נוזלים למנוע" ]
        };
        string[][] streetsNames = { ["Even Gvirol 11, Elad", "32.0579", "34"], ["Rabbi Hiyya, Elad", "32.049344", "34.963798"], ["Derech Menachem Begin, Petah Tikva", "32.069869", "34.914232"], ["Pinhas Rozen 12, Tel Aviv-Yafo", "32.108024", "34.827305"], ["Be'er Mayim Chaim 12, Bnei Brak", "32.083079", "34.841832"], ["Herzl Street 25, Tel Aviv", "32.060780", "34.770589"], ["Jabotinsky 15, Ramat Gan", "32.073667", "34.809965"], ["Allenby Street, Tel Aviv", "32.067519", "34.771222"], ["King George Street, Tel Aviv", "32.073428", "34.775037"], ["Dizengoff Street 120, Tel Aviv", "32.083687", "34.773541"], ["Ehad Ha'am Street 9, Tel Aviv", "32.065789", "34.776231"], ["Rothschild Boulevard, Tel Aviv", "32.063229", "34.774996"], ["Balfour Street 20, Bat Yam", "32.027325", "34.747369"], ["Shenkar Street 7, Herzliya", "32.164458", "34.836211"], ["Nordau Boulevard, Tel Aviv", "32.091236", "34.785009"], ["Ben Yehuda Street, Tel Aviv", "32.082246", "34.769322"], ["Hashalom Road, Givatayim", "32.069478", "34.808196"], ["Arlozorov Street 78, Tel Aviv", "32.085679", "34.782549"], ["HaYarkon Street 123, Tel Aviv", "32.089678", "34.769126"], ["King George Street, Tel Aviv", "32.073428", "34.775037"], ["Allenby Street, Tel Aviv", "32.067519", "34.771222"] };
        DateTime startTime = new DateTime(s_dalConfig.Clock.Year, s_dalConfig.Clock.Month, s_dalConfig.Clock.Day - 1);
        for (int i = 0; i < 50; i++)
        {
            string[] detailLocation = streetsNames[s_rand.Next(0, streetsNames.Length)];
            DateTime dateTime = startTime.AddHours(s_rand.Next((s_dalConfig.Clock - startTime).Hours));
            s_dalCall!.Create(new(0, Enum.Parse<KindOfCall>(callDescriptionsMatrix[i][0]), detailLocation[0], double.Parse(detailLocation[1]), double.Parse(detailLocation[2]), dateTime, dateTime.AddHours(s_rand.Next(1, 4)),callDescriptionsMatrix[i][1]));
        }
    }
    /// <summary>
    /// intilize the array of assignments in the DataSource with 50 assignments
    /// </summary>
    private static void createAssigments()
    {
        List<Volunteer> allVolunteer = s_dalVolunteer.ReadAll();
        List<Call> allCall = s_dalCall.ReadAll();

        for (int i = 0; i < 12; i++)//clasic assignments which were taken and comlited before the finish time of the call
        {
            s_dalAssignment!.Create(new(0, allCall[i].Id, allVolunteer[s_rand.Next(0, allVolunteer.Count - 3)].Id, allCall[i].OpeningTime.AddMinutes(10), allCall[i].FinishTime, GetRandomEnumValue<TypeOfTreatmentTermination>(s_rand, 1)));
        }
        for (int i = 12; i < 24; i++)//assignments that started and didnt end yet
        {
            s_dalAssignment!.Create(new(0, allCall[i].Id, allVolunteer[s_rand.Next(0, allVolunteer.Count - 3)].Id, allCall[i].OpeningTime.AddMinutes(10), null, null));
        }
        for (int i = 24; i < 50; i++)//assignments that started after the finish time of the call 
        {
            s_dalAssignment!.Create(new(0, allCall[i].Id, allVolunteer[s_rand.Next(0, allVolunteer.Count - 3)].Id, allCall[i].OpeningTime.AddHours(5), null, TypeOfTreatmentTermination.CancellationExpired));
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