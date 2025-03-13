using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CallInProgress
    {
        public int Id { get; init; }
        public int CallId {  get; init; }
        public KindOfCall KindOfCall { get; set; }
        public required string AddressOfCall { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public string? Description { get; set; }
        public DateTime TreatmentEntryTime { get; set; }
        public double DistanceFromVolunteer { get; set; }
        public StatusCallInProgress Status { get; set; }
     //   public override string ToString() => this.ToStringProperty();

    }
}
