using BlApi;
using DalApi;
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
       => AdminManager.Now;

    public TimeSpan GetRiskRange()
    => AdminManager.RiskRange;
    public int GetNextCallId()
      => AdminManager.NextCallId;
    public int GetNextAssignmentId()
       => AdminManager.NextAssignmentId;
    public void UpdateRiskRange(TimeSpan riskRange) => AdminManager.RiskRange = riskRange;

    //אתחול
    public void Initialization()
    {
        AdminManager.InitializeDB();
    }

    public void Reset()
    {
        AdminManager.ResetDB();
    }

    public void UpdateClock(BO.TypeOfTime typeOfTime)
    {
     switch(typeOfTime)
     {
            case BO.TypeOfTime.Minute: AdminManager.UpdateClock(AdminManager.Now.AddMinutes(1));break;
            case BO.TypeOfTime.Hour: AdminManager.UpdateClock(AdminManager.Now.AddHours(1)); break;
            case BO.TypeOfTime.Day: AdminManager.UpdateClock(AdminManager.Now.AddDays(1)); break;
            case BO.TypeOfTime.Month: AdminManager.UpdateClock(AdminManager.Now.AddMonths(1)); break;
            case BO.TypeOfTime.Year: AdminManager.UpdateClock(AdminManager.Now.AddYears(1)); break;
     };
    }
    #region Stage 5
    public void AddClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers += clockObserver;
    public void RemoveClockObserver(Action clockObserver) =>
    AdminManager.ClockUpdatedObservers -= clockObserver;
    public void AddConfigObserver(Action configObserver) =>
   AdminManager.ConfigUpdatedObservers += configObserver;
    public void RemoveConfigObserver(Action configObserver) =>
    AdminManager.ConfigUpdatedObservers -= configObserver;
    #endregion Stage 5
}
