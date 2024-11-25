using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dal;
/// <summary>
/// Config entity
/// </summary>
/// <param name="startCallId">Initializes the ID number of the next call </param>
/// <param name="nextCallId">Represents an ID number for the next new call. runs automatically</param>
/// <param name="startAssignmentId">Initialize the allocation runner ID number to be equal to 1</param>
/// <param name="nextAssignmentId">Represents an ID number for a new instance of the Bin assignment entity Volunteer to read.from which the following ID number will be taken for the allocation entity.runs automatically</param>
/// <parm name="Clock">A system clock that will be maintained separately from the actual computer clock.
///                    The system administrator will be able to initialize and update (advance) the system clock.
///                    In each operation, the current system time will be the current value of the system clock.</parm>
/// <param name="RiskRange">Time range from which onwards reading is considered at risk. approaching its required end time.</param>

internal static class Config
{
    //risk range, after 15 minutes the assigment is at risk
    internal static TimeSpan RiskRange { get; set; } = TimeSpan.FromMinutes(15);

    //count call id auto
    internal const int startCallId = 1;
    private static int nextCallId = startCallId;
    internal static int NextCallId { get => nextCallId++; }

    ///count auto
    internal const int startAssignmentId = 1;
    private static int nextAssignmentId = startAssignmentId;
    internal static int NextAssignmentId { get => nextAssignmentId++; }

    //clock
    internal static DateTime Clock { get; set; } = DateTime.Now;
    internal static void Reset()
    {
        nextCallId = 0;
        nextAssignmentId = 0;
        Clock = DateTime.Now;
    }

}

