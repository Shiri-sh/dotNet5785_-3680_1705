namespace Dal;
using DalApi;
public class ConfigImplementation : IConfig
{
    public DateTime Clock
    {
        get => Config.Clock;
        set => Config.Clock = value;
    }
    //...
    public void Reset()
    {
        Config.Reset();
    }
}

//i dont know why there are errors here......