namespace DO;
/// <summary>
/// Assignmet entity
/// </summary>
/// <param name="Id"> </param>
/// <param name="CalledId"></param>
/// <param name="volunteerId"></param>
/// <param name="TreatmentEntryTime"></param>
/// <param name="TreatmentEndTime"></param>
/// <param name="TypeOfTreatmentTermination"></param>
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
