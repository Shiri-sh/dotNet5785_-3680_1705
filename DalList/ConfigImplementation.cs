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

    public int NextAssignmentId()
    {
        return Config.NextAssignmentId;
    }
    public int NextCallId()
    {
        return Config.NextCallId;
    }
    public void Reset()
    {
        Config.Reset();
    }
}
