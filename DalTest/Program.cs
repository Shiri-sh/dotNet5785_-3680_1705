
using Dal;
using DalApi;
using DO;

namespace DalTest;

internal class Program
{
    private static IVolunteer? s_dalVolunteer = new VolunteerImplementation();
    private static ICall? s_dalCall = new CallImplementation();
    private static IAssignment? s_dalAssignment = new AssignmetImplementation();
    private static IConfig? s_dalConfig = new ConfigImplementation();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        try {
            Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
            MainMenu();
        }
        catch (Exception errorMassage) {
            Console.WriteLine(errorMassage.Message);
        }
    }
    static void MainMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- תפריט ראשי ---");
            Console.WriteLine("0. יציאה");
            Console.WriteLine("1. הצגת רשימת מתנדבים ");
            Console.WriteLine("2. הצגת קריאות ");
            Console.WriteLine("3. הצגת טיפולים בקריאות ");
            Console.WriteLine("4. אתחול נתונים");
            Console.WriteLine("5. הצגת כל הנתונים בבסיס הנתונים");
            Console.WriteLine("6. הצגת תת-תפריט עבור ישות תצורה");
            Console.WriteLine("7. איפוס בסיס נתונים ואיפוס נתוני תצורה");
            Console.Write("בחר אפשרות: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    SubMenu("VolunteerVolunteer");
                    break;
                case "2":
                    SubMenu("Call");
                    break;
                case "3":
                    SubMenu("Assignment");
                    break;
                case "4":
                    Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
                    break;
                case "5":
                    DisplayAllData();
                    break;
                case "6":
                    ConfigurationSubMenu();
                    break;
                case "7":
                    ResetDatabase();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("בחירה לא תקפה. נסה שוב.");
                    break;
            }
        }
    }
    static void SubMenu(string entityName)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine($"\n--- תת-תפריט עבור {entityName} ---");
            Console.WriteLine("0. יציאה");
            Console.WriteLine("1. הוספת אובייקט חדש");
            Console.WriteLine("2. תצוגת אובייקט לפי מזהה");
            Console.WriteLine("3. תצוגת רשימת כל האובייקטים");
            Console.WriteLine("4. עדכון אובייקט קיים");
            Console.WriteLine("5. מחיקת אובייקט קיים");
            Console.WriteLine("6. מחיקת כל האובייקטים");
            Console.Write("בחר אפשרות: ");
            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddEntity(entityName);
                        break;
                    case "2":
                        ReadEntity(entityName);
                        break;
                    case "3":
                        ReadAllEntities(entityName);
                        break;
                    case "4":
                        UpdateEntity(entityName);
                        break;
                    case "5":
                        DeleteEntity(entityName);
                        break;
                    case "6":
                        DeleteAllEntities(entityName);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("בחירה לא תקפה. נסה שוב.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה: {ex.Message}");
            }
        }
    }
    static void ConfigurationSubMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- תת-תפריט תצורה ---");
            Console.WriteLine("0. יציאה");
            Console.WriteLine("1. קדם שעון מערכת בדקה");
            Console.WriteLine("2. קדם שעון מערכת בשעה");
            Console.WriteLine("3. הצג ערך נוכחי של שעון מערכת");
            Console.WriteLine("4. קבע ערך חדש למשתנה תצורה");
            Console.WriteLine("5. הצג ערך נוכחי למשתנה תצורה");
            Console.WriteLine("6. אפס ערכים עבור כל משתני תצורה");
            Console.Write("בחר אפשרות: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AdvanceClock(1);
                    break;
                case "2":
                    AdvanceClock(60);
                    break;
                case "3":
                    Console.WriteLine($"שעון מערכת נוכחי: {s_dalConfig.Clock}");
                    break;
                case "4":
                    SetConfigurationValue();
                    break;
                case "5":
                    DisplayConfigurationValue();
                    break;
                case "6":
                    s_dalConfig.Reset();
                    Console.WriteLine("כל משתני התצורה אופסו.");
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("בחירה לא תקפה. נסה שוב.");
                    break;
            }
        }
    }
    static void AddEntity(string entityName)
    {
        if (entityName == "Volunteer")
        {
            Console.Write("Enter Id: ");
            int? Id = int.TryParse(Console.ReadLine(), out int tempId) ? tempId : (int?)null;

            Console.Write("Enter Name: ");
            string? Name = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string? PhoneNumber = Console.ReadLine();

            Console.Write("Enter Email: ");
            string? Email = Console.ReadLine();

            Console.Write("Enter Position: ");
            Position? Position = Enum.TryParse<Position>(Console.ReadLine(), out var tempPos) ? tempPos : (Position?)null;

            Console.Write("Enter Password: ");
            string? Password = Console.ReadLine();

            Console.Write("Is Active (true/false): ");
            bool? active = bool.TryParse(Console.ReadLine(), out bool tempActive) ? tempActive : (bool?)null;

            Console.Write("Enter Current Address: ");
            string? CurrentAddress = Console.ReadLine();

            Console.Write("Enter Latitude: ");
            double? Latitude = double.TryParse(Console.ReadLine(), out double tempLat) ? tempLat : (double?)null;

            Console.Write("Enter Longitude: ");
            double? Longitude = double.TryParse(Console.ReadLine(), out double tempLong) ? tempLong : (double?)null;

            Console.Write("Enter Maximum Distance For Reading: ");
            double? MaximumDistanceForReading = double.TryParse(Console.ReadLine(), out double tempDist) ? tempDist : (double?)null;

            Console.Write("Enter Type Of Distance: ");
            TypeOfDistance? TOfDistance = Enum.TryParse<TypeOfDistance>(Console.ReadLine(), out var tempTOfDistance) ? tempTOfDistance : (TypeOfDistance?)null;

            Volunteer newVolunteer = new Volunteer(Id, Name, PhoneNumber, Email, Position, Password, active, CurrentAddress, Latitude, Longitude, MaximumDistanceForReading, TOfDistance);
            IVolunteer.Create(newVolunteer);
        }
        Console.WriteLine($"הוספת אובייקט חדש עבור {entityName}");
        // כתוב כאן את הלוגיקה להוספת אובייקט
    }
    static void ReadEntity(string entityName)
    {
        try { 
            Console.WriteLine("הכנס מספר זיהוי של {entityName}");
            int id = int.TryParse(Console.ReadLine(), out int tempId) ? tempId : (int?)null;
            Type entityType = Type.GetType(fullClassName);
            // יצירת מופע של המחלקה
            object entityInstance = Activator.CreateInstance(entityType);
            if (entityInstance == null)
            {
                Console.WriteLine($"לא ניתן ליצור מופע של המחלקה {entityName}");
                return;
            }
            // הפעלת המתודה עם הפרמטר id
            object result = readMethod.Invoke(entityInstance, new object[] { id });
            // הפעלת המתודה Read
            var readMethod = entityType.GetMethod("Read");
            Console.WriteLine($"תצוגת אובייקט לפי מזהה עבור {entityName}");
            // כתוב כאן את הלוגיקה לתצוגת אובייקט
        }
        catch (Exception ex)
        {
            Console.WriteLine($"שגיאה בעת קריאה למחלקה {entityName}: {ex.Message}");
        }
    }
    static void ReadAllEntities(string entityName)
    {
        Console.WriteLine($"תצוגת כל האובייקטים עבור {entityName}");
        // כתוב כאן את הלוגיקה להצגת כל האובייקטים
    }
    static void UpdateEntity(string entityName)
    {
        Console.WriteLine($"עדכון אובייקט עבור {entityName}");
        // כתוב כאן את הלוגיקה לעדכון אובייקט
    }
    static void DeleteEntity(string entityName)
    {
        Console.WriteLine($"מחיקת אובייקט עבור {entityName}");
        // כתוב כאן את הלוגיקה למחיקת אובייקט
    }
    static void DeleteAllEntities(string entityName)
    {
        Console.WriteLine($"מחיקת כל האובייקטים עבור {entityName}");
        // כתוב כאן את הלוגיקה למחיקת כל האובייקטים
    }
    static void AdvanceClock(int minutes)
    {
        Console.WriteLine($"שעון המערכת התקדם ב-{minutes} דקות.");
        // כתוב כאן את הלוגיקה להתקדמות השעון
    }
    static void SetConfigurationValue()
    {
        Console.WriteLine("קביעת ערך חדש למשתנה תצורה.");
        // כתוב כאן את הלוגיקה לקביעת ערך
    }
    static void DisplayConfigurationValue()
    {
        Console.WriteLine("הצגת ערך נוכחי למשתנה תצורה.");
        // כתוב כאן את הלוגיקה להצגת ערך
    }
    static void ResetDatabase()
    {
        Console.WriteLine("בסיס הנתונים אופס.");
        // כתוב כאן את הלוגיקה לאיפוס בסיס הנתונים
    }
    static void DisplayAllData()
    {
        Console.WriteLine("מציג את כל הנתונים בבסיס הנתונים:");
        // כתוב כאן את הלוגיקה להצגת כל הנתונים
    }
}


