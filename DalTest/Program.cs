using Dal;
using DalApi;

namespace DalTest
{
    internal class Program
    {
        private static IVolunteer? s_dalVolunteer = new VolunteerImplementation();
        private static ICall? s_dalCall = new CallImplementation();
        private static IAssignment? s_dalAssignment = new AssignmetImplementation();
        private static IConfig? s_dalConfig = new ConfigImplementation();

        static void Main(string[] args)
        {
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
                        Console.WriteLine($"שעון מערכת נוכחי: {s_dal.Config.Clock}");
                        break;
                    case "4":
                        SetConfigurationValue();
                        break;
                    case "5":
                        DisplayConfigurationValue();
                        break;
                    case "6":
                        s_dal.Config.Reset();
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
            Console.WriteLine($"הוספת אובייקט חדש עבור {entityName}");
            // כתוב כאן את הלוגיקה להוספת אובייקט
        }
        static void ReadEntity(string entityName)
        {
            Console.WriteLine($"תצוגת אובייקט לפי מזהה עבור {entityName}");
            // כתוב כאן את הלוגיקה לתצוגת אובייקט
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
    static class s_dal
    {
        public static ConfigClass Config { get; } = new ConfigClass();
    }
    class ConfigClass
    {
        public DateTime Clock { get; set; } = DateTime.Now;
        public void Reset()
        {
            Clock = DateTime.Now;
            Console.WriteLine("כל משתני התצורה אופסו.");
        }
    }

}


