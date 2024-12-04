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
        if (Read(ass => ass.Id == id) is null)
            throw new DalDoesNotExistException($"assignment with ID={id} isn't exists");
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
    /// <param name="filter">boolian function</param>
    /// <returns>assignment or null</returns>
    public Assignment? Read(Func<Assignment, bool> filter)
      => DataSource.Assignments.FirstOrDefault(filter);


    /// <summary>
    /// Return a copy of the list of references to all objects from a type
    /// </summary>
    /// <returns>Return a copy of the list of references to all objects from a type</returns>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
            => filter == null
            ? DataSource.Assignments.Select(item => item) : DataSource.Assignments.Where(filter);

    /// <summary>
    /// Update of an existing object.
    /// </summary>
    /// <param name="item">An existing object of type Assignment is updated.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>
    public void Update(Assignment item)
    {
        if (Read(ass => ass.Id==item.Id) is null)
            throw new DalDoesNotExistException($"assignment with ID={item.Id} isn't exists");
        Assignment newAssignment = item with { Id = item.Id };
        DataSource.Assignments.RemoveAll(assignment => assignment.Id == item.Id);
        DataSource.Assignments.Add(newAssignment);
    }
}
