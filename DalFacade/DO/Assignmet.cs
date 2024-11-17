namespace DO;
/// <summary>
/// Assignmet entity
/// </summary>
/// <param name="Id"> </param>
/// <param name="CalledId"></param>
/// <param name="volunteerId"></param>
/// <param name="Treatment_entry_time"></param>
/// <param name="Treatment_end_time"></param>
/// <param name="Type_of_treatment_termination"></param>
public enum Type_of_treatment_termination {Handled, Self_cancellation, Concelling_administrator, Cancellation_Expired }
public record Assignment
 (
    int Id,
    int CalledId,
    int VolunteerId,
    DateTime Treatment_entry_time,
    DateTime? Treatment_end_time=null,
    Type_of_treatment_termination? Type_of_treatment_termination=null

 )
{
    /// <summary>
    /// Default constructor for stage 3 
    /// </summary>
    public Assignment() : this(0,0,0,DateTime.Now){ }/*not sure about the DateTime.Now*/
}
