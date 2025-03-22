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
    /// Retrieves the count of calls grouped by their status.
    /// </summary>
    /// <returns>An array containing the count of calls for each status.</returns>
    int[] CallByStatus();

    /// <summary>
    /// Retrieves a list of calls with optional filtering and sorting.
    /// </summary>
    /// <param name="objFilter">Filter property.</param>
    /// <param name="filterBy">Filter value.</param>
    /// <param name="objSort">Sorting property.</param>
    /// <returns>A list of calls.</returns>
    IEnumerable<BO.CallInList> CallList(BO.CallInListObjects? objFilter=null, object? filterBy=null,BO.CallInListObjects? objSort=null);
    BO.Call ReadCall(int id);
    /// <summary>
    /// Updates an existing call.
    /// </summary>
    /// <param name="call">The call object with updated details.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the call does not exist.</exception>
    /// <exception cref="BO.BlInvalidDataException">Thrown if the call data is invalid.</exception>

    void UpdateCall(BO.Call call);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    void DeleteCall(int id);
    /// <summary>
    /// Adds a new call to the system.
    /// </summary>
    /// <param name="call">The call object to add.</param>
    /// <exception cref="BO.BlAlreadyExistsException">Thrown if the call already exists.</exception>
    void AddCall(BO.Call call);

    IEnumerable<BO.ClosedCallInList> GetCloseCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, BO.CloseCallInListObjects? objCloseCall=null);

    IEnumerable<BO.OpenCallInList> GetOpenCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, BO.OpenCallInListFields? objCloseCall=null);
    void UpdateEndCall(int volunteerId, int callID);
    void UpdateCancelCall(int volunteerId, int callID);
    void CooseCall(int volunteerId, int callID);
}
