namespace DO;
public enum KindOfCall { RescueKid, changeWheel, FirstAid, CableAssistance, fuelOilWater }
public enum Position { Managar, Volunteer }
public enum TypeOfDistance { Aerial, walking, driving }
public enum TypeOfTreatmentTermination { Handled, SelfCancellation, ConcellingAdministrator, CancellationExpired }

public enum MainMenuEnum  { Exit, SubMenuVolunteer, SubMenuCall, SubMenuAssignment, InitializationAll, DisplayAllData, SubMenuConfig, ResetAllDetails }
public enum SubMenuEnum { Exit, AddNew, DisplayById, DisplayAll, Update,Delete,DeleteAll}
public enum ConfigSubMenuEnum { Exit, AdvanceMinute, AdvanceHour, AdvancePress, DisplayClock,SetOne, DisplayConfig, Reset }
