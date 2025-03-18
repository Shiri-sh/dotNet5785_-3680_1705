using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        int[] sumCallByStatus = new int[5];
        var groupedCalls = CallList().GroupBy(x => (int)x.Status);
        for (int i = 0; i < 5; i++)
        {
            sumCallByStatus[i]= groupedCalls.Where(x=>x.Key==i).Select(x=>x.Count()).FirstOrDefault();
        }
        return sumCallByStatus;
    }

    public IEnumerable<CallInList> CallList(CallInListObjects? objFilter = null, object? filterBy = null, CallInListObjects? objSort = null)
    {

        IEnumerable<DO.Call> calls = _dal.Call.ReadAll();
        calls = filterBy == null ? calls.Select(item => item) : calls.Where(v => v.Active == filterBy);

        if (objSort == null)
        {
            calls = calls.OrderBy(v => v.Id);
        }

        string propertyName = feildToSort.ToString();
        var propertyInfo = typeof(DO.Volunteer).GetProperty(propertyName);

        if (propertyInfo != null)
        {
            calls = calls.OrderBy(v => propertyInfo.GetValue(v, null));
        }

        ICall call = new CallImplementation();

        return calls.Select(c => new BO.CallInList
        {
            Id =,
            CallId =c.Id ,
            KindOfCall = ,
            OpeningTime = ,
            RemainingTimeToFinish =,
            LastVolunteer =,
            CompletionTime = ,
            Status = ,
            TotalAlocation=
        });
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
