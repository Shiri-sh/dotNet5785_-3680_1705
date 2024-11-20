namespace DO;
/// <summary>
/// Assignmet entity
/// </summary>
/// <param name="Id">Represents a number that uniquely identifies the allocation entity.</param>
/// <param name="CalledId">Represents a number that identifies the call that the volunteer chose to handle</param>
/// <param name="VolunteerId">represents the ID of the volunteer who chose to take care of the reading</param>
/// <param name="TreatmentEntryTime">Time (date and time) when the current call was processed</param>
/// <param name="TreatmentEndTime">Time (date and time) when the current volunteer finished handling the current call.</param>
/// <param name="TypeOfTreatmentTermination">The manner in which the treatment of the current reading was completed by the current volunteer.</param>
public record Assignment
 (
    int Id,
    int CalledId,
    int VolunteerId,
    DateTime TreatmentEntryTime,
    DateTime? TreatmentEndTime=null,
    TypeOfTreatmentTermination? TypeOfTreatmentTermination = null
 )
{
    /// <summary>
    /// Default constructor for stage 3 
    /// </summary>
    public Assignment() : this(0,0,0,DateTime.Now){ }
}
