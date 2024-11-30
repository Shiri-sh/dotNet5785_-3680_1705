namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class AssignmetImplementation : IAssignment
{
    /// <summary>
    /// Adding a new object of type Assigment to a database,
    /// </summary>
    /// <param name="item">exist Assigment object</param>
    public void Create(Assignment item)
    {
        int id = Config.NextAssignmentId;
        Assignment newAssignment = item with { Id = id };
        DataSource.Assignments.Add(newAssignment);
    }
    /// <summary>
    /// Deletion of an existing object with a certain ID, from a list of objects of type Assigment.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new NotImplementedException($"assignment with ID={id} isn't exists");
        DataSource.Assignments.RemoveAll(assignment => assignment.Id == id);
    }
    /// <summary>
    /// Deletion of all objects in a list of type Assignment
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>Returning a reference to a single object of type Assignment with a certain ID, if it exists in a database, or null if the object does not exist.</returns>
    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(assignment => assignment.Id == id);
    }
    /// <summary>
    /// Return a copy of the list of references to all objects from a type
    /// </summary>
    /// <returns>Return a copy of the list of references to all objects from a type</returns>
    public List<Assignment> ReadAll()
    {
        return new List<Assignment>(DataSource.Assignments);
    }
    /// <summary>
    /// Update of an existing object.
    /// </summary>
    /// <param name="item">An existing object of type Assignment is updated.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>
    public void Update(Assignment item)
    {
        if (Read(item.Id) is null)
            throw new NotImplementedException($"assignment with ID={item.Id} isn't exists");
        Assignment newAssignment = item with { Id = item.Id };
        DataSource.Assignments.RemoveAll(assignment => assignment.Id == item.Id);
        DataSource.Assignments.Add(newAssignment);
    }
}
