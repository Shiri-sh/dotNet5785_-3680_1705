
namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;

internal class CallImplementation : ICall
{
    public void Create(Call item)
    {
        int id = DataSource.Config.NextCourseId;
        Call copy = item with { Id = id };
        DataSource.Calls.Add(copy);
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new NotImplementedException("Call with such Id doesnt exist");
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
            throw new NotImplementedException("Call with such Id doesnt exist");
        Call copy = item with { Id = item.Id };
        DataSource.Calls.RemoveAll(call => call.Id == item.Id);
        DataSource.Calls.Add(copy);
    }
}
