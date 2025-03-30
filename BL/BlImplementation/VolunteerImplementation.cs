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
        double[]? latLon = boVolunteer.CurrentAddress == null ? null : Tools.GetCoordinates(boVolunteer.CurrentAddress);
        try
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
            latLon==null?null: boVolunteer.CurrentAddress,
            latLon?[0],
            latLon?[1],
            boVolunteer.MaximumDistanceForReading,
            (DO.TypeOfDistance)boVolunteer.TypeOfDistance
        );
       
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
        }
        catch (BO.BlInvalidDataException ex)
        {
            throw new BO.BlInvalidDataException(ex.Message);
        }
    }
    public void DeleteVolunteer(int id)
    {
        DO.Assignment? assignment = _dal.Assignment.Read(assign=>assign.VolunteerId==id);
        if (assignment == null)
        {
            try
            {
                _dal.Volunteer.Delete(id);
            }
            catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"Volunteer with {id} does Not exist"); }
        }
        else
            throw new BO.BlNotAloudToDoException($"A volunteer with assignments cannot be deleted.");

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
        DO.Assignment? assignment=_dal.Assignment.Read(a=>a.VolunteerId== id && a.TreatmentEndTime == null);
        DO.Call? callInProgress = assignment==null?null:_dal.Call.Read(c => c.Id == assignment.CalledId);
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
            CallInProgress = callInProgress!=null ? new BO.CallInProgress{
                            Id=assignment.Id,
                            CallId= assignment.CalledId,
                            KindOfCall= (BO.KindOfCall)callInProgress.KindOfCall,
                            AddressOfCall= callInProgress.AddressOfCall,
                            OpeningTime= callInProgress.OpeningTime,
                            FinishTime= callInProgress.FinishTime,
                            Description = callInProgress.Description,
                            TreatmentEntryTime= assignment.TreatmentEntryTime,
                            DistanceFromVolunteer= Tools.GetDistance(doVolunteer,callInProgress),//////dont know if this good parameters
                            Status=  CallManager.StatusCallInProgress(callInProgress)
            }:null
        };
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? activity = null, BO.VoluteerInListObjects? feildToSort = null)
    {
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll();
        volunteers = activity == null ? volunteers.Select(item => item) : volunteers.Where(v => v.Active == activity);
        ICall call = new CallImplementation();
        IEnumerable<BO.VolunteerInList> listOfVol= from v in volunteers
               let closeCalls = call.GetCloseCallByVolunteer(v.Id)
               let assign= _dal.Assignment.Read(a => a.VolunteerId == v.Id && a.TreatmentEndTime == null)
               select new BO.VolunteerInList
               {
                   Id = v.Id,
                   Name = v.Name,
                   Active = v.Active,
                   SumCancledCalls = closeCalls.Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.SelfCancellation),
                   SumCaredCalls = closeCalls.Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.Handled),
                   SumIrelevantCalls = closeCalls.Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.CancellationExpired),
                   IdOfCall = assign?.CalledId??null,
                   KindOfCall = assign==null ? null : (BO.KindOfCall)_dal.Call.Read(a => a.Id == assign.CalledId).KindOfCall

               };
        if (feildToSort == null)
        {
            listOfVol = listOfVol.OrderBy(v => v.Id);
        }
        else
        {
            string propertyName = feildToSort.ToString();
            var propertyInfo = typeof(BO.VolunteerInList).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                listOfVol = listOfVol.OrderBy(v => propertyInfo.GetValue(v, null));
            }
        }
        return listOfVol;
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
        double[]? latLon= volunteer.CurrentAddress==null?null:Tools.GetCoordinates(volunteer.CurrentAddress);
        _dal.Volunteer.Update(new DO.Volunteer
        {
            Id = volunteer.Id,
            Name = volunteer.Name,
            Email = volunteer.Email,
            Longitude = latLon?[1],
            Latitude = latLon?[0],
            Position = (DO.Position)volunteer.Position,
            Password =volunteer.Password,
            Active = volunteer.Active,
            CurrentAddress =latLon==null?null: volunteer.CurrentAddress,
            MaximumDistanceForReading = volunteer.MaximumDistanceForReading,
            TypeOfDistance = (DO.TypeOfDistance)volunteer.TypeOfDistance
        });
    }
}
