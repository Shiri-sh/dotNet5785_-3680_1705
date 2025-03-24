using BlApi;
using Helpers;
namespace BlImplementation;

internal class CallImplementation : ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    //
    public void AddCall(BO.Call call)
    {
        CallManager.ValidateCall(call);
        DO.Call doCall = new(
            call.Id,
            (DO.KindOfCall)call.KindOfCall,
            call.AddressOfCall,
            call.Latitude,
            call.Longitude,
            call.OpeningTime,
            call.FinishTime,
            call.Description
        );
        try
        {
            _dal.Call.Create(doCall);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"call with ID={doCall.Id} already exists", ex);
        }
    }
    public int[] CallByStatus()
    {
        int[] sumCallByStatus = new int[5];
        var groupedCalls = CallList().GroupBy(x => (int)x.Status);
        for (int i = 0; i < 5; i++)
        {
            sumCallByStatus[i] = (from x in groupedCalls
                                  where x.Key == i
                                  select x.Count()).FirstOrDefault();
        }
        return sumCallByStatus;
    }
    public IEnumerable<BO.CallInList> CallList(BO.CallInListObjects? objFilter = null, object? filterBy = null, BO.CallInListObjects? objSort = null)
    {
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();
        var propertyInfo = typeof(DO.Call).GetProperty(objFilter.ToString());
        if (propertyInfo != null)
        {
            calls = from c in calls
                    where propertyInfo.GetValue(c, null) == filterBy
                    select c;
        }
        else
        {
            calls = from item in calls
                    select item;
        }

        var propertyInfoSort = typeof(DO.Call).GetProperty(objSort.ToString());

        if (propertyInfoSort != null)
        {
            calls = from c in calls
                    orderby propertyInfoSort.GetValue(c, null)
                    select c;
        }
        else
        {
            calls = from c in calls
                    orderby c.Id
                    select c;
        }
        return from c in calls
               let assignment = _dal.Assignment.Read(a => a.CalledId == c.Id)
               select new BO.CallInList
               {
                   Id = assignment.Id,
                   CallId = assignment.CalledId,
                   KindOfCall = (BO.KindOfCall)c.KindOfCall,
                   OpeningTime = c.OpeningTime,
                   RemainingTimeToFinish = c.FinishTime - ClockManager.Now,
                   LastVolunteer = _dal.Volunteer.Read(v => v.Id ==
                                                       _dal.Assignment.ReadAll(a => a.CalledId == c.Id)
                                                       .OrderByDescending(a => a.TreatmentEntryTime)
                                                       .Select(a => a.VolunteerId)
                                                       .FirstOrDefault())
                                                .Name,
                   CompletionTime = assignment.TreatmentEndTime == null ? null : assignment.TreatmentEndTime - c.OpeningTime,
                   Status = CallManager.GetStatus(c),
                   TotalAlocation = _dal.Assignment.ReadAll().Count(a => a.CalledId == _dal.Assignment.Read(a => a.CalledId == c.Id).Id),

               };
    }

    public void UpdateCancelCall(int volunteerId, int assignId)
    {
        DO.Assignment? doAssign;
        try
        {
            doAssign = _dal.Assignment.Read(a => a.Id == assignId);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"assignt with {assignId} does not exist",ex);
        }
        BO.Position volPosition=(BO.Position)_dal.Volunteer.Read(v=>v.Id==volunteerId).Position;
        if (volPosition != BO.Position.Managar && volunteerId != doAssign.VolunteerId)
        {
            throw new BO.BlNotAloudToDoException("only a managar can cancle a call or the volunteer that took the call");
        }
        if((doAssign.TypeOfTreatmentTermination==DO.TypeOfTreatmentTermination.Handled || doAssign.TypeOfTreatmentTermination==DO.TypeOfTreatmentTermination.CancellationExpired) && doAssign.TreatmentEndTime!=null) {
            throw new BO.BlNotAloudToDoException("you cant cancle a call if its alocation is open");
        }
        _dal.Assignment.Update(new DO.Assignment
        {
            Id = assignId,
            VolunteerId = volunteerId,
            CalledId = doAssign.CalledId,
            TreatmentEntryTime = doAssign.TreatmentEntryTime,
            TreatmentEndTime = ClockManager.Now,
            TypeOfTreatmentTermination = volPosition==BO.Position.Managar?DO.TypeOfTreatmentTermination.ConcellingAdministrator: DO.TypeOfTreatmentTermination.SelfCancellation,
        });
    }
    public void UpdateEndCall(int volunteerId, int assignId)
    {
        DO.Assignment? doAssign;
        try
        {
            doAssign = _dal.Assignment.Read(a => a.Id == assignId);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"assignt with {assignId} does not exist", ex);
        }
        if (doAssign.VolunteerId!=volunteerId)
        {
            throw new BO.BlNotAloudToDoException("only the volunteer that took the call can finish it");
        }
        if (doAssign.TypeOfTreatmentTermination==null && doAssign.TreatmentEndTime == null)
        {
            _dal.Assignment.Update(new DO.Assignment
            {
                Id = assignId,
                VolunteerId = volunteerId,
                CalledId = doAssign.CalledId,
                TreatmentEntryTime = doAssign.TreatmentEntryTime,
                TreatmentEndTime = ClockManager.Now,
                TypeOfTreatmentTermination = DO.TypeOfTreatmentTermination.Handled,
            });
        }
        else
        {
            throw new BO.BlNotAloudToDoException("you cant cancle a call if its alocation is open");
        }
    }
    public void CooseCall(int volunteerId, int callId)
    {
        DO.Call? call = _dal.Call.Read(c => c.Id == callId);
        DO.Assignment? assign = _dal.Assignment.Read(a => a.CalledId == callId && (a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled || a.TypeOfTreatmentTermination==null));
        if(assign!=null && assign.TypeOfTreatmentTermination==DO.TypeOfTreatmentTermination.CancellationExpired)
        {
            throw new BO.BlNotAloudToDoException("the call has been handled or someone took it already or the call is irelevant");
        }
        _dal.Assignment.Create(new DO.Assignment { CalledId = callId, VolunteerId = volunteerId, TreatmentEntryTime = ClockManager.Now });
    }
    public void DeleteCall(int id)
    {
        DO.Call? doCall;
        try
        {
            doCall = _dal.Call.Read(cal => cal.Id == id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"call with {id} does Not exist", ex);
        }
        if (CallManager.GetStatus(doCall) == BO.Status.Open && _dal.Assignment.ReadAll(a => a.CalledId == doCall.Id) == null)
        {
            _dal.Call.Delete(doCall.Id);
        }
        else
        {
            throw new BO.BlNotAloudToDoException("You cant delete the call since it is open or someone took it");
        }

    }
    public IEnumerable<BO.ClosedCallInList> GetCloseCallByVolunteer(int VolunteerId, BO.KindOfCall? kindOfCall = null, BO.CloseCallInListObjects? objCloseCall = null)
    {
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll();

        List<int> callOfVol = assignments.Where(a => a.VolunteerId == VolunteerId && a.TreatmentEndTime != null).Select(a => a.CalledId).ToList();
        calls = calls.Where(c => callOfVol.Contains(c.Id));

        if (kindOfCall.HasValue)
        {
            calls = calls.Where(c => c.KindOfCall == (DO.KindOfCall)kindOfCall.Value);
        }

        if (objCloseCall != null)
        {
            var propertyInfoSort = typeof(DO.Call).GetProperty(objCloseCall.ToString());
            calls = calls.OrderBy(c => propertyInfoSort.GetValue(c, null));
        }
        else
        {
            calls = calls.OrderBy(c => c.Id);
        }
        return from c in calls
               let assignment = _dal.Assignment.Read(a => a.CalledId == c.Id)
               select new BO.ClosedCallInList
               {
                   Id = c.Id,
                   KindOfCall = (BO.KindOfCall)c.KindOfCall,
                   AddressOfCall = c.AddressOfCall,
                   OpeningTime = c.OpeningTime,
                   TreatmentEntryTime = assignment.TreatmentEntryTime,
                   TreatmentEndTime = assignment?.TreatmentEndTime,
                   TypeOfTreatmentTermination = (BO.TypeOfTreatmentTermination)assignment.TypeOfTreatmentTermination,
               };
    }
    public IEnumerable<BO.OpenCallInList> GetOpenCallByVolunteer(int VolunteerId, BO.KindOfCall? kindOfCall = null, BO.OpenCallInListFields? objOpenCall = null)

    {
        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();
        IEnumerable<DO.Assignment> assignments = _dal.Assignment.ReadAll();
        DO.Volunteer? vol = _dal.Volunteer.Read(v=>v.Id==VolunteerId);

        List<int> callOfVol = assignments.Where(a => a.VolunteerId == VolunteerId && a.TreatmentEndTime == null).Select(a => a.CalledId).ToList();
        calls = calls.Where(c => callOfVol.Contains(c.Id));

        if (kindOfCall.HasValue)
        {
            calls = calls.Where(c => c.KindOfCall == (DO.KindOfCall)kindOfCall.Value);
        }

        if (objOpenCall != null)
        {
            var propertyInfoSort = typeof(DO.Call).GetProperty(objOpenCall.ToString());
            calls = calls.OrderBy(c => propertyInfoSort.GetValue(c, null));
        }
        else
        {
            calls = calls.OrderBy(c => c.Id);
        }

        return calls.Select(c => new BO.OpenCallInList
        {
            Id = c.Id,
            KindOfCall = (BO.KindOfCall)c.KindOfCall,
            AddressOfCall = c.AddressOfCall,
            OpeningTime = c.OpeningTime,
            FinishTime=c.FinishTime,
            Description=c.Description,
            DistanceFromVol=CallManager.GetDistanceFromVol(c.Latitude,c.Longitude,vol.Latitude,vol.Longitude)
        });
    }

    public BO.Call ReadCall(int id)
    {
        DO.Call? doCall;
        try
        {
            doCall = _dal.Call.Read(cal => cal.Id == id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"call with {id} does Not exist", ex);
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
            ListOfAlocation = _dal.Assignment.ReadAll(a => a.CalledId == id)
                                                                   .Select(a => new BO.CallAssignInList
                                                                   {
                                                                       VolunteerId = a.VolunteerId,
                                                                       VolunteerName = _dal.Volunteer.Read(v => v.Id == a.VolunteerId).Name,
                                                                       TreatmentEntryTime = a.TreatmentEntryTime,
                                                                       TreatmentEndTime = a.TreatmentEndTime,
                                                                       TypeOfTreatmentTermination = (BO.TypeOfTreatmentTermination)a.TypeOfTreatmentTermination
                                                                   }).ToList()
        };
    }
    public void UpdateCall(BO.Call call)
    {
        DO.Call? doCall;
        try
        {
            doCall = _dal.Call.Read(vol => vol.Id == call.Id);
            CallManager.ValidateCall(call);
            _dal.Call.Update(new DO.Call
            {
                Id = call.Id,
                KindOfCall = (DO.KindOfCall)call.KindOfCall,
                AddressOfCall = call.AddressOfCall,
                Latitude = call.Latitude,
                Longitude = call.Longitude,
                OpeningTime = call.OpeningTime,
                FinishTime = call.FinishTime,
                Description = call.Description,
            });
        }
        catch (DO.DalDoesNotExistException e)
        {
            throw new BO.BlDoesNotExistException($"Call with ID={call.Id} does Not exist",e);
        }
        catch(BO.BlInvalidDataException)
        {
            throw new BO.BlInvalidDataException("you put invalid data");
        }
    }
}
