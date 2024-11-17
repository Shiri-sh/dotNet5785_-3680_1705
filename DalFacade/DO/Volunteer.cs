using DalApi;

namespace DO;
/// <summary>
/// Student Entity represents a student with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the volunteer (as in national id card)</param>
/// <param name="Name">Private and last Name of the volunteer</param>
/// <param name="PhoneNumber">volunterr's phone-number</param>
/// <param name="Email">volunterr's email-address</param>
/// <param name="Password">volunterr's secret Password to enter the system</param>
/// <param name="Current_Address">where the volunteer is currently at</param>
/// <param name="Latitude">A number indicating how far a point on Earth is south or north of the equator.</param>
/// <param name="Longitude">A number indicating how far a point on Earth is east or west of the equator.</param>
/// <param name="Position">A managar or a volunteer</param>
/// 
/// <param name="Maximum_distance_for_reading">Each volunteer will define through the display the maximum distance for receiving a call. will be able to choose a reading</param>
/// <param name="Type_of_distance">Aerial distance, walking distance, driving distance The default is air distance.</param>

public enum Position{Managar , volunteer}
public enum Type_of_distance { Aerial , walking , driving }
public record  Volunteer
 (
    int Id,
    string Name,
    string PhoneNumber,
    string Email,
    Position Position,
    bool active,
    string? Password= null,
    string? Current = null,
    double? Latitude=null,
    double? Longitude = null,
    double? Maximum_distance_for_reading=null,
    Type_of_distance Type_of_distance = Type_of_distance.Aerial
 )
{

    /// <summary>
    /// Default constructor for stage 3 
    /// </summary>
    public Volunteer() : this(0,"","","", Position.volunteer,true) { }/*not sure about the position(ENUM) value*/
}
