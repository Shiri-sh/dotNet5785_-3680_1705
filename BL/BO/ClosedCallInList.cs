using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace BO;

public class ClosedCallInList
{
    public int Id { get; init; }
    public KindOfCall KindOfCall { get; set; }
    public string AddressOfCall { get; set; }
   
    public DateTime OpeningTime { get; set; }
    public DateTime TreatmentEntryTime {  get; set; }
    public DateTime? TreatmentEndTime {  get; set; }
    public TypeOfTreatmentTermination? TypeOfTreatmentTermination {  get; set; }
    public override string ToString() => this.ToStringProperty();
}
