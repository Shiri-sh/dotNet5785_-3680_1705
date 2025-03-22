
using Helpers;

namespace BO;
public enum TypeOfTime { Minute, Hour, Day, Month, Year }
public enum Position { Managar, Volunteer }
public enum TypeOfDistance { Aerial, walking, driving }
public enum KindOfCall { RescueKid, changeWheel, FirstAid, CableAssistance, fuelOilWater,None }
//לא לשנות את הסדר זה חשוב !
public enum StatusCallInProgress { Open, TreatInRisk }
public enum Status { Open, TreatInRisk, OpenInRisk, BeingCared, Closed, Irelavant }
//
public enum TypeOfTreatmentTermination { Handled, SelfCancellation, ConcellingAdministrator, CancellationExpired }
public enum VoluteerInListObjects {Id, Name, Active , SumCancledCalls, SumCaredCalls, sumIrelevantCalls, IdOfCall, KindOfCall }
public enum CallInListObjects { Id ,CallId, KindOfCall, OpeningTime, RemainingTimeToFinish, LastVolunteer, CompletionTime, Status, TotalAlocation }
public enum CloseCallInListObjects { Id, KindOfCall, AddressOfCall, OpeningTime, TreatmentEntryTime, TreatmentEndTime, TypeOfTreatmentTermination}
//main-menu
public enum MainMenuEnum { Exit, SubMenuVolunteer, SubMenuCall, SubMenuAdmin }
public enum SubMenuVolunteerEnum { Exit, AddNew, DisplayById, DisplayAll  , Update, Delete, LoginSystem }
public enum SubMenuCallEnum { Exit,Add, CallByStatus,Cancel, Delete, DisplayAll, GetAllCallByVolunteer , UpdateEndCall, UpdateCall, DisplayById, CooseCall,OpenCalls,UpdateOpenCalls }
public enum ConfigSubMenuEnum { Exit, AdvanceMinute, AdvanceHour, AdvancePress, DisplayClock, SetOne, DisplayConfig, Reset }
