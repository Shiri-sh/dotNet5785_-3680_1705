namespace DO;
/// <summary>
/// Call entity
/// </summary>
/// <param name="Id">Represents a number that uniquely identifies the call. </param>
/// <param name="Kind_of_call">According to the type of specific system</param>
/// <param name="Description">Verbal description on the reading</param>
/// <param name="Address_of_call">Full and real address in correct format, of the reading location.</param>
/// <param name="latitude"> a number indicating how far a point on the Earth is south or north of the line</param>
/// <param name="Longitude">a number indicating how far a point on Earth is east or west of the line</param>
/// <param name="Opening_time">Time (date and time) when the call was opened by the administrator.</param>
/// <param name="Finish_time">Time (date and time) by which the reading should be closed.</param>


public enum Kind_of_call { Rescue_Kid, change_wheel, First_aid, Cable_assistance , fuel_oil_water,  }
public record Call
(
    int Id,
    Kind_of_call Kind_of_call,
    string Description,
    string Address_of_call,
    double latitude,
    double Longitude,
    DateTime Opening_time,
    DateTime Finish_time

)
{
    ///<summary>
    ///
    ///</summary>
}

