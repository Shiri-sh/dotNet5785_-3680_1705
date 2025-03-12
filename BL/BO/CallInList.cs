using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
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
