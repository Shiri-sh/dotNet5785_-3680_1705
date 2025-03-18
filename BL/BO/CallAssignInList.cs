using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
namespace BO
{
    public  class CallAssignInList
    {
           public int? VolunteerId {  get; set; }
           public string? VolunteerName { get; set; }
           public DateTime TreatmentEntryTime {  get; set; }
           public DateTime? TreatmentEndTime {  get; set; }
           public TypeOfTreatmentTermination? TypeOfTreatmentTermination {  get; set; }
           public override string ToString() => this.ToStringProperty();

    }
}
