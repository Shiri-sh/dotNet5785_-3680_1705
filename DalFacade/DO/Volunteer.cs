using DalApi;

namespace DO;
/// <summary>
/// Student Entity represents a student with all its props
/// </summary>
#region properties
/// <param name="Id">Personal unique ID of the volunteer (as in national id card)</param>
/// <param name="Name">Private and last Name of the volunteer</param>
/// <param name="PhoneNumber">volunterr's phone-number</param>
/// <param name="Email">volunterr's email-address</param>
/// <param name="Password">volunterr's secret Password to enter the system</param>
/// <param name="CurrentAddress">where the volunteer is currently at</param>
/// <param name="Latitude">A number indicating how far a point on Earth is south or north of the equator.</param>
/// <param name="Longitude">A number indicating how far a point on Earth is east or west of the equator.</param>
/// <param name="Position">A managar or a volunteer</param>
/// <param name="MaximumDistanceForReading">Each volunteer will define through the display the maximum distance for receiving a call. will be able to choose a reading</param>
/// <param name="active"></param>
/// <param name="TypeOfDistance">Aerial distance, walking distance, driving distance The default is air distance.</param>

#endregion

public record  Volunteer
 (
    int Id,
    string Name,
    string PhoneNumber,
    string Email,
    Position Position,
    string Password,
    bool active = false,
    string? CurrentAddress = null,
    double? Latitude = null,
    double? Longitude = null,
    double? MaximumDistanceForReading = null,
    TypeOfDistance TypeOfDistance = TypeOfDistance.Aerial
 )
{
    /// <summary>
    /// Default constructor for stage 3 
    /// </summary>
    public Volunteer() : this(0,"","","", Position.Volunteer,true) { }
}
