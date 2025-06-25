using DO;
using DalApi;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Dal;

internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// Adding a new object of type Volunteer to a database,
    /// </summary>
    /// <param name="item">exist Volunteer object</param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Volunteer item)
    {
        if (Read(vol => vol.Id == item.Id) is not null)
            throw new DalAlreadyExistsException($"Volunteer with ID ={item.Id} already exist");
        DataSource.Volunteers.Add(item); 
    }
    /// <summary>
    /// Deletion of an existing object with a certain ID, from a list of objects of type Volunteer.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="NotImplementedException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        if (Read(volunteer => volunteer.Id == id) is null)
            throw new DalDoesNotExistException($"Volunteer with ID ={id} isn't exists");
        DataSource.Volunteers.RemoveAll(Volunteer=>Volunteer.Id==id);

    }
    /// <summary>
    /// Deletion of all objects in a list of type Volunteer
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>Returning a reference to a single object of type Volunteer with a certain ID, if it exists in a database, or null if the object does not exist.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(Func<Volunteer, bool> filter)
      => DataSource.Volunteers.FirstOrDefault(filter);

    /// <summary>
    ///
    /// </summary>
    /// <returns>Return a copy of the list of references to all objects from a type</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
            => filter == null
            ? DataSource.Volunteers.Select(item => item) : DataSource.Volunteers.Where(filter);


    /// <summary>
    /// Update of an existing object.
    /// </summary>
    /// <param name="item">An existing object of type Volunteer is updated.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Volunteer item)
    {
        if (Read(vol => vol.Id == item.Id) is null) 
           throw new DalDoesNotExistException($"Volunteer with ID ={ item.Id } isn't exists");
        Volunteer newVolunteer = item with { Id = item.Id };
        DataSource.Volunteers.RemoveAll(Volunteer => Volunteer.Id == item.Id);
        DataSource.Volunteers.Add(newVolunteer);

    }
}

