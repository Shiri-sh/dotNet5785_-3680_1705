using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace BO;
/// <summary>
/// Represents the details of a call that is currently in progress.
/// </summary>
/// <param name="Id">The unique identifier of the call in progress.</param>
/// <param name="CallId">The identifier of the specific call.</param>
/// <param name="KindOfCall">The type of call, represented by an enum (e.g., emergency, routine, etc.).</param>
/// <param name="AddressOfCall">The address where the call is taking place. This is a required field.</param>
/// <param name="OpeningTime">The date and time when the call was opened or initiated.</param>
/// <param name="FinishTime">The date and time when the call was finished. This can be NULL if the call is still in progress.</param>
/// <param name="Description">A description of the call, such as additional details. This can be NULL if no description is provided.</param>
/// <param name="TreatmentEntryTime">The time when the treatment for the current call started.</param>
/// <param name="DistanceFromVolunteer">The distance from the volunteer to the location of the call, measured in kilometers or other units.</param>
/// <param name="Status">The current status of the call in progress, represented by an enum.</param>

public class CallInProgress
{
    public int Id { get; init; }
    public int CallId { get; init; }
    public KindOfCall KindOfCall { get; set; }
    public required string AddressOfCall { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime? FinishTime { get; set; }
    public string? Description { get; set; }
    public DateTime TreatmentEntryTime { get; set; }
    public double DistanceFromVolunteer { get; set; }
    public StatusCallInProgress Status { get; set; }
    public override string ToString() => this.ToStringProperty();
}