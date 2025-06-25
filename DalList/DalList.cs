namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    //עושים סינגלטון כדי שליצר רק מופע אחד מהמחלקה הזאת
    private static readonly DalList intance = new DalList();
    public static IDal Instance { get; } = new DalList();
    static DalList() { }
    private DalList() { }
    public IVolunteer Volunteer { get; } = new VolunteerImplementation();
    public ICall Call { get; } =new CallImplementation();

    public IAssignment Assignment { get; }=new AssignmetImplementation();

    public IConfig Config { get; }=new ConfigImplementation();  

    public void ResetDB()
    {
        Volunteer.DeleteAll();
        Call.DeleteAll();
        Assignment.DeleteAll();
        Config.Reset();
    }
}
