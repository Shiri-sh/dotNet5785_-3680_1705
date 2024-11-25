namespace DalApi;
public interface IConfig
{
    TimeSpan RiskRange { get; set; }
    DateTime Clock { get; set; }
    //...
    void Reset();
    void UpdatenextCallId(int newId);
    int NextCallId();
    void UpdatenextAssignmentId(int newId);
    int NextAssignmentId();
}
