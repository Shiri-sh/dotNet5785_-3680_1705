
using DO;
using DalApi;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace Dal;

internal class CallImplementation : ICall
{
    /// <summary>
    /// Adding a new object of type Call to a database,
    /// </summary>
    /// <param name="item">exist Call object</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Call item)
    {
        int id = Config.NextCallId;
        Call newCall = item with { Id = id };
        DataSource.Calls.Add(newCall);
    }
    /// <summary>
    /// Deletion of an existing object with a certain ID, from a list of objects of type Call.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="NotImplementedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        if (Read(call => call.Id == id) is null)
            throw new DalDoesNotExistException($"call with ID ={id} isn't exists");
        DataSource.Calls.RemoveAll(call => call.Id == id);
    }
    /// <summary>
    /// Deletion of all objects in a list of type Call
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        DataSource.Calls.Clear();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>Returning a reference to a single object of type Call with a certain ID, if it exists in a database, or null if the object does not exist.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Call? Read(Func<Call, bool> filter)
         =>DataSource.Calls.FirstOrDefault(filter);
    /// <summary>
    /// Return a copy of the list of references to all objects from a type
    /// </summary>
    /// <returns>Return a copy of the list of references to all objects from a type</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
            => filter == null
            ? DataSource.Calls.Select(item => item) : DataSource.Calls.Where(filter);

    /// <summary>
    /// Update of an existing object.
    /// </summary>
    /// <param name="item">An existing object of type Call is updated.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Call item)
    {
        if (Read(call => call.Id == item.Id) is null)
            throw new DalDoesNotExistException($"call with ID ={item.Id} isn't exists");
        Call newCall = item with { Id = item.Id };
        DataSource.Calls.RemoveAll(call => call.Id == item.Id);
        DataSource.Calls.Add(newCall);
    }
}
