using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
namespace BO;
/// <summary>
/// Call entity
/// </summary>
/// <param name="Id">Represents a number that uniquely identifies the call. </param>
/// <param name="KindOfCall">According to the type of specific system</param>
/// <param name="Description">Verbal description on the reading</param>
/// <param name="AddressOfCall">Full and real address in correct format, of the reading location.</param>
/// <param name="Latitude"> a number indicating how far a point on the Earth is south or north of the line</param>
/// <param name="Longitude">a number indicating how far a point on Earth is east or west of the line</param>
/// <param name="OpeningTime">Time (date and time) when the call was opened by the administrator.</param>
/// <param name="FinishTime">Time (date and time) by which the reading should be closed.</param>
/// <param name="Status">the current stage the call is</param>
/// <param name="ListOfAlocation">all the alocations for this call</param>
public class Call
{
    public int Id { get; init; }
    public KindOfCall KindOfCall { get;set; }
    public string AddressOfCall {  get; set; }
    public double Latitude {  get; set; }
    public double Longitude {  get; set; }
    public DateTime OpeningTime {  get; set; }
    public DateTime? FinishTime {  get; set; }
    public string? Description {  get; set; }
    public Status Status {  get; set; }
    public List<BO.CallAssignInList>? ListOfAlocation {  get; set; }
    public override string ToString() => this.ToStringProperty();

}
