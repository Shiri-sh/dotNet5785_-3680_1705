//using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ICall
{
    int[] CallByStatus();
    IEnumerable<BO.CallInList> CallList(CallInListObjects? objFilter=null, object? filterBy=null,CallInListObjects? objSort=null);
    BO.Call GetCall(int id);
    void UpdateCall(BO.Call call);
    void DeleteCall(int id);
    void AddCall(BO.Call call);
    IEnumerable<BO.ClosedCallInList> GetAllCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, CloseCallInListObjects? objCloseCall=null);
    void UpdateEndCall(int volunteerId, int callID);
    void CancelCall(int volunteerId, int callID);
    void CooseCall(int volunteerId, int callID);
}
