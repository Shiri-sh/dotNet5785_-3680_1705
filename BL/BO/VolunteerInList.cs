using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace BO;
/// <summary>
/// Represents a volunteer in the list, including their activity status and performance metrics.
/// </summary>
/// <param name="Id">The unique identifier of the volunteer.</param>
/// <param name="Name">The name of the volunteer.</param>
/// <param name="Active">Indicates whether the volunteer is currently active and available for calls.</param>
/// <param name="SumCancledCalls">The total number of calls the volunteer has canceled.</param>
/// <param name="SumCaredCalls">The total number of calls the volunteer has completed or cared for.</param>
/// <param name="SumIrelevantCalls">The total number of calls that were deemed irrelevant by the volunteer.</param>
/// <param name="IdOfCall">The identifier of the current call the volunteer is working on, if any. This can be NULL if no call is in progress.</param>
/// <param name="KindOfCall">The type of the current call the volunteer is working on (e.g., emergency, routine, etc.).</param>

public class VolunteerInList
{
    public int Id { get; init; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public int SumCancledCalls { get; set; }
    public int SumCaredCalls { get; set; }
    public int SumIrelevantCalls { get; set; }
    public int? IdOfCall {  get; set; }
    public KindOfCall KindOfCall { get; set; }
    public override string ToString() => this.ToStringProperty();
}

