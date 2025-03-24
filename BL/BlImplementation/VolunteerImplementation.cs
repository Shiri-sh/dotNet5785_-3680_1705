using BlApi;
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

        VolunteerManager.ValidateVolunteer(boVolunteer);
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
        var doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id)??
                     throw new BO.BlDoesNotExistException($"Volunteer with {id} does Not exist");
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
        DO.Volunteer? doVolunteer;
        doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id)??
                 throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
        ICall call=new CallImplementation();
        DO.Assignment? assignment=_dal.Assignment.Read(a=>a.VolunteerId== id && a.TreatmentEndTime == null) ??null;
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
            SumCancledCalls = call.GetCloseCallByVolunteer(id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.SelfCancellation),
            SumCaredCalls = call.GetCloseCallByVolunteer(id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.Handled),
            SumIrelevantCalls = call.GetCloseCallByVolunteer(id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.CancellationExpired),
            CallInProgress = new BO.CallInProgress{
                            Id=assignment.Id,
                            CallId= assignment.CalledId,
                            KindOfCall= (BO.KindOfCall)callInProgress.KindOfCall,
                            AddressOfCall= callInProgress.AddressOfCall,
                            OpeningTime= callInProgress.OpeningTime,
                            FinishTime= callInProgress.FinishTime,
                            Description = callInProgress.Description,
                            TreatmentEntryTime= assignment.TreatmentEntryTime,
                            DistanceFromVolunteer= CallManager.GetDistanceFromVol(callInProgress.Latitude, callInProgress.Longitude,doVolunteer.Latitude, doVolunteer.Longitude),//////dont know if this good parameters
                            Status=  CallManager.StatusCallInProgress(callInProgress)
            }
        };
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? activity = null, BO.VoluteerInListObjects? feildToSort = null)
    {
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll();
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
        ICall call = new CallImplementation();
        return volunteers.Select(v => new BO.VolunteerInList {
            Id = v.Id,
            Name = v.Name,
            Active=v.Active,
            SumCancledCalls= call.GetCloseCallByVolunteer(v.Id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.SelfCancellation),
            SumCaredCalls = call.GetCloseCallByVolunteer(v.Id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.Handled),
            SumIrelevantCalls = call.GetCloseCallByVolunteer(v.Id).Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.CancellationExpired),
            IdOfCall = _dal.Assignment.Read(a => a.VolunteerId == v.Id && a.TreatmentEndTime==null).CalledId,
            KindOfCall = (BO.KindOfCall)_dal.Call.Read(a => a.Id == _dal.Assignment.Read(a => a.VolunteerId == v.Id && a.TreatmentEndTime == null).CalledId).KindOfCall
        });
    }
    public void UpdateVolunteer(int id, BO.Volunteer volunteer)
    {
        //המתנדב קיים במערכת
        DO.Volunteer doVolunteer = _dal.Volunteer.Read(vol => vol.Id == volunteer.Id)??
            throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
        DO.Volunteer voluRequest = _dal.Volunteer.Read(vol => vol.Id == id) ?? 
            throw new BO.BlDoesNotExistException($"someone with ID={id} does Not exist");
        //המעדכן הוא מנהל או האדם עצמו
        if ( id!= volunteer.Id && voluRequest.Position!=DO.Position.Managar)
        {
            throw new BO.BlNotAloudToDoException("Only a managar can update a volunteer or the volunteer himself");
        }
        // בדיקת תקינות של הנתונים שהוזנו
        VolunteerManager.ValidateVolunteer(volunteer);
        //בדיקת שינויים בשדות מסוימים
        if ((volunteer.Position != (BO.Position)doVolunteer.Position || volunteer.Active!=doVolunteer.Active)&&voluRequest.Position!=DO.Position.Managar)
        {
            throw new BO.BlNotAloudToDoException("Only a managar can update the volunteer's Position");
        }
        // עדכון הנתונים במערכת
        _dal.Volunteer.Update(new DO.Volunteer
        {
            Id = volunteer.Id,
            Name = volunteer.Name,
            Email = volunteer.Email,
            Longitude = volunteer.Longitude,
            Latitude = volunteer.Latitude,
            Position = (DO.Position)volunteer.Position,
            Password =volunteer.Password,
            Active = volunteer.Active,
            CurrentAddress = volunteer.CurrentAddress,
            MaximumDistanceForReading = volunteer.MaximumDistanceForReading,
            TypeOfDistance = (DO.TypeOfDistance)volunteer.TypeOfDistance
        });
    }
}
