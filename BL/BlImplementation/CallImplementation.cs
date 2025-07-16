using BlApi;
using Helpers;
namespace BlImplementation;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public async Task  AddCall(BO.Call call)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }

        double[]? latLon = call.AddressOfCall == null ? null : await Tools.GetCoordinates(call.AddressOfCall);
        if (latLon == null) {
             throw new BO.BlInvalidDataException("Address not exist");
        }
        try
        {
            CallManager.ValidateCall(call);
            lock (AdminManager.BlMutex)
                _dal.Call.Create(new(
                call.Id,
                (DO.KindOfCall)call.KindOfCall,
                call.AddressOfCall!,
                latLon[0],
                latLon[1],
                call.OpeningTime,
                call.FinishTime,
                call.Description
            ));
            CallManager.Observers.NotifyListUpdated();

        }
        catch (DO.DalAlreadyExistsException ex) { throw new BO.BlAlreadyExistsException($"call with ID={call.Id} already exists", ex); }
        catch (BO.BlInvalidDataException ex) { throw new BO.BlInvalidDataException(ex.Message); }
    }
    public int[] CallByStatus()
    {
        int[] sumCallByStatus = new int[6];
        lock (AdminManager.BlMutex)
        {
            var groupedCalls = CallList().GroupBy(x => (int)x.Status);
            foreach (var group in groupedCalls)
            {
                sumCallByStatus[group.Key] = group.Count();
            }
        }
        return sumCallByStatus;
    }
    public IEnumerable<BO.CallInList> CallList(BO.CallInListObjects? objFilter = null, object? filterBy = null, BO.CallInListObjects? objSort = null)
    {
        List<BO.CallInList> listOfCall=new List<BO.CallInList>();
        List<DO.Call> calls;
        lock (AdminManager.BlMutex)
           calls = _dal.Call.ReadAll().ToList();

        foreach(var c in calls)
        {

            lock (AdminManager.BlMutex)
            {
                var assignment = _dal.Assignment.ReadAll(a => a.CalledId == c.Id)
                                                        .OrderByDescending(a => a.TreatmentEntryTime)
                                                        .FirstOrDefault();
                listOfCall.Add(
                   new BO.CallInList
                   {
                       Id = assignment?.Id,
                       CallId = c.Id,
                       KindOfCall = (BO.KindOfCall)c.KindOfCall,
                       OpeningTime = c.OpeningTime,
                       RemainingTimeToFinish = c.FinishTime - AdminManager.Now,
                       LastVolunteer = assignment == null ? null : _dal.Volunteer.Read(v => v.Id == assignment.VolunteerId)?.Name,
                       CompletionTime = assignment == null ? null : assignment.TreatmentEndTime == null ? null : assignment.TreatmentEndTime - c.OpeningTime,
                       Status = CallManager.GetStatus(c),
                       TotalAlocation = _dal.Assignment.ReadAll(a => a.CalledId == c.Id).Count(),
                   });
            }
        }
        if (objFilter != null && filterBy != null)
        {
            var propertyInfo = typeof(BO.CallInList).GetProperty(objFilter.ToString()!);
            if (propertyInfo != null)
            {
                object convertedFilterBy = filterBy;

                // אם סוג המאפיין הוא enum או nullable enum – מבצעים המרה
                if (propertyInfo.PropertyType.IsEnum)
                {
                    convertedFilterBy = Enum.Parse(propertyInfo.PropertyType, filterBy.ToString()!);
                }
                else if (Nullable.GetUnderlyingType(propertyInfo.PropertyType)?.IsEnum == true)
                {
                    Type enumType = Nullable.GetUnderlyingType(propertyInfo.PropertyType)!;
                    convertedFilterBy = Enum.Parse(enumType!, filterBy.ToString()!);
                }

                listOfCall = listOfCall
                            .Where(c => propertyInfo.GetValue(c) != null &&
                                        propertyInfo.GetValue(c)!.Equals(convertedFilterBy))
                            .ToList();
            }
        }
        if (objSort != null)
        {
            var propertyInfoSort = typeof(BO.CallInList).GetProperty(objSort.ToString()!);
            if (propertyInfoSort != null)
            {
                listOfCall = listOfCall
                            .OrderBy(c => propertyInfoSort.GetValue(c))
                            .ToList();
            }
        }
        else
        {
            listOfCall = listOfCall
                        .OrderBy(c => c.Id)
                        .ToList();
        }

        return listOfCall;
    }
    public void UpdateCancelCall(int volunteerId, int assignId)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }
        CallManager.UpdateCancelCall(volunteerId, assignId);
    }
    public void UpdateEndCall(int volunteerId, int assignId)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }
        CallManager.UpdateEndCall(volunteerId, assignId);
    }
    public void CooseCall(int volunteerId, int callId)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }
        CallManager.CooseCall(volunteerId, callId); 
    }
    public void DeleteCall(int id)
    {
        AdminManager.ThrowOnSimulatorIsRunning();
         //

        DO.Call doCall;
        IEnumerable<DO.Assignment>? a;
        BO.Status st;
        lock (AdminManager.BlMutex)
        {
            doCall = _dal.Call.Read(cal => cal.Id == id) ?? throw new BO.BlDoesNotExistException($"call with {id} does Not exist");
            a = _dal.Assignment.ReadAll(a => a.CalledId == doCall.Id);
            st = CallManager.GetStatus(doCall);
        }
            
        if (st== BO.Status.Open &&  a.Count()==0)
        {
            lock (AdminManager.BlMutex)
                _dal.Call.Delete(doCall.Id);
            CallManager.Observers.NotifyListUpdated();
        }
        else
        {
            throw new BO.BlNotAloudToDoException("You cant delete the call since it isn't open or someone took it");
        }
      
    }
    public IEnumerable<BO.ClosedCallInList> GetCloseCallByVolunteer(int VolunteerId, BO.KindOfCall? kindOfCall = null, BO.CloseCallInListObjects? objCloseCall = null)
    {
        List<int> callOfVol;
        List<BO.ClosedCallInList> calls=new List<BO.ClosedCallInList>();
        lock (AdminManager.BlMutex)
            callOfVol = _dal.Assignment.ReadAll().Where(a => a.VolunteerId == VolunteerId && a.TreatmentEndTime != null).Select(a => a.CalledId).ToList();
        foreach(var c in _dal.Call.ReadAll().Where(c => callOfVol.Contains(c.Id)))
        {
            lock (AdminManager.BlMutex)
            {
                var assignment = _dal.Assignment.Read(a => a.CalledId == c.Id);
                calls.Add(
                    new BO.ClosedCallInList
                    {
                        Id = c.Id,
                        KindOfCall = (BO.KindOfCall)c.KindOfCall,
                        AddressOfCall = c.AddressOfCall,
                        OpeningTime = c.OpeningTime,
                        TreatmentEntryTime = assignment.TreatmentEntryTime,
                        TreatmentEndTime = assignment.TreatmentEndTime == null ? null : assignment.TreatmentEndTime,
                        TypeOfTreatmentTermination = assignment.TypeOfTreatmentTermination == null ? null : (BO.TypeOfTreatmentTermination)assignment.TypeOfTreatmentTermination,
                    }
                );
            }
        }
        if (kindOfCall.HasValue && kindOfCall!=BO.KindOfCall.None)
        {
            calls = calls.Where(c => c.KindOfCall == (BO.KindOfCall)kindOfCall.Value).ToList();
        }
        string propertyInfo = objCloseCall.ToString()!;
        var propertyInfoSort = typeof(BO.ClosedCallInList).GetProperty(propertyInfo);
        
        if (objCloseCall != null)
                calls = calls.OrderBy(c => propertyInfoSort!.GetValue(c, null)).ToList();         
        else
                calls = calls.OrderBy(c => c.Id).ToList();
        return calls;
    }
    public IEnumerable<BO.OpenCallInList> GetOpenCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall = null, BO.OpenCallInListFields? objOpenCall = null)
    {
        //כל ההקצאות הקימות
        DO.Volunteer vol;
        IEnumerable<DO.Call> listcalls;
        List<BO.OpenCallInList> calls=new List<BO.OpenCallInList>();
        lock (AdminManager.BlMutex)
        {
            vol = _dal.Volunteer.Read(v => v.Id == volunteerId)!;
            listcalls = _dal.Call.ReadAll();
        }
        foreach (var c in listcalls)
        {
            DO.Assignment assignment;
            lock (AdminManager.BlMutex)
            {
                //הקצאה טופלה או עבר זמנה 
                if (c.FinishTime > AdminManager.Now)
                {
                    assignment = _dal.Assignment.ReadAll(a => a.CalledId == c.Id)
                                                          .FirstOrDefault(a =>
                                                           a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled
                                                          || a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.CancellationExpired)!;
                    if (assignment == null)
                        calls.Add(
                            new BO.OpenCallInList
                            {
                                Id = c.Id,
                                KindOfCall = (BO.KindOfCall)c.KindOfCall,
                                AddressOfCall = c.AddressOfCall,
                                OpeningTime = c.OpeningTime,
                                FinishTime = c.FinishTime,
                                Description = c.Description,
                                DistanceFromVol = Tools.GetDistance(vol, c)
                            });
                }
               
            }
        }
        if (kindOfCall.HasValue)
        {
            calls = calls.Where(c => c.KindOfCall == (BO.KindOfCall)kindOfCall.Value).ToList();
        }
            if (objOpenCall != null)
            {
                var propertyInfoSort = typeof(BO.OpenCallInList).GetProperty(objOpenCall.ToString()!);
                calls = calls.OrderBy(c => propertyInfoSort!.GetValue(c, null)).ToList();
            }
            else
                calls = calls.OrderBy(c => c.Id).ToList();
        return calls;
    }
    public BO.Call ReadCall(int id)
    {
        DO.Call doCall;
        List<BO.CallAssignInList> callAssignInList=new List<BO.CallAssignInList>();
        lock (AdminManager.BlMutex) 
           doCall = _dal.Call.Read(cal => cal.Id == id) ??
           throw new BO.BlDoesNotExistException($"call with {id} does Not exist");
        foreach(var a in _dal.Assignment.ReadAll(a => a.CalledId == id))
        {
            lock (AdminManager.BlMutex)
            {
                callAssignInList.Add(
                    new BO.CallAssignInList
                    {
                        VolunteerId = a.VolunteerId,
                        VolunteerName = _dal.Volunteer.Read(v => v.Id == a.VolunteerId)?.Name,
                        TreatmentEntryTime = a.TreatmentEntryTime,
                        TreatmentEndTime = a.TreatmentEndTime == null ? null : a.TreatmentEndTime,
                        TypeOfTreatmentTermination = a.TypeOfTreatmentTermination.HasValue
                                                    ? (BO.TypeOfTreatmentTermination?)a.TypeOfTreatmentTermination.Value
                                                    : null
                    }
                );
            }
        }
        return new BO.Call
        {
            Id = id,
            KindOfCall = (BO.KindOfCall)doCall.KindOfCall,
            AddressOfCall = doCall.AddressOfCall,
            Latitude = doCall.Latitude,
            Longitude = doCall.Longitude,
            OpeningTime = doCall.OpeningTime,
            FinishTime = doCall.FinishTime,
            Description = doCall.Description,
            Status = CallManager.GetStatus(doCall),
            CallAssignInList = callAssignInList
        };
    }
    public async void UpdateCall(BO.Call call)
    {
        try
        {
            AdminManager.ThrowOnSimulatorIsRunning();
        }
        catch (BO.BLTemporaryNotAvailableException ex)
        {
            throw new BO.BLTemporaryNotAvailableException(ex.Message);
        }

        double[]? latLon = call.AddressOfCall == null ? null : await Tools.GetCoordinates(call.AddressOfCall);
        if (latLon == null)
        {
            throw new BO.BlInvalidDataException("Address not exist");
        }
        try
        {
            CallManager.ValidateCall(call);
            lock (AdminManager.BlMutex)
                _dal.Call.Update(new DO.Call
                {
                    Id = call.Id,
                    KindOfCall = (DO.KindOfCall)call.KindOfCall,
                    AddressOfCall = call.AddressOfCall!,
                    Latitude = latLon[0],
                    Longitude = latLon[1],
                    OpeningTime = call.OpeningTime,
                    FinishTime = call.FinishTime,
                    Description = call.Description,
                });
            CallManager.Observers.NotifyItemUpdated(call.Id);
            CallManager.Observers.NotifyListUpdated();

        }
        catch (DO.DalDoesNotExistException e)
        {
            throw new BO.BlDoesNotExistException($"Call with ID={call.Id} does Not exist",e);
        }
        catch(BO.BlInvalidDataException e)
        {
            throw new BO.BlInvalidDataException($"you put invalid data,{e.Message}");
        }
    }

    #region Stage 5
    public void AddObserver(Action listObserver) =>
    CallManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    CallManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    CallManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    CallManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5

}
