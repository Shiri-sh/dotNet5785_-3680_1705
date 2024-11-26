using Dal;
using DalApi;
using DO;
using System.Data;

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
                    SubMenu("Volunteer");
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
                    s_dalConfig.Reset();
                    s_dalVolunteer.DeleteAll();
                    s_dalCall.DeleteAll();
                    s_dalAssignment.DeleteAll();
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
                Console.WriteLine($"error: {ex.Message}");
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
            Console.WriteLine("3. Advance system clock by a minute you enter");
            Console.WriteLine("4. Display current value of system clock");
            Console.WriteLine("5. Set a new value for a configuration variable");
            Console.WriteLine("6. Display the current value of a configuration variable");
            Console.WriteLine("7. Reset values for all configuration variables");
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
                    int minuteToAdvance = int.Parse(Console.ReadLine());
                    AdvanceClock(minuteToAdvance);
                    break;
                case "4":
                    Console.WriteLine($"Current system clock: {s_dalConfig.Clock}");
                    break;
                case "5":
                    SetConfigurationValue();
                    break;
                case "6":
                    DisplayConfigurationValue();
                    break;
                case "7":
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
        try
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString() + " try again");
        }

    }
    static Volunteer CreateNewVolunteer(int id = 0)
    {
        Volunteer newVolunteer=new Volunteer();
        int finalId=0;
        try
        {
            Console.Write("Enter Volunteer details: Id, Name, Phone-Number, Email,Position,Password,Active,Current-Address,Latitude,Longitude, Maximum-Distance-For-Reading,Type-Of-Distance");
            if (id == 0)///that means that you got here for creat new volunteer
                finalId = int.TryParse(Console.ReadLine(), out var _id) ? _id : throw new FormatException("Id is unvalid!");
            else//you w
                finalId = id;
             newVolunteer = new Volunteer()
            {
                Id = finalId,
                Name = StringParse(),
                PhoneNumber = StringParse(),
                Email = StringParse(),
                Position = Enum.TryParse<Position>(Console.ReadLine(), out var position) ? position : Position.Volunteer,
                Password = StringParse(),
                Active = bool.TryParse(Console.ReadLine(), out var active) ? active : false,
                CurrentAddress = StringParse(),
                Latitude = double.TryParse(Console.ReadLine(), out var latitude) ? latitude : (double?)null,
                Longitude = double.TryParse(Console.ReadLine(), out var longitude) ? longitude : (double?)null,
                MaximumDistanceForReading = double.TryParse(Console.ReadLine(), out var maxDistance) ? maxDistance : (double?)null,
                TypeOfDistance = Enum.TryParse<TypeOfDistance>(Console.ReadLine(), out var typeOfDistance) ? typeOfDistance : TypeOfDistance.Aerial
            };
            return newVolunteer;
        }
        catch (Exception ex)
        {
            Console.WriteLine(finalId);
            ReadVolunteer(newVolunteer);
            throw ex;
        }
    }
    static Assignment CreateNewAssigment(int id = 0)
    {
        Console.WriteLine("Enter Assignment details: Id, Called-Id, Volunteer-Id,Treatment-Entry-Time, Treatment-End-Time,Type-Of-Treatment-Termination ");
        try
        {
            Assignment newAssignment = new Assignment()
            {
                Id = id, //int.TryParse(Console.ReadLine(), out var id) ? id : throw new FormatException("Id is invalid!"),
                CalledId = int.TryParse(Console.ReadLine(), out var calledId) ? calledId : throw new FormatException("calledId is invalid!"),
                VolunteerId = int.TryParse(Console.ReadLine(), out var volunteerId) ? volunteerId : throw new FormatException("volunteerId is invalid!"),
                TreatmentEntryTime = DateTime.TryParse(Console.ReadLine(), out var treatmentEntryTime) ? treatmentEntryTime : s_dalConfig.Clock,
                TreatmentEndTime = DateTime.TryParse(Console.ReadLine(), out var treatmentEndTime) ? treatmentEndTime : null,
                TypeOfTreatmentTermination = Enum.TryParse<TypeOfTreatmentTermination>(Console.ReadLine(), out var typeOfTreatmentTermination) ? typeOfTreatmentTermination : null,
            };
            return newAssignment;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    static Call CreateNewCall(int id = 0)
    {
        Console.Write("Enter Call details: Id,Kind-Of-Call,Address-Of-Call,Latitude,Longitude,Opening-Time,Finish-Time,Description");
        try
        {
            Call newCall = new Call()
            {
                Id = id,
                KindOfCall = Enum.TryParse<KindOfCall>(Console.ReadLine(), out var kindOfCall) ? kindOfCall : KindOfCall.changeWheel,
                AddressOfCall = StringParse(),
                Latitude = double.TryParse(Console.ReadLine(), out var latitude) ? latitude : throw new FormatException("Latitude is invalid!"),
                Longitude = double.TryParse(Console.ReadLine(), out var longitude) ? longitude : throw new FormatException("Longitude is invalid!"),
                OpeningTime = DateTime.TryParse(Console.ReadLine(), out var openingTime) ? openingTime : s_dalConfig.Clock,
                FinishTime = DateTime.TryParse(Console.ReadLine(), out var finishTime) ? finishTime : s_dalConfig.Clock,
                Description = StringParse()
            };
            return newCall;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    static void ReadVolunteer(Volunteer volunteerToRead)
    {
        Console.WriteLine("Id: " + volunteerToRead.Id +
            " Name: " + volunteerToRead.Name +
            " PhoneNumber: " + volunteerToRead.PhoneNumber +
            " Email: " + volunteerToRead.Email +
            " Position: " + volunteerToRead.Position +
            " Password: " + volunteerToRead.Password +
            " Active: " + volunteerToRead.Active);
        if (!string.IsNullOrEmpty(volunteerToRead.CurrentAddress))
            Console.WriteLine("CurrentAddress: " + volunteerToRead.CurrentAddress);
        else
            Console.WriteLine("CurrentAddress: null");
        if (volunteerToRead.Latitude.HasValue)
            Console.WriteLine("Latitude: " + volunteerToRead.Latitude.Value);
        else
            Console.WriteLine("Latitude: null");

        if (volunteerToRead.Longitude.HasValue)
            Console.WriteLine("Longitude: " + volunteerToRead.Longitude.Value);
        else
            Console.WriteLine("Longitude: null");
        if (volunteerToRead.MaximumDistanceForReading.HasValue)
            Console.WriteLine("MaximumDistanceForReading: " + volunteerToRead.MaximumDistanceForReading.Value);
        else
            Console.WriteLine("MaximumDistanceForReading: null");
        Console.WriteLine("TypeOfDistance: " + volunteerToRead.TypeOfDistance);
        Console.WriteLine("----------------------------");
    }
    static void ReadAssignment(Assignment assignmentToRead)
    {
        Console.WriteLine("Id: " + assignmentToRead.Id);
        Console.WriteLine("CalledId: " + assignmentToRead.CalledId);
        Console.WriteLine("VolunteerId: " + assignmentToRead.VolunteerId);
        Console.WriteLine("TreatmentEntryTime: " + assignmentToRead.TreatmentEntryTime);
        if (assignmentToRead.TreatmentEndTime.HasValue)
            Console.WriteLine("TreatmentEndTime: " + assignmentToRead.TreatmentEndTime.Value);
        else
            Console.WriteLine("TreatmentEndTime: null");

        if (assignmentToRead.TypeOfTreatmentTermination.HasValue)
            Console.WriteLine("TypeOfTreatmentTermination: " + assignmentToRead.TypeOfTreatmentTermination.Value);
        else
            Console.WriteLine("TypeOfTreatmentTermination: null");
        Console.WriteLine("----------------------------");
    }
    static void ReadCall(Call callToRead)
    {
        Console.WriteLine("Id: " + callToRead.Id);
        Console.WriteLine("KindOfCall: " + callToRead.KindOfCall);
        Console.WriteLine("AddressOfCall: " + callToRead.AddressOfCall);
        Console.WriteLine("Latitude: " + callToRead.Latitude);
        Console.WriteLine("Longitude: " + callToRead.Longitude);
        Console.WriteLine("OpeningTime: " + callToRead.OpeningTime);
        if (callToRead.FinishTime.HasValue)
            Console.WriteLine("FinishTime: " + callToRead.FinishTime.Value);
        else
            Console.WriteLine("FinishTime: null");

        if (!string.IsNullOrEmpty(callToRead.Description))
            Console.WriteLine("Description: " + callToRead.Description);
        else
            Console.WriteLine("Description: null");
        Console.WriteLine("----------------------------");
    }
    static void ReadEntity(string entityName)
    {
        Console.WriteLine("Enter an ID number for the object you want to display.");
        int idToRead = int.TryParse(Console.ReadLine(), out var id) ? id : throw new FormatException("Id is invalid!");
        if (entityName is "Volunteer")
        {
            Volunteer volunteerToRead = s_dalVolunteer.Read(idToRead);
            ReadVolunteer(volunteerToRead);
        }
        else if (entityName is "Assignment")
        {
            Assignment assignmentToRead = s_dalAssignment.Read(idToRead);
            ReadAssignment(assignmentToRead);
        }
        else
        {
            Call callToRead = s_dalCall.Read(idToRead);
            ReadCall(callToRead);
        }
    }
    static void ReadAllEntities(string entityName)
    {
        Console.WriteLine($"View all objects for {entityName}");
        if (entityName is "Volunteer")
        {

            List<Volunteer> Volunteers = s_dalVolunteer.ReadAll();

            foreach (var volunteer in Volunteers)
            {
                ReadVolunteer(volunteer);
            }

        }
        else if (entityName is "Assignment")
        {
            List<Assignment> assignments = s_dalAssignment.ReadAll();

            foreach (var assignment in assignments)
            {
                ReadAssignment(assignment);
            }
        }
        else
        {
            List<Call> calls = s_dalCall.ReadAll();

            foreach (var call in calls)
            {
                ReadCall(call);
            }
        }
    }
    static void UpdateEntity(string entityName)
    {
        try
        {
            Console.WriteLine("Enter the ID number for the object you want to update.");
            int id = int.TryParse(Console.ReadLine(), out var _id) ? _id : throw new FormatException("Id is invalid!");
            if (entityName is "Volunteer")
            {
                s_dalVolunteer.Update(CreateNewVolunteer(id));
            }
            else if (entityName is "Assignment")
            {
                s_dalAssignment.Update(CreateNewAssigment(id));
            }
            else
            {
                s_dalCall.Update(CreateNewCall(id));
            }
            Console.WriteLine($"Update object for {entityName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " try again");
        }

    }
    static void DeleteEntity(string entityName)   //delete entity by id
    {
        try
        {
            Console.WriteLine("Enter the ID number for the object you want to delete.");
            int idToDelete = int.TryParse(Console.ReadLine(), out var idDelete) ? idDelete : throw new FormatException("Id is unvalid!");
            if (entityName is "Volunteer")
            {
                s_dalVolunteer.Delete(idToDelete);
            }
            else if (entityName is "Assignment")
            {
                s_dalAssignment.Delete(idToDelete);
            }
            else
            {
                s_dalCall.Delete(idToDelete);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " try again");
        }
    }
    static void DeleteAllEntities(string entityName)
    {
        Console.WriteLine($"Deleting all objects for {entityName}");
        if (entityName is "Volunteer")
        {
            s_dalVolunteer.DeleteAll();
        }
        else if (entityName is "Assignment")
        {
            s_dalAssignment.DeleteAll();
        }
        else
        {
            s_dalCall.DeleteAll();
        }
    }
    static void AdvanceClock(int minutes)
    {
        Console.WriteLine($"The system clock has advanced by-{minutes} minute.");
        s_dalConfig.Clock.AddMinutes(minutes);
    }
    static void SetConfigurationValue()
    {
        Console.WriteLine("To change a specific variable, press:\n 1. to change RiskRange. \n 2. to reset the clock");
        int chooseWhatToDo = int.TryParse(Console.ReadLine(), out var chooseDo) ? chooseDo : throw new FormatException("choose is unvalid!");
        Console.WriteLine($"enter number to update");
        int numberToUpdate = int.TryParse(Console.ReadLine(), out var numberUpdate) ? numberUpdate : throw new FormatException("number is unvalid!");
        if (chooseWhatToDo is 1)
            s_dalConfig.RiskRange = TimeSpan.FromMinutes(numberToUpdate);
        else if (chooseWhatToDo is 2)
            s_dalConfig.Clock = DateTime.Now;
    }
    static void DisplayConfigurationValue()
    {
        Console.WriteLine("To display a specific variable, press:\n 1.to display RiskRange. \n 2. to display the clock");
        int chooseWathToDo = int.TryParse(Console.ReadLine(), out var chooseWathDo) ? chooseWathDo : throw new FormatException("Id is invalid!");
        if (chooseWathToDo is 1)
            Console.WriteLine(s_dalConfig.RiskRange);
        else if (chooseWathToDo is 1)
            Console.WriteLine(s_dalConfig.Clock);
    }
    static void DisplayAllData()
    {
        ReadAllEntities("Volunteer");
        ReadAllEntities("Assignment");
        ReadAllEntities("Call");
        Console.WriteLine($"clock: {s_dalConfig.Clock}.\n nextcallid:{s_dalConfig.RiskRange} ");
    }
    static string? StringParse()
    {
        string st = Console.ReadLine();
        if (string.IsNullOrEmpty(st))
            throw new FormatException("input is invalid!");
        return st;
    }

}

