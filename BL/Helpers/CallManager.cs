
using DalApi;
using System.Collections.Generic;
namespace Helpers;
internal static class CallManager
{
    internal static ObserverManager Observers = new(); //stage 5 

    private static IDal s_dal = Factory.Get; //stage 4
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Determines the current status of a call in progress based on the finish time and the current risk range.
    /// </summary>
    /// <param name="call">The call object whose status needs to be determined.</param>
    /// <returns>The status of the call in progress (either 'TreatInRisk' or 'Open').</returns>
    public static BO.StatusCallInProgress StatusCallInProgress(DO.Call call)
    {
        lock (AdminManager.BlMutex)
            return AdminManager.Now + s_dal.Config.RiskRange > call.FinishTime ? BO.StatusCallInProgress.TreatInRisk : BO.StatusCallInProgress.BeingCared;
    }
    /// <summary>
    /// Retrieves the status of a given call based on various conditions including its finish time and assignments.
    /// </summary>
    /// <param name="call">The call object whose status needs to be determined.</param>
    /// <returns>The status of the call (e.g., 'OpenInRisk', 'Closed', 'Irelavant').</returns>
    public static BO.Status GetStatus(DO.Call call)
    {
        IEnumerable<DO.Assignment> assignments;
        lock (AdminManager.BlMutex)
            assignments = s_dal.Assignment.ReadAll(a => a.CalledId == call.Id);
        //מתנדב סים לטפל בה
        if (null != assignments!.FirstOrDefault(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled))
            return BO.Status.Closed;
        //פג תוקף
        if (call.FinishTime < AdminManager.Now)
            return BO.Status.Irelavant;


        //הקריאה לא הוקצתה לאף מתנדב והיא בטווח סיכון
        if (null==assignments!.FirstOrDefault(a => a.TypeOfTreatmentTermination == null))
        {
            if (AdminManager.Now + s_dal.Config.RiskRange > call.FinishTime)
                return BO.Status.OpenInRisk;
            else
                return BO.Status.Open;

        }
            
            return (BO.Status)StatusCallInProgress(call);
        
    }
    /// <summary>
    /// Validates the call by checking if the finish time is not earlier than the opening time and if the address is valid.
    /// </summary>
    /// <param name="call">The call object to be validated.</param>
    /// <exception cref="BO.BlInvalidDataException">Thrown if the finish time is earlier than the opening time or if the address is invalid.</exception>
    internal static void ValidateCall(BO.Call call)
    {

            if (call.FinishTime < call.OpeningTime|| call.FinishTime<AdminManager.Now)
            {
                throw new BO.BlInvalidDataException("the finish-time cant be earlier than the opening time or passed ");
            }
            if (!Enum.IsDefined(typeof(BO.KindOfCall), call.KindOfCall))
            {
                throw new BO.BlInvalidDataException("Invalid call type.");
            }
        
    }
    internal static void UpdateOpenCallNotRelevant(DateTime clock)
    {
        List<BO.CallInList> calls;
        lock (AdminManager.BlMutex)
             calls = s_bl.Call.CallList().Where(c => c.CompletionTime == null && c.RemainingTimeToFinish == TimeSpan.Zero).ToList();
        foreach(var c in calls)
        {
            if (c.TotalAlocation == 0)
            {
                lock (AdminManager.BlMutex)
                    s_dal.Assignment.Create(new DO.Assignment
                {
                    Id = 0,
                    CalledId = c.CallId,
                    VolunteerId = 0,
                    TreatmentEntryTime = clock,
                    TreatmentEndTime = clock,
                    TypeOfTreatmentTermination = DO.TypeOfTreatmentTermination.CancellationExpired
                });
            }
            else
            {
                DO.Assignment na;
                lock (AdminManager.BlMutex)
                {
                     na = s_dal.Assignment.Read(nc => nc.Id == c.Id)!;
                    s_dal.Assignment.Update(new DO.Assignment
                    {
                        Id = na.Id,
                        CalledId = c.CallId,
                        VolunteerId = na.VolunteerId,
                        TreatmentEntryTime = na.TreatmentEntryTime,
                        TreatmentEndTime = clock,
                        TypeOfTreatmentTermination = DO.TypeOfTreatmentTermination.CancellationExpired
                    });
                }
                    Observers.NotifyItemUpdated(na.Id); //stage 5
                
            }
        }
            Observers.NotifyListUpdated(); //stage 5

    }
    public static async Task UpdateCoordinatesForCallAddressAsync(DO.Call call)
    {
        if (call.AddressOfCall is not null)
        {
            double[]? loc = await Tools.GetCoordinates(call.AddressOfCall);
            if (loc is not null)
            {
                call = call with { Latitude = loc[0], Longitude = loc[1] };
                lock (AdminManager.BlMutex)
                    s_dal.Call.Update(call);
                Observers.NotifyListUpdated();
                Observers.NotifyItemUpdated(call.Id);
            }

        }
    }

    public static void UpdateCancelCall(int volunteerId, int assignId)
    {

        DO.Assignment doAssign;
        BO.Position volPosition;
        lock (AdminManager.BlMutex)
        {
            doAssign = s_dal.Assignment.Read(a => a.Id == assignId) ??
            throw new BO.BlDoesNotExistException($"assignt with {assignId} does not exist");
            volPosition = (BO.Position)s_dal.Volunteer.Read(v => v.Id == volunteerId)!.Position;
        }

        if (volPosition != BO.Position.Managar && volunteerId != doAssign.VolunteerId)
        {
            throw new BO.BlNotAloudToDoException("only a managar can cancle a call or the volunteer that took the call");
        }
        if ((doAssign.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled || doAssign.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.CancellationExpired) && doAssign.TreatmentEndTime != null)
        {
            throw new BO.BlNotAloudToDoException("you cant cancle a call if its alocation is open");
        }
        lock (AdminManager.BlMutex)//stage 7
            s_dal.Assignment.Update(new DO.Assignment
            {
                Id = assignId,
                VolunteerId = volunteerId,
                CalledId = doAssign.CalledId,
                TreatmentEntryTime = doAssign.TreatmentEntryTime,
                TreatmentEndTime = AdminManager.Now,
                TypeOfTreatmentTermination = volPosition == BO.Position.Managar ? DO.TypeOfTreatmentTermination.ConcellingAdministrator : DO.TypeOfTreatmentTermination.SelfCancellation,
            });
        CallManager.Observers.NotifyItemUpdated(doAssign.CalledId);
        CallManager.Observers.NotifyListUpdated();
        VolunteerManager.Observers.NotifyItemUpdated(doAssign.VolunteerId);
        VolunteerManager.Observers.NotifyListUpdated();
    }
    public static void UpdateEndCall(int volunteerId, int assignId)
    {

        DO.Assignment doAssign;
        lock (AdminManager.BlMutex)
            doAssign = s_dal.Assignment.Read(a => a.Id == assignId) ??
            throw new BO.BlDoesNotExistException($"assignt with {assignId} does not exist");
        if (doAssign.VolunteerId != volunteerId)
        {
            throw new BO.BlNotAloudToDoException("only the volunteer that took the call can finish it");
        }
        if (doAssign.TypeOfTreatmentTermination == null && doAssign.TreatmentEndTime == null)
        {
            lock (AdminManager.BlMutex)
                s_dal.Assignment.Update(new DO.Assignment
                {
                    Id = assignId,
                    VolunteerId = volunteerId,
                    CalledId = doAssign.CalledId,
                    TreatmentEntryTime = doAssign.TreatmentEntryTime,
                    TreatmentEndTime = AdminManager.Now,
                    TypeOfTreatmentTermination = DO.TypeOfTreatmentTermination.Handled,
                });
            CallManager.Observers.NotifyItemUpdated(assignId);
            CallManager.Observers.NotifyListUpdated();
            VolunteerManager.Observers.NotifyItemUpdated(doAssign.VolunteerId);
            VolunteerManager.Observers.NotifyListUpdated();
        }
        else
        {
            throw new BO.BlNotAloudToDoException("you cant comlete a call if its alocation is closed");
        }


    }
    public static void CooseCall(int volunteerId, int callId)
    {

        DO.Call? call;
        DO.Assignment? assign;
        lock (AdminManager.BlMutex)
        {
            call = s_dal.Call.Read(c => c.Id == callId);
            assign = s_dal.Assignment.Read(a => a.CalledId == callId && (a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled || a.TypeOfTreatmentTermination == null));
        }
        if (assign != null && assign.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.CancellationExpired)
        {
            throw new BO.BlNotAloudToDoException("the call has been handled or someone took it already or the call is irelevant");
        }
        lock (AdminManager.BlMutex)
            s_dal.Assignment.Create(new DO.Assignment { CalledId = callId, VolunteerId = volunteerId, TreatmentEntryTime = AdminManager.Now });
        VolunteerManager.Observers.NotifyItemUpdated(volunteerId);
        VolunteerManager.Observers.NotifyListUpdated();
        CallManager.Observers.NotifyItemUpdated(callId);
        CallManager.Observers.NotifyListUpdated();
    }

}
