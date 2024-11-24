namespace DO;
/// <summary>
/// Call entity
/// </summary>
/// <param name="Id">Represents a number that uniquely identifies the call. </param>
/// <param name="KindOfCall">According to the type of specific system</param>
/// <param name="Description">Verbal description on the reading</param>
/// <param name="AddressOfCall">Full and real address in correct format, of the reading location.</param>
/// <param name="latitude"> a number indicating how far a point on the Earth is south or north of the line</param>
/// <param name="Longitude">a number indicating how far a point on Earth is east or west of the line</param>
/// <param name="OpeningTime">Time (date and time) when the call was opened by the administrator.</param>
/// <param name="FinishTime">Time (date and time) by which the reading should be closed.</param>


public record Call
(
    int Id,
    KindOfCall KindOfCall,
    string AddressOfCall,
    double Latitude,
    double Longitude,
    DateTime OpeningTime,
    DateTime? FinishTime = null,
    string? Description = null
)
{
    ///<summary>
    ///
    ///</summary>
    public Call() : this(0, KindOfCall.CableAssistance, "",0,0,DateTime.Now) { }
}

