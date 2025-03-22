//using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    internal static class CallManager
    {
        private static IDal s_dal = Factory.Get; //stage 4

        public static BO.StatusCallInProgress StatusCallInProgress(DO.Call call)
        {
            return ClockManager.Now + s_dal.Config.RiskRange > call.FinishTime ? BO.StatusCallInProgress.TreatInRisk : BO.StatusCallInProgress.Open;

        }
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

        internal static void ValidateCall(BO.Call call)
        {

            if (call.FinishTime < call.OpeningTime)
            {
                throw new BO.BlInvalidDataException("the finish-time cant be earlier than the opening time");
            }
            if (!Tools.IsValidAddress(call.Longitude, call.Latitude)) {
                throw new BO.BlInvalidDataException("Address not exist");
            };
            
        }

        internal static double GetDistanceFromVol(double? latitude1, double? longitude1, double latitude2, double longitude2)
        {
            throw new NotImplementedException();
        }
    }
}
