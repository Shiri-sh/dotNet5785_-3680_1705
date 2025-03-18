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
            return ClockManager.Now + s_dal.Config.RiskRange > call.FinishTime ? BO.StatusCallInProgress.OpenInRisk : BO.StatusCallInProgress.Open;

        }
    }
}
