namespace Dal;
using DalApi;
public class ConfigImplementation : IConfig
{
    public TimeSpan RiskRange
    {
        get => Config.RiskRange;
        set => Config.RiskRange = value;    
    }
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }
    //...
    public void UpdatenextAssignmentId(int newId)
    {
        Config.UpdatenextAssignmentId(newId);
    }
    public void UpdatenextCallId(int newId)
    {
        Config.UpdatenextCallId(newId);
    }
    public void Reset()
    {
        Config.Reset();
    }
}

//i dont know why there are errors here......