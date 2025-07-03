
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IAdmin
{
    DateTime GetClock();
    void UpdateClock(BO.TypeOfTime typeOfTime);
    TimeSpan GetRiskRange();
    void UpdateRiskRange(TimeSpan riskRange);
    void Reset();
    void Initialization();
    #region Stage 5
    void AddConfigObserver(Action configObserver);
    void RemoveConfigObserver(Action configObserver);
    void AddClockObserver(Action clockObserver);
    void RemoveClockObserver(Action clockObserver);

    #endregion Stage 5
    #region 7
    void StartSimulator(int interval); //stage 7
    void StopSimulator();
    void AddSimulatorStoppedObserver(Action observer);
    #endregion Stage 7

}
