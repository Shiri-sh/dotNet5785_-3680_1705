using Accessories;
using BO;
using DalApi;
using DO;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Display sub-Menu for Ivolunteers");
            Console.WriteLine("2. Display sub-Menu for Icalls");
            Console.WriteLine("3. Display sub-Menu for IAdmin");
            Console.Write("Choose an option: ");
            MainMenuEnum choice = ReadHelper.ReadEnum<MainMenuEnum>();
            switch (choice)
            {
                case MainMenuEnum.SubMenuVolunteer:
                    SubMenuVolunteer();
                    break;
                case MainMenuEnum.SubMenuCall:
                    SubMenuCall();
                    break;
                case MainMenuEnum.SubMenuAdmin:
                    SubMenuAdmin();
                    break;
                case MainMenuEnum.Exit:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }
    /// <summary>
    /// options to choose  for Volunteer
    /// </summary>
    static void SubMenuVolunteer()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine($"\n--- Sub-menu for Volunteer ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Add a new object");
            Console.WriteLine("2. Display an object by ID");
            Console.WriteLine("3. Display the list of all objects");
            Console.WriteLine("4. Update an existing object");
            Console.WriteLine("5. Delete an existing object");
            Console.WriteLine("6. Delete all objects");
            Console.Write("Choose an option: ");

            SubMenuVolunteerEnum choice = ReadHelper.ReadEnum<SubMenuVolunteerEnum>();
            try
            {
                switch (choice)
                {
                    case SubMenuVolunteerEnum.AddNew:
                        AddNewVolunteer();
                        break;
                    case SubMenuVolunteerEnum.DisplayById:
                        DisplayByIdVolunteer();
                        break;
                    case SubMenuVolunteerEnum.DisplayAll:
                        DisplayAllVolunteer();
                        break;
                    case SubMenuVolunteerEnum.Update:
                        UpdateVolunteer();
                        break;
                    case SubMenuVolunteerEnum.Delete:
                        DeleteVolunteer();
                        break;
                    case SubMenuVolunteerEnum.LoginSystem:
                        LoginSystem();
                        break;
                    case SubMenuVolunteerEnum.Exit:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
    }
    /// <summary>
    /// options to choose  for Call
    /// </summary>
    static void SubMenuCall()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine($"\n--- Sub-menu for Call ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Add a new object");
            Console.WriteLine("2. Display an object by ID");
            Console.WriteLine("3. Display the list of all objects");
            Console.WriteLine("4. Update an existing object");
            Console.WriteLine("5. Delete an existing object");
            Console.WriteLine("6. Delete all objects");
            Console.Write("Choose an option: ");

            BO.SubMenuCallEnum choice = ReadHelper.ReadEnum<BO.SubMenuCallEnum>();
            try
            {
                switch (choice)
                {
                    case BO.SubMenuCallEnum.Add:
                        AddCall();
                        break;
                    case BO.SubMenuCallEnum.CallByStatus:
                        CallByStatus();
                        break;
                    case BO.SubMenuCallEnum.Cancel:
                        CancelCall();
                        break;
                    case BO.SubMenuCallEnum.Delete:
                        DeleteCall();
                        break;
                    case BO.SubMenuCallEnum.DisplayAll:
                        DisplayAll();
                        break;
                    case BO.SubMenuCallEnum.DisplayById:
                        DisplayById();
                        break;
                    case BO.SubMenuCallEnum.UpdateEndCall:
                        UpdateEndCall();
                        break;
                    case BO.SubMenuCallEnum.UpdateCall:
                        UpdateCall();
                        break;
                    case BO.SubMenuCallEnum.GetCall:
                        GetCall();
                        break;
                    case BO.SubMenuCallEnum.GetAllCallByVolunteer:
                        GetAllCallByVolunteer();
                        break;
                    case BO.SubMenuCallEnum.Exit:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
    }
   
    /// <summary>
    /// Display sub-menu for Admin 
    /// </summary>
    static void SubMenuAdmin()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Admin Sub-menu ---");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Advance system clock by a minute");
            Console.WriteLine("2. Advance system clock by an hour");
            Console.WriteLine("3. Advance system clock by a minute you enter");
            Console.WriteLine("4. Display current value of system clock");
            Console.WriteLine("5. Set a new value for a configuration variable");
            Console.WriteLine("6. Display the current value of a configuration variable");
            Console.WriteLine("7. Reset values for all configuration variables");
            Console.Write("Choose an option: ");
            ConfigSubMenuEnum choice = ReadHelper.ReadEnum<ConfigSubMenuEnum>();

            switch (choice)
            {
                case ConfigSubMenuEnum.AdvanceMinute:
                    AdvanceClock(1);
                    break;
                case ConfigSubMenuEnum.AdvanceHour:
                    AdvanceClock(60);
                    break;
                case ConfigSubMenuEnum.AdvancePress:
                    int minuteToAdvance = ReadHelper.ReadInt();
                    AdvanceClock(minuteToAdvance);
                    break;
                case ConfigSubMenuEnum.DisplayClock:
                    Console.WriteLine($"Current system clock: {s_dal.Config.Clock}");
                    break;
                case ConfigSubMenuEnum.SetOne:
                    SetConfigurationValue();
                    break;
                case ConfigSubMenuEnum.DisplayConfig:
                    DisplayConfigurationValue();
                    break;
                case ConfigSubMenuEnum.Reset:
                    s_dal.Config.Reset();
                    Console.WriteLine("All configuration variables have been reset.");
                    break;
                case ConfigSubMenuEnum.Exit:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }
    /// <summary>
    /// login to system
    /// </summary>
     static void LoginSystem()
     {
        Console.WriteLine("press name and password to login");
        Console.WriteLine("your position is a "+s_bl.Volunteer.Login(ReadHelper.ReadString(),ReadHelper.ReadString()));
     }
    /// <summary>
    /// show list of volunteers 
    /// </summary>
    static void DisplayAllVolunteer()
    {
        Console.WriteLine("Type all but 0 to get a list filtered by active and inactive.");
        bool? filActive= bool.TryParse(Console.ReadLine(), out var active) ? active : null;
        Console.WriteLine("Choose a number by which the list will be sorted:\n 1.Id\n 2.Name\n 3.Active\n 4.SumCancledCalls\n 5.SumCaredCalls\n 6.sumIrelevantCalls\n 7.IdOfCall\n 8.KindOfCall\n");
     //אם הוא ילחץ על מספר לא באינם זה יהיה לו נל או שישים ערך לא נכון?
        BO.VoluteerInListObjects? filAll = ReadHelper.ReadEnum<BO.VoluteerInListObjects>();
        IEnumerable<BO.VolunteerInList> volunteers=s_bl.Volunteer.ReadAll(filActive,filAll);
        foreach (var item in volunteers)
        {
            Console.WriteLine(item);
        }
    }
    /// <summary>
    /// Display Volunteer By Id 
    /// </summary>
    static void DisplayByIdVolunteer()
    {
        Console.WriteLine("press id to search");
        Console.WriteLine(s_bl.Volunteer.Read(ReadHelper.ReadInt()));
    }
    /// <summary>
    /// Delete Volunteer By Id
    /// </summary>
    static void DeleteVolunteer()
    {
        Console.WriteLine("press id to delete");
        try { s_bl.Volunteer.DeleteVolunteer(ReadHelper.ReadInt()); }
        catch(Exception ex) { throw ex; }
    }
    /// <summary>
    /// Add or Update cccccccVolunteer
    /// </summary>
    static BO.Volunteer AddUpdateVolunteer()
    {
        Console.Write("Enter Volunteer details: Id, Name, Phone-Number, Email,Position,Password,Active,Current-Address,Latitude,Longitude, Maximum-Distance-For-Reading,Type-Of-Distance\n");
        return new()
        {
            Id = ReadHelper.ReadInt(),
            Name = ReadHelper.ReadString(),
            PhoneNumber = ReadHelper.ReadString(),
            Email = ReadHelper.ReadString(),
            Position = ReadHelper.ReadEnum<BO.Position>(),
            Password = ReadHelper.ReadString(),
            Active = bool.TryParse(Console.ReadLine(), out var active) ? active : false,
            CurrentAddress = ReadHelper.ReadString(),
            Latitude = ReadHelper.ReadDouble(),
            Longitude = ReadHelper.ReadDouble(),
            MaximumDistanceForReading = ReadHelper.ReadDouble(),
            TypeOfDistance = ReadHelper.ReadEnum<BO.TypeOfDistance>(),
            SumCancledCalls = 0,
            SumCaredCalls = 0,
            SumIrelevantCalls = 0,
            CallInProgress = null,
        };
    }
    ///<summary>
    ///Add New Volunteer
    /// </summary>
    static void AddNewVolunteer()
    {
        try
        {
            s_bl.Volunteer.AddVolunteer(AddUpdateVolunteer());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Update Volunteer
    /// </summary>
    static void UpdateVolunteer()
    {
        Console.WriteLine("press your id");
        int idRequest = ReadHelper.ReadInt();
        try
        {
            s_bl.Volunteer.UpdateVolunteer(idRequest, AddUpdateVolunteer());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Add or Update Call
    /// </summary>
    static BO.Call AddUpdateCall()
    {
        return new()
        {
            Id = ReadHelper.ReadInt(),//if the id is 0 it will update in the creat function in the CallImplementation
            KindOfCall = ReadHelper.ReadEnum<BO.KindOfCall>(),
            AddressOfCall = ReadHelper.ReadString(),
            Latitude = ReadHelper.ReadDouble(),
            Longitude = ReadHelper.ReadDouble(),
            OpeningTime = ReadHelper.ReadDate(),
            FinishTime = ReadHelper.ReadDate(),
            Description = ReadHelper.ReadString(),
            status=ReadHelper.ReadEnum<BO.Status>()
        };
    }
    private static void GetAllCallByVolunteer()
    {
        throw new NotImplementedException();
    }

    private static void GetCall()
    {
        throw new NotImplementedException();
    }

    private static void UpdateCall()
    {
        throw new NotImplementedException();
    }

    private static void UpdateEndCall()
    {
        throw new NotImplementedException();
    }

    private static void DisplayById()
    {
        throw new NotImplementedException();
    }

    private static void DisplayAll()
    {
        throw new NotImplementedException();
    }

    private static void DeleteCall()
    {
        throw new NotImplementedException();
    }

    private static void CancelCall()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Call By Status
    /// </summary>
    private static void CallByStatus()
    {
        int[] Calls=s_bl.Call.CallByStatus();
        for (int i = 0; i < Calls.Length; i++)
        {
            BO.Status status = (BO.Status)i;
            Console.WriteLine($"amount Call in {status}: {Calls[i]}");
        }
    }
    private static void AddCall()
    {
        throw new NotImplementedException();
    }

}
