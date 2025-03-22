using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Represents the details of a call in the list, including its status, remaining time, and associated volunteer.
    /// </summary>
    /// <param name="Id">The unique identifier of the call. This can be NULL if not assigned.</param>
    /// <param name="CallId">The identifier of the specific call.</param>
    /// <param name="KindOfCall">The type of call, represented by an enum.</param>
    /// <param name="OpeningTime">The date and time when the call was opened or created.</param>
    /// <param name="RemainingTimeToFinish">The remaining time to finish the call. This can be NULL if there is no time constraint.</param>
    /// <param name="LastVolunteer">The name of the last volunteer who worked on the call. This can be NULL if no volunteer has been assigned.</param>
    /// <param name="CompletionTime">The time taken to complete the call. This can be NULL if the call is ongoing or not completed.</param>
    /// <param name="Status">The current status of the call, represented by an enum.</param>
    /// <param name="TotalAlocation">The total number of times the call has been allocated or worked on.</param>
    public class CallInList
    {
        public int?  Id {get;init; }
        public int CallId { get;init;}
        public KindOfCall KindOfCall { get;set;}
        public DateTime OpeningTime { get; set; }
        public TimeSpan? RemainingTimeToFinish {  get; set; }
        public string? LastVolunteer {  get; set; }
        public TimeSpan? CompletionTime { get; set; }
        public Status Status { get; set; }
        public int TotalAlocation {  get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
