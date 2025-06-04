
namespace BlApi;

public interface IVolunteer: IObservable
{
    /// <summary>
    /// Logs in to the system.
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="password">Password</param>
    /// <returns>Volunteer position</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the volunteer is not found</exception>
    BO.Position Login(int id, string password);
    /// <summary>
    /// Reads all volunteers with optional filters and sorting.
    /// </summary>
    /// <param name="activity">Filter by active status.</param>
    /// <param name="feildToSort">Field to sort by.</param>
    /// <returns>A list of volunteers.</returns>
    IEnumerable<BO.VolunteerInList>ReadAll(bool? activity=null, BO.VoluteerInListObjects? objectToSort=null,object? valueOfVilter=null);
    /// <summary>
    /// Reads a volunteer by ID.
    /// </summary>
    /// <param name="id">Volunteer ID</param>
    /// <returns>Volunteer object</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the volunteer is not found</exception>
    BO.Volunteer Read(int id);
    /// <summary>
    /// Updates a volunteer's information.
    /// </summary>
    /// <param name="id">The volunteer ID.</param>
    /// <param name="volunteer">The updated volunteer details.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the volunteer does not exist.</exception>
    /// <exception cref="BO.BlNotAloudToDoException">Thrown if the update is not authorized.</exception>
    void UpdateVolunteer(int id, BO.Volunteer volunteer);
    /// <summary>
    /// Deletes a volunteer from the system.
    /// </summary>
    /// <param name="id">Volunteer ID</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the volunteer does not exist</exception>
    void DeleteVolunteer(int id);
    /// <summary>
    /// Adds a new volunteer to the system.
    /// </summary>
    /// <param name="boVolunteer">Volunteer details to be added</param>
    /// <exception cref="BO.BlAlreadyExistsException">Thrown if the volunteer already exists</exception>
    void AddVolunteer(BO.Volunteer boVolunteer);

}
