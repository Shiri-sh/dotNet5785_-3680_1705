using Accessories;
using BO;
using DalApi;

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
            BO.MainMenuEnum choice = ReadHelper.ReadEnum<BO.MainMenuEnum>();
            switch (choice)
            {
                case BO.MainMenuEnum.SubMenuVolunteer:
                    SubMenuVolunteer();
                    break;
                case BO.MainMenuEnum.SubMenuCall:
                    SubMenuCall();
                    break;
                case BO.MainMenuEnum.SubMenuAdmin:
                    SubMenuAdmin();
                    break;
                case BO.MainMenuEnum.Exit:
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
            Console.WriteLine("1. Add a new volunteer");
            Console.WriteLine("2. Display an volunteer by ID");
            Console.WriteLine("3. Display the list of all volunteer");
            Console.WriteLine("4. Update a volunteer");
            Console.WriteLine("5. Delete a volunteer");
            Console.WriteLine("6. Login System");
            Console.Write("Choose an option: ");
            BO.SubMenuVolunteerEnum choice = ReadHelper.ReadEnum<BO.SubMenuVolunteerEnum>();
            try
            {
                switch (choice)
                {
                    case BO.SubMenuVolunteerEnum.AddNew:
                        AddNewVolunteer();
                        break;
                    case BO.SubMenuVolunteerEnum.DisplayById:
                        DisplayByIdVolunteer();
                        break;
                    case BO.SubMenuVolunteerEnum.DisplayAll:
                        DisplayAllVolunteer();
                        break;
                    case BO.SubMenuVolunteerEnum.Update:
                        UpdateVolunteer();
                        break;
                    case BO.SubMenuVolunteerEnum.Delete:
                        DeleteVolunteer();
                        break;
                    case BO.SubMenuVolunteerEnum.LoginSystem:
                        LoginSystem();
                        break;
                    case BO.SubMenuVolunteerEnum.Exit:
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
            Console.WriteLine("1. Add a new call");
            Console.WriteLine("2. call by status");
            Console.WriteLine("3. cancel call");
            Console.WriteLine("4. delete call");
            Console.WriteLine("5. Display list of calls ");
            Console.WriteLine("6. Display by id of volunteer list of calls ");
            Console.WriteLine("7. Update call to ent treatment");
            Console.WriteLine("8. Update call details");
            Console.WriteLine("9. Display details of call ");
            Console.WriteLine("10. Coose Call");
            Console.WriteLine("11. get all open call");
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
                    case BO.SubMenuCallEnum.GetAllCallByVolunteer:
                        GetAllCallByVolunteer();
                        break;
                    case BO.SubMenuCallEnum.CooseCall:
                        CooseCall();
                        break;
                    case BO.SubMenuCallEnum.OpenCalls:
                        OpenCalls();
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
            Console.WriteLine("1. Display current value of system clock");
            Console.WriteLine("2. display time risk ");
            Console.WriteLine("3. Set a new value for time risk");
            Console.WriteLine("4. Reset db");
            Console.WriteLine("5. intilazition db");
            Console.WriteLine("5. update clock");
            Console.Write("Choose an option: ");
            SubMenuAdminEnum choice = ReadHelper.ReadEnum<SubMenuAdminEnum>();
            switch (choice)
            {
                case SubMenuAdminEnum.DisplayTime:
                    Console.WriteLine(s_bl.Admin.GetClock());
                    break;
                case SubMenuAdminEnum.DisplayTimeRisk:
                    Console.WriteLine(s_bl.Admin.GetRiskRange());
                    break;
                case SubMenuAdminEnum.updateRiskTime:
                    Console.WriteLine("press time to update risk range time");
                    if(TimeSpan.TryParse(Console.ReadLine(), out TimeSpan timespam))
                         s_bl.Admin.UpdateRiskRange(timespam);
                    break;
                case SubMenuAdminEnum.reset:
                    s_bl.Admin.Reset();
                    break;
                case SubMenuAdminEnum.initialization:
                    s_bl.Admin.Initialization();
                    break;
                case SubMenuAdminEnum.updateTime:
                    Console.WriteLine("press number of type time:\n 1.Minute\n 2.Hour\n 3.Day\n 4.Month\n 5.Year");
                    s_bl.Admin.UpdateClock(ReadHelper.ReadEnum<TypeOfTime>()); break;
                case SubMenuAdminEnum.Exit:
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
        catch (BO.BlDoesNotExistException ex) { Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}", ex); }
    }
    /// <summary>
    /// Add or Update cccccccVolunteer
    /// </summary>
    static BO.Volunteer AddUpdateVolunteer()
    {
        Console.Write("Enter Volunteer details: Id, Name, Phone-Number, Email,Position,Password,Active,Current-Address,Latitude,Longitude, Maximum-Distance-For-Reading,Type-Of-Distance\n");
        return new()
        {
            //חסר כאן המידע שאם זה עדכון אז שיהיה קריאה בתהליך
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
        catch (BO.BlAlreadyExistsException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}", ex);
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
        catch (BO.BlDoesNotExistException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
        catch (BO.BlNotAloudToDoException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
    }

    //----------------call
    /// <summary>
    /// Add or Update Call
    /// </summary>
    static BO.Call AddUpdateCall()
    {
        Console.Write("Enter Call details: Id,Kind-Of-Call,Address-Of-Call,Latitude,Longitude,Opening-Time,Finish-Time,Description,status\n");
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
            Status=ReadHelper.ReadEnum<BO.Status>()
        };
    }
    /// <summary>
    /// dispaly all call by id of volunteer
    /// </summary>
    private static void GetAllCallByVolunteer()
    {
        Console.WriteLine("press your id of volunteer");
        int idV=ReadHelper.ReadInt();
        Console.WriteLine("Choose a number by which the list will be filterd:\n 1.RescueKid\n 2.changeWheel\n 3.FirstAid\n 4.CableAssistance\n 5.fuelOilWater\n 6.None\n");
        BO.KindOfCall? filAll = ReadHelper.ReadEnum<BO.KindOfCall>();
        Console.WriteLine("Choose a number by which the list will be sorted:\n 1.Id\n 2.KindOfCall\n 3.AddressOfCall\n 4.OpeningTime\n 5.TreatmentEntryTime\n 6.TreatmentEndTime\n 7.TypeOfTreatmentTermination\n");
        BO.CloseCallInListObjects? sortAll = ReadHelper.ReadEnum<BO.CloseCallInListObjects>();
        IEnumerable<BO.ClosedCallInList> calls = s_bl.Call.GetCloseCallByVolunteer(idV, filAll, sortAll);
        foreach (var item in calls)
        {
            Console.WriteLine(item);
        }
    }
    /// <summary>
    /// get detail of call
    /// </summary>
    static void DisplayById()
    {
        Console.WriteLine("press id of call");
        try { Console.WriteLine(s_bl.Call.ReadCall(ReadHelper.ReadInt())); }
        catch (BO.BlDoesNotExistException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
    }
    /// <summary>
    /// Update Call
    /// </summary>
    static void UpdateCall()
    {
        try
        {
            s_bl.Call.UpdateCall(AddUpdateCall());
        }
        catch (BO.BlDoesNotExistException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
        catch (BO.BlInvalidDataException ex){
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}"); 
        }
    }
    /// <summary>
    /// Update End Call
    /// </summary>
    static void UpdateEndCall()
    {
        Console.WriteLine("press your id and id of call you want to close");
        try
        {
            s_bl.Call.UpdateEndCall(ReadHelper.ReadInt(), ReadHelper.ReadInt());
        }
        catch (BO.BlDoesNotExistException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
        catch(BO.BlNotAloudToDoException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
    }
    /// <summary>
    /// display list of call
    /// </summary>
     static void DisplayAll()
    {
        object? filValue=null;
        Console.WriteLine("Choose a number by which the list will be filterd:\n 1.Id\n 2.CallId\n 3.KindOfCall\n 4.OpeningTime\n 5.RemainingTimeToFinish\n 6.LastVolunteer\n 7.CompletionTime\n 8.Status\n 9.TotalAlocation\n");
        BO.CallInListObjects? filAll = ReadHelper.ReadEnum<BO.CallInListObjects>();
        if (filAll != null) {
            Console.WriteLine("press value to filter");
             filValue = Console.ReadLine();
        }
        Console.WriteLine("Choose a number by which the list will be sorted:\n 1.Id\n 2.CallId\n 3.KindOfCall\n 4.OpeningTime\n 5.RemainingTimeToFinish\n 6.LastVolunteer\n 7.CompletionTime\n 8.Status\n 9.TotalAlocation\n");
        BO.CallInListObjects? sortAll = ReadHelper.ReadEnum<BO.CallInListObjects>();
        IEnumerable<BO.CallInList> calls = s_bl.Call.CallList( filAll, filValue,sortAll);
        foreach (var item in calls)
        {
            Console.WriteLine(item);
        }
     }
    /// <summary>
    /// Delete Call
    /// </summary>
     static void DeleteCall()
    {
        Console.WriteLine("press id of call to delete");
        try { s_bl.Call.DeleteCall(ReadHelper.ReadInt()); }
        catch (BO.BlDoesNotExistException ex) { Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}"); }
        catch (BO.BlNotAloudToDoException ex) { Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}"); }

    }
        /// <summary>
        /// cancel call
        /// </summary>
        static void CancelCall()
    {
        Console.WriteLine("press your id and id of call");
        try { s_bl.Call.UpdateCancelCall(ReadHelper.ReadInt(), ReadHelper.ReadInt()); }
        catch (BO.BlDoesNotExistException ex) { Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}"); }
        catch (BO.BlNotAloudToDoException ex) { Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}"); }
    }
    /// <summary>
    /// Call By Status
    /// </summary>
     static void CallByStatus()
    {
        int[] Calls=s_bl.Call.CallByStatus();
        for (int i = 0; i < Calls.Length; i++)
        {
            BO.Status status = (BO.Status)i;
            Console.WriteLine($"amount Call in {status}: {Calls[i]}");
        }
    }
    /// <summary>
    /// add call
    /// </summary>
     static void AddCall()
    {
        try
        {
            s_bl.Call.AddCall(AddUpdateCall());
        }
        catch (BO.BlAlreadyExistsException ex)
        {
            Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}");
        }
    }
    /// <summary>
    ///volunteer Coose Call to treat
    /// </summary>
     static void CooseCall()
    {
        Console.WriteLine("press your id and id of call you want treat");
        try { s_bl.Call.CooseCall(ReadHelper.ReadInt(), ReadHelper.ReadInt()); }
        catch (BO.BlNotAloudToDoException ex) { Console.WriteLine($"Error: {ex.GetType().Name}, Message: {ex.Message}"); }
    }
   
    /// <summary>
    /// get open calls that volunteer can choose
    /// </summary>
    static void OpenCalls()
    {
        Console.WriteLine("press your id of volunteer");
        int idV = ReadHelper.ReadInt();
        Console.WriteLine("Choose a number by which the list will be filterd:\n 1.RescueKid\n 2.changeWheel\n 3.FirstAid\n 4.CableAssistance\n 5.fuelOilWater\n 6.None\n");
        BO.KindOfCall? filAll = ReadHelper.ReadEnum<BO.KindOfCall>();
        Console.WriteLine("Choose a number by which the list will be sorted:\n 1.Id\n 2.KindOfCall\n 3.AddressOfCall\n 4.OpeningTime\n 5.FinishTime\n 6.Description\n 7.DistanceFromVol\n");
        BO.OpenCallInListFields? sortAll = ReadHelper.ReadEnum<BO.OpenCallInListFields>();
        IEnumerable<BO.OpenCallInList> calls = s_bl.Call.GetOpenCallByVolunteer(idV, filAll, sortAll);
        foreach (var item in calls)
        {
            Console.WriteLine(item);
        }
    }
}
