using BlApi;
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
        throw new NotImplementedException();
    }

    public TimeSpan GetRiskRange()
    {
        throw new NotImplementedException();
    }

    public void Initialization()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void UpdateClock(TypeOfTime typeOfTime)
    {
        throw new NotImplementedException();
    }

    public void UpdateRiskRange(TimeSpan riskRange)
    {
        throw new NotImplementedException();
    }
}
