using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class CallImplementation: ICall
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddCall(Call call)
    {
        throw new NotImplementedException();
    }

    public int[] CallByStatus()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CallInList> CallList(CallInListObjects? objFilter = null, object? filterBy = null, CallInListObjects? objSort = null)
    {
        throw new NotImplementedException();
    }

    public void CancelCall(int volunteerId, int callID)
    {
        throw new NotImplementedException();
    }

    public void CooseCall(int volunteerId, int callID)
    {
        throw new NotImplementedException();
    }

    public void DeleteCall(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ClosedCallInList> GetAllCallByVolunteer(int volunteerId, KindOfCall? kindOfCall = null, CloseCallInListObjects? objCloseCall = null)
    {
        throw new NotImplementedException();
    }

    public Call GetCall(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateCall(Call call)
    {
        throw new NotImplementedException();
    }

    public void UpdateEndCall(int volunteerId, int callID)
    {
        throw new NotImplementedException();
    }
}
