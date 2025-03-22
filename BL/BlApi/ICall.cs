//using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ICall
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    int[] CallByStatus();
    IEnumerable<BO.CallInList> CallList(BO.CallInListObjects? objFilter=null, object? filterBy=null,BO.CallInListObjects? objSort=null);
    BO.Call ReadCall(int id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="call"></param>
    void UpdateCall(BO.Call call);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    void DeleteCall(int id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="call"></param>
    void AddCall(BO.Call call);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volunteerId"></param>
    /// <param name="kindOfCall"></param>
    /// <param name="objCloseCall"></param>
    /// <returns></returns>
    IEnumerable<BO.ClosedCallInList> GetCloseCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, BO.CloseCallInListObjects? objCloseCall=null);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volunteerId"></param>
    /// <param name="kindOfCall"></param>
    /// <param name="objCloseCall"></param>
    /// <returns></returns>
    IEnumerable<BO.OpenCallInList> GetOpenCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, BO.OpenCallInListFields? objCloseCall=null);
    void UpdateEndCall(int volunteerId, int callID);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volunteerId"></param>
    /// <param name="callID"></param>
    void UpdateCancelCall(int volunteerId, int callID);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volunteerId"></param>
    /// <param name="callID"></param>
    void CooseCall(int volunteerId, int callID);
}
