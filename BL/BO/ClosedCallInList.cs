
using Helpers;

namespace BO;

public class ClosedCallInList
{
    public int Id { get; init; }
    public KindOfCall KindOfCall { get; set; } 
    public string AddressOfCall { get; set; }
    public DateTime OpeningTime { get; set; } //זמן פתיחה
    public DateTime TreatmentEntryTime {  get; set; } //זמן כניסה לטיפול
    public DateTime? TreatmentEndTime {  get; set; } //זמן סיום טיפול
    public TypeOfTreatmentTermination? TypeOfTreatmentTermination {  get; set; }
    public override string ToString() => this.ToStringProperty();
}
