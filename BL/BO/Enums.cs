
using Helpers;

namespace BO;
public enum TypeOfTime { Minute, Hour, Day, Month, Year }
public enum Position { Managar, Volunteer }
public enum TypeOfDistance { Aerial, walking, driving }
public enum KindOfCall { RescueKid, changeWheel, FirstAid, CableAssistance, fuelOilWater,None }
//לא לשנות את הסדר זה חשוב !
public enum Status { Open, OpenInRisk, BeingCared, Closed , Irelavant}
public enum StatusCallInProgress { Open, OpenInRisk }

public enum TypeOfTreatmentTermination { Handled, SelfCancellation, ConcellingAdministrator, CancellationExpired }
public enum VoluteerInListObjects {Id, Name, Active , SumCancledCalls, SumCaredCalls, sumIrelevantCalls, IdOfCall, KindOfCall }
public enum CallInListObjects { Id ,CallId, KindOfCall, OpeningTime, RemainingTimeToFinish, LastVolunteer, CompletionTime, Status, TotalAlocation }
public enum CloseCallInListObjects { Id, KindOfCall, AddressOfCall, OpeningTime, TreatmentEntryTime, TreatmentEndTime, TypeOfTreatmentTermination}

//main-menu
public enum MainMenuEnum { Exit, SubMenuVolunteer, SubMenuCall, SubMenuAdmin }
public enum SubMenuVolunteerEnum { Exit, LoginSystem, DisplayAll, DisplayById, Update, Delete,AddNew }
public enum SubMenuCallEnum { Exit,Add, CallByStatus, DisplayAll,Cancel, DisplayById, Delete, GetAllCallByVolunteer, GetCall, UpdateCall, UpdateEndCall, CooseCall }

public enum ConfigSubMenuEnum { Exit, AdvanceMinute, AdvanceHour, AdvancePress, DisplayClock, SetOne, DisplayConfig, Reset }
