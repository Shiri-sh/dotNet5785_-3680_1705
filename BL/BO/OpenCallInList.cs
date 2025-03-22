using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace BO;
/// <summary>
/// Represents the details of an open call in the list, including its status, description, and distance from the volunteer.
/// </summary>
/// <param name="Id">The unique identifier of the open call.</param>
/// <param name="KindOfCall">The type of call, represented by an enum </param>
/// <param name="AddressOfCall">The address where the open call is taking place.</param>
/// <param name="OpeningTime">The date and time when the call was opened or initiated.</param>
/// <param name="FinishTime">The date and time when the call was finished. This can be NULL if the call is still open.</param>
/// <param name="Description">A description of the call, such as additional details. This can be NULL if no description is provided.</param>
/// <param name="DistanceFromVol">The distance from the volunteer to the location of the open call, measured in kilometers or other units.</param>
public class OpenCallInList
{
    public int Id { get; init; }
    public KindOfCall KindOfCall { get; set; }
    public string AddressOfCall { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime? FinishTime { get; set; }
    public string? Description { get; set; }
    public double DistanceFromVol { get; set; }
    public override string ToString() => this.ToStringProperty();

}
