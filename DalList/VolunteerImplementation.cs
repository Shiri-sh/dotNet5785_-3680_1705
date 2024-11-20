
namespace Dal;
using DO;
using DalApi;
using System.Collections.Generic;
public class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// Adding a new object of type Volunteer to a database,
    /// </summary>
    /// <param name="item">exist Volunteer object</param>
    public void Create(Volunteer item)
    {
        if (Read(item.Id) is not null)
            throw new NotImplementedException("the volunteer's id is already exist");
        DataSource.Volunteers.Add(item); 
    }
    /// <summary>
    /// Deletion of an existing object with a certain ID, from a list of objects of type Volunteer.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new NotImplementedException("Volunteer with such Id doesnt exist");
        DataSource.Volunteers.RemoveAll(Volunteer=>Volunteer.Id==id);

    }
    /// <summary>
    /// Deletion of all objects in a list of type Volunteer
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>Returning a reference to a single object of type Volunteer with a certain ID, if it exists in a database, or null if the object does not exist.</returns>
    public Volunteer? Read(int id)
    {
         return DataSource.Volunteers.Find(volunteer => volunteer.Id == id);
    }
    /// <summary>
    ///
    /// </summary>
    /// <returns>Return a copy of the list of references to all objects from a type</returns>
    public List<Volunteer> ReadAll()
    {
        return new List<Volunteer>(DataSource.Volunteers);
    }
    /// <summary>
    /// Update of an existing object.
    /// </summary>
    /// <param name="item">An existing object of type Volunteer is updated.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>
    public void Update(Volunteer item)
    {
        if (Read(item.Id) is null) 
           throw new NotImplementedException("Volunteer with such Id doesnt exist");
        Volunteer copy = item with { Id = item.Id };
        DataSource.Volunteers.RemoveAll(Volunteer => Volunteer.Id == item.Id);
        DataSource.Volunteers.Add(copy);

    }
}

