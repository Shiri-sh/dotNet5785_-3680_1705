using DO;
using DalApi;
using System.Collections.Generic;

namespace Dal;

public class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        if (Read(item.Id) is not null)
            throw new NotImplementedException($"Volunteer with ID ={item.Id} already exist");
        DataSource.Volunteers.Add(item); 
    }
    public void Delete(int id)
    {
        if (Read(id) is null)
            throw new NotImplementedException($"Volunteer with ID ={id} isn't exists");
        DataSource.Volunteers.RemoveAll(Volunteer=>Volunteer.Id==id);

    }
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    public Volunteer? Read(int id)
    {
         return DataSource.Volunteers.Find(volunteer => volunteer.Id == id);
    }

    public List<Volunteer> ReadAll()
    {
        return new List<Volunteer>(DataSource.Volunteers);
    }

    public void Update(Volunteer item)
    {
        if (Read(item.Id) is null) 
           throw new NotImplementedException($"Volunteer with ID ={ item.Id } isn't exists");
        Volunteer newVolunteer = item with { Id = item.Id };
        DataSource.Volunteers.RemoveAll(Volunteer => Volunteer.Id == item.Id);
        DataSource.Volunteers.Add(newVolunteer);

    }
}

