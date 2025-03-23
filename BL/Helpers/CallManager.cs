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

            if (call.FinishTime < call.OpeningTime)
            {
                throw new BO.BlInvalidDataException("the finish-time cant be earlier than the opening time");
            }
            if (!Tools.IsValidAddress(call.Longitude, call.Latitude)) {
                throw new BO.BlInvalidDataException("Address not exist");
            };
            
        }
        /// <summary>
        /// Calculates the distance between the call's location and a volunteer's location using the Haversine formula.
        /// </summary>
        /// <param name="laCall">Latitude of the call's location.</param>
        /// <param name="lonCall">Longitude of the call's location.</param>
        /// <param name="latVol">Latitude of the volunteer's location.</param>
        /// <param name="lonVol">Longitude of the volunteer's location.</param>
        /// <returns>The distance in kilometers between the call's location and the volunteer's location.</returns>
        internal static double GetDistanceFromVol(double laCall, double lonCall, double? latVol, double? lonVol)
        {
            const double R = 6371; // רדיוס כדור הארץ בק"מ
            double dLat =DegreesToRadians(laCall - latVol);
            double dLon = DegreesToRadians(lonCall - lonVol);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(latVol)) * Math.Cos(DegreesToRadians(laCall)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
