using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IAdmin
{
    DateTime GetClock();
    void UpdateClock(TypeOfTime typeOfTime);
    TimeSpan GetRiskRange();
    void UpdateRiskRange(TimeSpan riskRange);
    void Reset();
    void Initialization();

}
