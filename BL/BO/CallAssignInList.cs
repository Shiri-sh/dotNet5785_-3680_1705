using Helpers;

namespace BO;
/// <summary>
/// Represents the assignment details of a volunteer for a treatment call.
/// </summary>
/// <param name="VolunteerId">The ID of the volunteer who is assigned to the treatment call. This can be NULL if no volunteer is assigned.</param>
/// <param name="VolunteerName">The name of the volunteer assigned to the treatment call. This can be NULL if no volunteer is assigned.</param>
/// <param name="TreatmentEntryTime">The time (date and time) when the treatment for the current call started.</param>
/// <param name="TreatmentEndTime">The time (date and time) when the volunteer finished handling the treatment call. This can be NULL if the treatment is ongoing.</param>
/// <param name="TypeOfTreatmentTermination">The type of treatment termination, indicating how the treatment was completed. This can be NULL if no termination type is defined.</param>
public class CallAssignInList
{
       public int? VolunteerId {  get; set; }
       public string? VolunteerName { get; set; }
       public DateTime TreatmentEntryTime {  get; set; }
       public DateTime? TreatmentEndTime {  get; set; }
       public TypeOfTreatmentTermination? TypeOfTreatmentTermination {  get; set; }
       public override string ToString() => this.ToStringProperty();

}
