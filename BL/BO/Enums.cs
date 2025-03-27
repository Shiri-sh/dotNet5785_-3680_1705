
using Helpers;

namespace BO;
public enum TypeOfTime { Minute, Hour, Day, Month, Year }
public enum Position { Managar, Volunteer }
public enum TypeOfDistance { Aerial, walking, driving }
public enum KindOfCall { RescueKid, changeWheel, FirstAid, CableAssistance, fuelOilWater,None }
//לא לשנות את הסדר זה חשוב !
public enum StatusCallInProgress { BeingCared, TreatInRisk }
public enum Status { BeingCared, TreatInRisk, Open, OpenInRisk, Closed, Irelavant }
//
public enum TypeOfTreatmentTermination { Handled, SelfCancellation, ConcellingAdministrator, CancellationExpired }
public enum VoluteerInListObjects { KindOfCall,Id, Name, Active , SumCancledCalls, SumCaredCalls, sumIrelevantCalls, IdOfCall  }
public enum CallInListObjects { TotalAlocation, Id ,CallId, KindOfCall, OpeningTime, RemainingTimeToFinish, LastVolunteer, CompletionTime, Status }
public enum CloseCallInListObjects { TypeOfTreatmentTermination,Id, KindOfCall, AddressOfCall, OpeningTime, TreatmentEntryTime, TreatmentEndTime}
public enum OpenCallInListFields { DistanceFromVol,Id, KindOfCall, AddressOfCall, OpeningTime, FinishTime, Description }

//main-menu
public enum MainMenuEnum { Exit, SubMenuVolunteer, SubMenuCall, SubMenuAdmin }
public enum SubMenuVolunteerEnum { Exit, AddNew, DisplayById, DisplayAll  , Update, Delete, LoginSystem }
public enum SubMenuAdminEnum { Exit, DisplayTime, DisplayTimeRisk, updateRiskTime  , reset, initialization, updateTime }
public enum SubMenuCallEnum { Exit,Add, CallByStatus,Cancel, Delete, DisplayAll, GetAllCallByVolunteer , UpdateEndCall, UpdateCall, DisplayById, CooseCall,OpenCalls }
public enum ConfigSubMenuEnum { Exit, AdvanceMinute, AdvanceHour, AdvancePress, DisplayClock, SetOne, DisplayConfig, Reset }
