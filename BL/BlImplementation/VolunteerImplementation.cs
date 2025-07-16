using BlApi;
using Helpers;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using static Helpers.Tools;

namespace BlImplementation;

internal class VolunteerImplementation: IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(BO.Volunteer boVolunteer)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }

        try
        {
            VolunteerManager.ValidateVolunteer(boVolunteer);
            boVolunteer.Password = PasswordHasher.HashPassword(boVolunteer.Password!);
            DO.Volunteer doVolunteer =
            new(
                    boVolunteer.Id,
                    boVolunteer.Name,
                    boVolunteer.PhoneNumber,
                    boVolunteer.Email,
                    (DO.Position)boVolunteer.Position,
                    boVolunteer.Password!,
                    boVolunteer.Active,
                    boVolunteer.CurrentAddress,
                    null,null,
                    boVolunteer.MaximumDistanceForReading,
                    (DO.TypeOfDistance)boVolunteer.TypeOfDistance
        );
            lock (AdminManager.BlMutex)
                _dal.Volunteer.Create(doVolunteer);
            VolunteerManager.Observers.NotifyListUpdated();
            _ = VolunteerManager.UpdateCoordinatesForVolunteerAddressAsync(doVolunteer); //stage 7
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
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }catch(BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }
        //
        DO.Assignment? assignment;
        lock (AdminManager.BlMutex)
        
            assignment = _dal.Assignment.Read(assign => assign.VolunteerId == id);
            if (assignment == null)
            {
                try
                {
                    lock (AdminManager.BlMutex)
                        _dal.Volunteer.Delete(id);
                    VolunteerManager.Observers.NotifyListUpdated();

                }
                catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"Volunteer with {id} does Not exist"); }
            }
            else
                throw new BO.BlNotAloudToDoException($"A volunteer with assignments cannot be deleted.");
        
    }
    public BO.Position Login(int id, string password)
    {
        string hashedPassword = PasswordHasher.HashPassword(password);
        DO.Volunteer doVolunteer;
        lock (AdminManager.BlMutex)
             doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id && vol.Password == hashedPassword) ?? 
                throw new BO.BlDoesNotExistException($"Volunteer with Id ={id} and Password={password} does Not exist");//need tocreate it later
        return (BO.Position)doVolunteer.Position;
        
    }
    public BO.Volunteer Read(int id)
    {
        
            DO.Volunteer? doVolunteer;
            DO.Assignment? assignment;
        DO.Call? callInProgress;
            ICall call = new CallImplementation();
        lock (AdminManager.BlMutex)
        {
            doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id) ??
                     throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
             assignment = _dal.Assignment.Read(a => a.VolunteerId == id && a.TreatmentEndTime == null);
            callInProgress = assignment == null ? null : _dal.Call.Read(c => c.Id == assignment.CalledId);
        }
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
                CallInProgress = callInProgress != null ? new BO.CallInProgress
                {
                    Id = assignment.Id,
                    CallId = assignment.CalledId,
                    KindOfCall = (BO.KindOfCall)callInProgress.KindOfCall,
                    AddressOfCall = callInProgress.AddressOfCall,
                    OpeningTime = callInProgress.OpeningTime,
                    FinishTime = callInProgress.FinishTime,
                    Description = callInProgress.Description,
                    TreatmentEntryTime = assignment.TreatmentEntryTime,
                    DistanceFromVolunteer = Tools.GetDistance(doVolunteer, callInProgress),//////dont know if this good parameters
                    Status = CallManager.StatusCallInProgress(callInProgress)
                } : null
            };
        
    }
    public IEnumerable<BO.VolunteerInList> ReadAll(bool? activity = null, BO.VoluteerInListObjects? feildToSort = null, object? valueOfFilter=null)
    {
        IEnumerable<DO.Volunteer> volunteers;
        List<BO.VolunteerInList> listOfVol = new List<BO.VolunteerInList>();
        lock (AdminManager.BlMutex)
            volunteers = _dal.Volunteer.ReadAll();
            volunteers = activity == null ? volunteers.Select(item => item) : volunteers.Where(v => v.Active == activity);
        
        ICall call = new CallImplementation();
        foreach (var v in volunteers)
        {
            DO.Assignment assign;
                var closeCalls = call.GetCloseCallByVolunteer(v.Id);
            lock (AdminManager.BlMutex)
                assign = _dal.Assignment.Read(a => a.VolunteerId == v.Id && a.TreatmentEndTime == null)!;
               BO.KindOfCall? kindOfCall = assign != null ?  (BO.KindOfCall)_dal.Call.Read(a => a.Id == assign.CalledId)!.KindOfCall :null;
                listOfVol.Add(
                    new BO.VolunteerInList
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Active = v.Active,
                        SumCancledCalls = closeCalls.Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.SelfCancellation),
                        SumCaredCalls = closeCalls.Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.Handled),
                        SumIrelevantCalls = closeCalls.Count(c => c.TypeOfTreatmentTermination == BO.TypeOfTreatmentTermination.CancellationExpired),
                        IdOfCall = assign?.CalledId ?? null,
                        KindOfCall = kindOfCall
                    }
                    );
        }
        if (feildToSort == null)
        {
            return listOfVol.OrderBy(v => v.Id);
        }
        string propertyName = feildToSort.ToString()!;
        var propertyInfo = typeof(BO.VolunteerInList).GetProperty(propertyName);
        if (propertyInfo != null)
        {
            if (valueOfFilter == null)
                return listOfVol.OrderBy(v => propertyInfo.GetValue(v, null));
            else
                return listOfVol.Where(v =>
                {
                    var propValue = propertyInfo.GetValue(v, null);
                    return object.Equals(propValue, valueOfFilter);
                });

        }
        return listOfVol;
    }
    public void UpdateVolunteer(int id, BO.Volunteer volunteer)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }
        DO.Volunteer doVolunteer;
        DO.Volunteer voluRequest;
        //המתנדב קיים במערכת
        lock (AdminManager.BlMutex)
        {
             doVolunteer = _dal.Volunteer.Read(vol => vol.Id == volunteer.Id) ??
            throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");
             voluRequest = _dal.Volunteer.Read(vol => vol.Id == id) ??
                throw new BO.BlDoesNotExistException($"someone with ID={id} does Not exist");
        }
            //המעדכן הוא מנהל או האדם עצמו
            if (id != volunteer.Id && voluRequest.Position != DO.Position.Managar)
            {
                throw new BO.BlNotAloudToDoException("Only a managar can update a volunteer or the volunteer himself");
            }
            // בדיקת תקינות של הנתונים שהוזנו
            VolunteerManager.ValidateVolunteer(volunteer);
            //בדיקת שינויים בשדות מסוימים
            if ((volunteer.Position != (BO.Position)doVolunteer.Position || volunteer.Active != doVolunteer.Active) && voluRequest.Position != DO.Position.Managar)
            {
                throw new BO.BlNotAloudToDoException("Only a managar can update the volunteer's Position");
            }
            volunteer.Password = PasswordHasher.HashPassword(volunteer.Password!);
            DO.Volunteer vol=new (
            
                 volunteer.Id,
                 volunteer.Name,
                 volunteer.PhoneNumber,
                 volunteer.Email,
                //Longitude = latLon?[1],
                //Latitude = latLon?[0],
                (DO.Position)volunteer.Position,
                 volunteer.Password,
                 volunteer.Active,
                 volunteer.CurrentAddress,
                 null,null,
                 volunteer.MaximumDistanceForReading,
                 (DO.TypeOfDistance)volunteer.TypeOfDistance
            );

        lock (AdminManager.BlMutex)
            _dal.Volunteer.Update(vol);
        VolunteerManager.Observers.NotifyListUpdated();
        _ = VolunteerManager.UpdateCoordinatesForVolunteerAddressAsync(vol); //stage 7

        VolunteerManager.Observers.NotifyItemUpdated(volunteer.Id);
        VolunteerManager.Observers.NotifyListUpdated();
    }
    #region Stage 5
    public void AddObserver(Action listObserver) =>
    VolunteerManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    VolunteerManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    VolunteerManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    VolunteerManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5
}
