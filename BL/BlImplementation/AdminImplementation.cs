using BlApi;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class AdminImplementation:IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    public DateTime GetClock()
    {
        return ClockManager.Now;
    }

    public TimeSpan GetRiskRange()
    {
        return _dal.Config.RiskRange;
    }
    //אתחול
    public void Initialization()
    {
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);
    }

    public void Reset()
    {
        _dal.ResetDB();
        ClockManager.UpdateClock(ClockManager.Now);
    }

    public void UpdateClock(BO.TypeOfTime typeOfTime)
    {
     switch(typeOfTime)
     {
            case BO.TypeOfTime.Minute: ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1));break;
            case BO.TypeOfTime.Hour: ClockManager.UpdateClock(ClockManager.Now.AddHours(1)); break;
            case BO.TypeOfTime.Day: ClockManager.UpdateClock(ClockManager.Now.AddDays(1)); break;
            case BO.TypeOfTime.Month: ClockManager.UpdateClock(ClockManager.Now.AddMonths(1)); break;
            case BO.TypeOfTime.Year: ClockManager.UpdateClock(ClockManager.Now.AddYears(1)); break;
     };
    }
    public void UpdateRiskRange(TimeSpan riskRange)
    {
            _dal.Config.RiskRange = riskRange;
    }
}
