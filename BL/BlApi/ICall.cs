//using BO;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ICall: IObservable
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
    /// <summary>
    /// Retrieves the details of a specific call by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the call to retrieve.</param>
    /// <returns>The details of the call, including its status, assignments, and other relevant information.</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the call with the given ID does not exist.</exception>
    BO.Call ReadCall(int id);

    /// <summary>
    /// Updates an existing call.
    /// </summary>
    /// <param name="call">The call object with updated details.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the call does not exist.</exception>
    /// <exception cref="BO.BlInvalidDataException">Thrown if the call data is invalid.</exception>
    void UpdateCall(BO.Call call);
    /// <summary>
    /// Deletes a call if it's open and not assigned to any volunteer.
    /// </summary>
    /// <param name="id">The unique identifier of the call to be deleted.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the call with the given ID does not exist.</exception>
    /// <exception cref="BO.BlNotAloudToDoException">Thrown if the call is open or already assigned to a volunteer and cannot be deleted.</exception>
    void DeleteCall(int id);
    /// <summary>
    /// Adds a new call to the system.
    /// </summary>
    /// <param name="call">The call object to add.</param>
    /// <exception cref="BO.BlAlreadyExistsException">Thrown if the call already exists.</exception>
    void AddCall(BO.Call call);
    /// <summary>
    /// Retrieves a list of closed calls assigned to a volunteer.
    /// </summary>
    /// <param name="VolunteerId">The unique identifier of the volunteer.</param>
    /// <param name="kindOfCall">The optional kind of call filter to apply (e.g., emergency, routine, etc.).</param>
    /// <param name="objCloseCall">Optional field to sort the results by a specific property.</param>
    /// <returns>A collection of closed calls assigned to the volunteer.</returns>
    IEnumerable<BO.ClosedCallInList> GetCloseCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, BO.CloseCallInListObjects? objCloseCall=null);
    /// <summary>
    /// Retrieves a list of open calls that the volunteer can choose from.
    /// </summary>
    /// <param name="VolunteerId">The unique identifier of the volunteer.</param>
    /// <param name="kindOfCall">The optional kind of call filter to apply (e.g., emergency, routine, etc.).</param>
    /// <param name="objOpenCall">Optional field to sort the results by a specific property.</param>
    /// <returns>A collection of open calls assigned to the volunteer.</returns>
    IEnumerable<BO.OpenCallInList> GetOpenCallByVolunteer(int volunteerId, BO.KindOfCall? kindOfCall=null, BO.OpenCallInListFields? objCloseCall=null);
    /// <summary>
    /// Ends a call assignment by marking it as handled.
    /// </summary>
    /// <param name="volunteerId">The unique identifier of the volunteer requesting to end the call.</param>
    /// <param name="assignId">The unique identifier of the assignment to end.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the assignment with the given ID does not exist.</exception>
    /// <exception cref="BO.BlNotAloudToDoException">Thrown if the volunteer is not the one who took the call or if the assignment has already been completed.</exception>
    void UpdateEndCall(int volunteerId, int callID);
    /// <summary>
    /// Cancels a call assignment either by the volunteer who took the call or a manager.
    /// </summary>
    /// <param name="volunteerId">The unique identifier of the volunteer requesting to cancel the call.</param>
    /// <param name="assignId">The unique identifier of the assignment to cancel.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the assignment with the given ID does not exist.</exception>
    /// <exception cref="BO.BlNotAloudToDoException">Thrown if the volunteer is not the one who took the call or if the volunteer is not a manager.</exception>
    void UpdateCancelCall(int volunteerId, int callID);
    /// <summary>
    /// Allows a volunteer to choose a call to handle. Assigns the call to the volunteer.
    /// </summary>
    /// <param name="volunteerId">The unique identifier of the volunteer choosing the call.</param>
    /// <param name="callId">The unique identifier of the call the volunteer is choosing to handle.</param>
    /// <exception cref="BO.BlNotAloudToDoException">Thrown if the call has already been handled, is irrelevant, or is taken by someone else.</exception>
    void CooseCall(int volunteerId, int callID);
}
