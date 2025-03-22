using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Represents a volunteer with their personal details, activity status, and performance metrics.
/// </summary>
/// <param name="Id">The unique identifier of the volunteer.</param>
/// <param name="Name">The name of the volunteer.</param>
/// <param name="PhoneNumber">The phone number of the volunteer.</param>
/// <param name="Email">The email address of the volunteer.</param>
/// <param name="Position">The position or role of the volunteer, represented by an enum.</param>
/// <param name="Password">The password of the volunteer. This can be NULL if no password is set.</param>
/// <param name="Active">Indicates whether the volunteer is currently active and available for calls.</param>
/// <param name="CurrentAddress">The current address of the volunteer. This can be NULL if not provided.</param>
/// <param name="Latitude">The latitude of the volunteer's current location. This can be NULL if not available.</param>
/// <param name="Longitude">The longitude of the volunteer's current location. This can be NULL if not available.</param>
/// <param name="MaximumDistanceForReading">The maximum distance (in kilometers or other units) the volunteer is willing to travel for calls. This can be NULL if not specified.</param>
/// <param name="TypeOfDistance">The unit of measurement for the volunteer's maximum distance (e.g., kilometers, miles).</param>
/// <param name="SumCancledCalls">The total number of calls the volunteer has canceled.</param>
/// <param name="SumCaredCalls">The total number of calls the volunteer has completed or cared for.</param>
/// <param name="CallInProgress">The current call the volunteer is working on. This can be NULL if no call is in progress.</param>
/// <param name="SumIrelevantCalls">The total number of calls that their "finishTime" was over.</param>??????????????????????
public class Volunteer
{

    public int Id {  get; init; }
    public string Name {  get; set; }
    public string PhoneNumber { get; set; }
    public string Email {  get; set; }
    public Position Position {  get; set; }
    public string? Password {  get; set; }
    public bool Active {  get; set; }
    public string? CurrentAddress {  get; set; }
    public double? Latitude {  get; set; }
    public double? Longitude {  get; set; }
    public double? MaximumDistanceForReading {  get; set; }
    public TypeOfDistance TypeOfDistance { get; set; }
    public int SumCancledCalls {  get; set; }
    public int SumCaredCalls {  get; set; }
    public BO.CallInProgress? CallInProgress { get; set; }
    public int SumIrelevantCalls {  get; set; }
 
    public override string ToString() => this.ToStringProperty(); 

}
