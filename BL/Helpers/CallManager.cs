
using DalApi;
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
        return ClockManager.Now + s_dal.Config.RiskRange > call.FinishTime ? BO.StatusCallInProgress.TreatInRisk : BO.StatusCallInProgress.BeingCared;
    }
    /// <summary>
    /// Retrieves the status of a given call based on various conditions including its finish time and assignments.
    /// </summary>
    /// <param name="call">The call object whose status needs to be determined.</param>
    /// <returns>The status of the call (e.g., 'OpenInRisk', 'Closed', 'Irelavant').</returns>
    public static BO.Status GetStatus(DO.Call call)
    {
        IEnumerable<DO.Assignment> assignments= s_dal.Assignment.ReadAll(a => a.CalledId == call.Id);
        //הקריאה לא הוקצתה לאף מתנדב והיא בטווח סיכון
        if (assignments==null&& ClockManager.Now + s_dal.Config.RiskRange > call.FinishTime)
            return BO.Status.OpenInRisk;
        //מחפש אם יש הקצאה לקריאה שמטופלת עכשיו ולא עבר זמנה
       if (call.FinishTime > ClockManager.Now && null == assignments.FirstOrDefault(a=> a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.SelfCancellation && a.TypeOfTreatmentTermination != DO.TypeOfTreatmentTermination.CancellationExpired))
            return BO.Status.Open;
        //מתנדב סים לטפל בה
        if (null!=assignments.FirstOrDefault(a => a.TypeOfTreatmentTermination == DO.TypeOfTreatmentTermination.Handled))
            return BO.Status.Closed;
        //פג תוקף
        if (call.FinishTime < ClockManager.Now)
            return BO.Status.Irelavant;
        return (BO.Status)StatusCallInProgress(call);
    }
    /// <summary>
    /// Validates the call by checking if the finish time is not earlier than the opening time and if the address is valid.
    /// </summary>
    /// <param name="call">The call object to be validated.</param>
    /// <exception cref="BO.BlInvalidDataException">Thrown if the finish time is earlier than the opening time or if the address is invalid.</exception>
    internal static void ValidateCall(BO.Call call)
    {

            if (call.FinishTime < call.OpeningTime|| call.FinishTime<ClockManager.Now)
            {
                throw new BO.BlInvalidDataException("the finish-time cant be earlier than the opening time or passed ");
            }
            if (!Enum.IsDefined(typeof(BO.KindOfCall), call.KindOfCall))
            {
                throw new BO.BlInvalidDataException("Invalid call type.");
            }
        
    }
    internal static void UpdateOpenCallNotRelevant()
    {
        var calls = s_bl.Call.CallList();
        calls = calls.Where(c => c.CompletionTime == null && c.RemainingTimeToFinish == TimeSpan.Zero);
        foreach(var c in calls)
        {
            if (c.TotalAlocation == 0)
            {
                s_dal.Assignment.Create(new DO.Assignment
                {

                });
            }
            s_dal.Call.Update(new DO.Call
            {
                Id =(int)c.Id
                
                
            });
        }
       
    }
 }
