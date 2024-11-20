
namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(newCall);
    }
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new NotImplementedException($"call with ID ={id} isn't exists");
        DataSource.Calls.RemoveAll(call => call.Id == id);
    }

    public void DeleteAll()
    {
        DataSource.Calls.Clear();

    }

    public Call? Read(int id)
    {
        return DataSource.Calls.Find(call => call.Id == id);

    }

    public List<Call> ReadAll()
    {
        return new List<Call>(DataSource.Calls);

    }

    public void Update(Call item)
    {
        if (Read(item.Id) is null)
            throw new NotImplementedException($"call with ID ={item.Id} isn't exists");
        Call newCall = item with { Id = item.Id };
        DataSource.Calls.RemoveAll(call => call.Id == item.Id);
        DataSource.Calls.Add(newCall);
    }
}
