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
        try
        {
            Initialization.Do(s_dalVolunteer, s_dalCall, s_dalAssignment, s_dalConfig);
            MainMenu();
        }
        catch (Exception errorMassage)
        {
            Console.WriteLine(errorMassage.Message);
        }
    }
    static void MainMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Display list of volunteers");
            Console.WriteLine("2. Display calls");
            Console.WriteLine("3. Display call treatments");
            Console.WriteLine("4. Initialize data");
            Console.WriteLine("5. Display all data in the database");
            Console.WriteLine("6. Display sub-menu for configuration entity");
            Console.WriteLine("7. Reset database and configuration data");
            Console.Write("Choose an option: ");
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
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }
    static void SubMenu(string entityName)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine($"\n--- Sub-menu for {entityName} ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Add a new object");
            Console.WriteLine("2. Display an object by ID");
            Console.WriteLine("3. Display the list of all objects");
            Console.WriteLine("4. Update an existing object");
            Console.WriteLine("5. Delete an existing object");
            Console.WriteLine("6. Delete all objects");
            Console.Write("Choose an option: ");

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
                        Console.WriteLine("Invalid selection. Please try again.");
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
            Console.WriteLine("\n--- Configuration Sub-menu ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Advance system clock by a minute");
            Console.WriteLine("2. Advance system clock by an hour");
            Console.WriteLine("3. Display current value of system clock");
            Console.WriteLine("4. Set a new value for a configuration variable");
            Console.WriteLine("5. Display the current value of a configuration variable");
            Console.WriteLine("6. Reset values for all configuration variables");
            Console.Write("Choose an option: ");
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
                    Console.WriteLine($"Current system clock: {s_dalConfig.Clock}");
                    break;
                case "4":
                    SetConfigurationValue();
                    break;
                case "5":
                    DisplayConfigurationValue();
                    break;
                case "6":
                    s_dalConfig.Reset();
                    Console.WriteLine("All configuration variables have been reset.");
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }
    static void AddEntity(string entityName)
    {
        if (entityName == "Volunteer")
        {
            s_dalVolunteer.Create(CreateNewVolunteer());
        }
        else if (entityName == "Assignment")
        {
            s_dalAssignment.Create(CreateNewAssigment());
        }
        else
        {
            s_dalCall.Create(CreateNewCall());
        }
        Console.WriteLine($"new object has been added to {entityName}s");
 
    }
    static Volunteer CreateNewVolunteer()
    {
        Console.Write("Enter Id: ");
        int Id = int.Parse(Console.ReadLine());
        Console.Write("Enter Name: ");
        string Name = Console.ReadLine();
        Console.Write("Enter Phone Number: ");
        string PhoneNumber = Console.ReadLine();
        Console.Write("Enter Email: ");
        string Email = Console.ReadLine();
        Console.Write("Enter Position : ");
        Position Position = Enum.Parse<Position>(Console.ReadLine());
        Console.Write("Enter Password: ");
        string Password = Console.ReadLine();
        Console.Write("Is Active (true/false): ");
        bool active = bool.Parse(Console.ReadLine());
        Console.Write("Enter Current Address: ");
        string CurrentAddress = Console.ReadLine();
        Console.Write("Enter Latitude: ");
        double Latitude = double.Parse(Console.ReadLine());
        Console.Write("Enter Longitude: ");
        double Longitude = double.Parse(Console.ReadLine());
        Console.Write("Enter Maximum Distance For Reading: ");
        double MaximumDistanceForReading = double.Parse(Console.ReadLine());
        Console.Write("Enter Type Of Distance: ");
        TypeOfDistance TOfDistance = Enum.Parse<TypeOfDistance>(Console.ReadLine());
        Volunteer newVolunteer = new Volunteer(Id, Name, PhoneNumber, Email, 0, Password, active, CurrentAddress, Latitude, Longitude, MaximumDistanceForReading, TOfDistance);
        return newVolunteer;
    }
    static Assignment CreateNewAssigment()
    {
        Console.WriteLine("Enter Assignment details:");
        Console.Write("Enter Id: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Enter CalledId: ");
        int calledId = int.Parse(Console.ReadLine());
        Console.Write("Enter VolunteerId: ");
        int volunteerId = int.Parse(Console.ReadLine());
        Console.Write("Enter TreatmentEntryTime (yyyy-MM-dd HH:mm): ");
        DateTime treatmentEntryTime = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter TreatmentEndTime (yyyy-MM-dd HH:mm): ");
        DateTime treatmentEndTime = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter TypeOfTreatmentTermination (Handled, SelfCancellation, ConcellingAdministrator, CancellationExpired): ");
        TypeOfTreatmentTermination TypeOfTreatmentTermination = Enum.Parse<TypeOfTreatmentTermination>(Console.ReadLine());
        Assignment newAssignment = new(id, calledId, volunteerId, treatmentEntryTime, treatmentEndTime, TypeOfTreatmentTermination);

        return newAssignment;
    }
    static Call CreateNewCall()
    {
        // Get user input for Call object
        Console.WriteLine("Enter Call details:");
        Console.Write("Enter Id: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Enter KindOfCall (Emergency, NonUrgent, FollowUp, Other): ");
        KindOfCall kindOfCall = Enum.Parse<KindOfCall>(Console.ReadLine());
        Console.Write("Enter AddressOfCall: ");
        string addressOfCall = Console.ReadLine();
        Console.Write("Enter Latitude: ");
        double latitude = double.Parse(Console.ReadLine());
        Console.Write("Enter Longitude: ");
        double longitude = double.Parse(Console.ReadLine());
        Console.Write("Enter OpeningTime (yyyy-MM-dd HH:mm): ");
        DateTime openingTime = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter FinishTime (yyyy-MM-dd HH:mm): ");
        DateTime FinishTime = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter Description: ");
        string Description = Console.ReadLine();
        Call newCall = new(id, kindOfCall, addressOfCall, latitude, longitude, openingTime, FinishTime, Description);
        return newCall;
    }
    static void ReadEntity(string entityName)
    {
        try
        {
            Console.WriteLine($"הכנס מספר זיהוי של {entityName}");
            int id = int.Parse(Console.ReadLine());
            Type entityType = Type.GetType(entityName);
            // יצירת מופע של המחלקה
            object entityInstance = Activator.CreateInstance(entityType);
            if (entityInstance == null)
            {
                Console.WriteLine($"לא ניתן ליצור מופע של המחלקה {entityName}");
                return;
            }
            // הפעלת המתודה עם הפרמטר id
           // object result = readMethod.Invoke(entityInstance, new object[] { id });
            // הפעלת המתודה Read
            var readMethod = entityType.GetMethod("Read");
            Console.WriteLine($"תצוגת אובייקט לפי מזהה עבור {entityName}");
            // . כאן את הלוגיקה לתצוגת אובייקט
        }
        catch (Exception ex)
        {
            Console.WriteLine($"שגיאה בעת קריאה למחלקה {entityName}: {ex.Message}");
        }
    }
    static void ReadAllEntities(string entityName)
    {
        Console.WriteLine($"תצוגת כל האובייקטים עבור {entityName}");
        // . כאן את הלוגיקה להצגת כל האובייקטים
    }
    static void UpdateEntity(string entityName)
    {
        Console.WriteLine($"עדכון אובייקט עבור {entityName}");
        // . כאן את הלוגיקה לעדכון אובייקט
    }
    static void DeleteEntity(string entityName)
    {
        Console.WriteLine($"מחיקת אובייקט עבור {entityName}");
        // . כאן את הלוגיקה למחיקת אובייקט
    }
    static void DeleteAllEntities(string entityName)
    {
        Console.WriteLine($"מחיקת כל האובייקטים עבור {entityName}");
        // . כאן את הלוגיקה למחיקת כל האובייקטים
    }
    static void AdvanceClock(int minutes)
    {
        Console.WriteLine($"שעון המערכת התקדם ב-{minutes} דקות.");
        // . כאן את הלוגיקה להתקדמות השעון
    }
    static void SetConfigurationValue()
    {
        Console.WriteLine("קביעת ערך חדש למשתנה תצורה.");
        // . כאן את הלוגיקה לקביעת ערך
    }
    static void DisplayConfigurationValue()
    {
        Console.WriteLine("הצגת ערך נוכחי למשתנה תצורה.");
        // . כאן את הלוגיקה להצגת ערך
    }
    static void ResetDatabase()
    {
        Console.WriteLine("בסיס הנתונים אופס.");
        // . כאן את הלוגיקה לאיפוס בסיס הנתונים
    }
    static void DisplayAllData()
    {
        Console.WriteLine("מציג את כל הנתונים בבסיס הנתונים:");
        // . כאן את הלוגיקה להצגת כל הנתונים
    }
}


