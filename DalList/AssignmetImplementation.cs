

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class AssignmetImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        int id = DataSource.Config.NextCourseId;
        Assignment copy = item with { Id = id };
        DataSource.Assignments.Add(copy);

    }

    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new NotImplementedException("Assignment with such Id doesnt exist");
        DataSource.Assignments.RemoveAll(assignment => assignment.Id == id);
    }

    public void DeleteAll()
    {
        DataSource.Assignments.Clear();

    }

    public Assignment? Read(int id)
    {
        return DataSource.Assignments.Find(assignment => assignment.Id == id);
    }

    public List<Assignment> ReadAll()
    {
        return new List<Assignment>(DataSource.Assignments);
    }

    public void Update(Assignment item)
    {
        if (Read(item.Id) is null)
            throw new NotImplementedException("Assignment with such Id doesnt exist");
        Assignment copy = item with { Id = item.Id };
        DataSource.Assignments.RemoveAll(assignment => assignment.Id == item.Id);
        DataSource.Assignments.Add(copy);
    }
}
