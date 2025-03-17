using BlApi;
using BO;
using DO;


//using BO;
using Helpers;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BlImplementation;

internal class VolunteerImplementation: IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(BO.Volunteer boVolunteer)
    {

        ValidateVolunteer(boVolunteer);
        DO.Volunteer doVolunteer =
            new(boVolunteer.Id,
            boVolunteer.Name,
            boVolunteer.PhoneNumber,
            boVolunteer.Email,
            (DO.Position)boVolunteer.Position,
            boVolunteer.Password,
            boVolunteer.Active,
            boVolunteer.CurrentAddress,
            boVolunteer.Latitude,
            boVolunteer.Longitude,
            boVolunteer.MaximumDistanceForReading,
            (DO.TypeOfDistance)boVolunteer.TypeOfDistance
        );
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
        }
    }

    public void DeleteVolunteer(int id)
    {
        try
        {
            var doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id);

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with {id} does Not exist",ex);//need tocreate it later
        }
        DO.Assignment? assignment = _dal.Assignment.Read(assign=>assign.VolunteerId==id);
        if (assignment == null)
        {
            _dal.Volunteer.Delete(id);
        }
        
    }
    public BO.Position Login(string username, string password)
    {

        var doVolunteer = _dal.Volunteer.Read(vol =>  vol.Name == username && vol.Password == password) ??
        throw new BO.BlDoesNotExistException($"Volunteer with Name ={username} and Password={password} does Not exist");//need tocreate it later
        return (BO.Position)doVolunteer.Position;
    }


    public BO.Volunteer Read(int id)
    {
        DO.Volunteer doVolunteer;
        try
        {
            doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id);
        }
        catch (DO.DalDoesNotExistException ex) {
            throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
       
        }
        //var allCall=_dal.Call.ReadAll(call=>call.Id==id);
        ICall call=new CallImplementation();
        DO.Assignment? assignment=_dal.Assignment.Read(a=>a.VolunteerId==id)??null;
        DO.Call? callInProgress = _dal.Call.Read(c => c.Id == assignment.CalledId)??null;
        return new()
        {
            Id = id,
            Name = doVolunteer.Name,
            PhoneNumber = doVolunteer.PhoneNumber,
            Email = doVolunteer.Email,
            Position = (BO.Position)doVolunteer.Position,
            Password = doVolunteer.Password,
            Active = doVolunteer.Active,
            CurrentAddress = doVolunteer.CurrentAddress,
            Latitude = doVolunteer.Latitude,
            Longitude = doVolunteer.Longitude,
            MaximumDistanceForReading = doVolunteer?.MaximumDistanceForReading,
            TypeOfDistance = (BO.TypeOfDistance)doVolunteer.TypeOfDistance,
            SumCancledCalls = call.GetAllCallByVolunteer(id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.SelfCancellation),
            SumCaredCalls = call.GetAllCallByVolunteer(id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.Handled),
            SumIrelevantCalls = call.GetAllCallByVolunteer(id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.CancellationExpired),
           //חסר סטטוס והמרחק
            CallInProgress = new (assignment.Id, assignment.CalledId,callInProgress.KindOfCall,callInProgress.AddressOfCall,callInProgress.OpeningTime,callInProgress.FinishTime,callInProgress.Description,assignment.TreatmentEntryTime)
        };
    }

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? activity = null, BO.VoluteerInListObjects? feildToSort = null)
    {
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll();
        //IEnumerable<BO.VolunteerInList> readVolunteers;
        volunteers = activity == null ? volunteers.Select(item => item) : volunteers.Where(v => v.Active == activity);

        if(feildToSort == null)
        {
            volunteers = volunteers.OrderBy(v => v.Id);
        }
        string propertyName = feildToSort.ToString();
        var propertyInfo = typeof(DO.Volunteer).GetProperty(propertyName);
        if (propertyInfo != null)
        {
            volunteers = volunteers.OrderBy(v => propertyInfo.GetValue(v, null));
        }
        return volunteers.Select(v => new BO.VolunteerInList { Id = v.Id, Name = v.Name ,Active=v.Active,SumCancledCalls=,SumCaredCalls=,SumIrelevantCalls=,IdOfCall=,KindOfCall= });
    }

    public void UpdateVolunteer(int id, BO.Volunteer volunteer)
    {
        DO.Volunteer doVolunteer;
        try
        {
            doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id);

        }
        catch (DO.DalDoesNotExistException e)
        {
            throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
        }

        if (doVolunteer.Position == DO.Position.Volunteer && doVolunteer.Id!=volunteer.Id) {
            throw new BO.BlNotAloudToDoException("Only a managar can update a volunteer or the volunteer himself");
        }
        // בדיקת תקינות של הנתונים שהוזנו
        ValidateVolunteer(volunteer);

        // עדכון הנתונים במערכת
        if (volunteer.Position != (BO.Position)doVolunteer.Position && doVolunteer.Position!=DO.Position.Managar)
        {
            throw new BlNotAloudToDoException("Only a managar can update the volunteer's Position");
        }
        _dal.Volunteer.Update(new DO.Volunteer
        {
            Id = volunteer.Id,
            Name = volunteer.Name,
            Email = volunteer.Email,
            Longitude = volunteer.Longitude,
            Latitude = volunteer.Latitude,
            Position = (DO.Position)volunteer.Position
        });

    }

    public void ValidateVolunteer(BO.Volunteer boVolunteer)
    {
        // בדיקת תקינות כתובת אימייל
        if (!Regex.IsMatch(boVolunteer.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
        {
            throw new BO.BlInvalidDataException("Invalid email format");
        }

        // בדיקת מספר טלפון (נניח בפורמט ישראלי)
        if (!Regex.IsMatch(boVolunteer.PhoneNumber, @"^\d{10}$"))
        {
            throw new BO.BlInvalidDataException("Invalid phone number format");
        }

        // בדיקת תקינות ת.ז
        if (!IsValidIsraeliID(boVolunteer.Id))
        {
            throw new BO.BlInvalidDataException("Invalid Israeli ID number");
        }

        if (!IsValidAddress(boVolunteer.Latitude,boVolunteer.Longitude))
        {
            throw new BO.BlInvalidDataException("Address cannot be empty");
        }
    }

    public  bool IsValidIsraeliID(int id)
    {
       
            string idStr = id.ToString().PadLeft(9, '0');
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int num = (idStr[i] - '0') * ((i % 2) + 1);
                sum += num > 9 ? num - 9 : num;
            }
            return sum % 10 == 0;
    }

    public bool IsValidAddress(double? lon, double? lat)
    {
        string requestUri = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}";

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = client.Send(new HttpRequestMessage(HttpMethod.Get, requestUri));

        if (!response.IsSuccessStatusCode) return false;

        string jsonResponse = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<OSMGeocodeResponse>(jsonResponse);

        return !string.IsNullOrWhiteSpace(result?.display_name);
    }
    private class OSMGeocodeResponse
    {
        public string display_name { get; set; }
    }
}
