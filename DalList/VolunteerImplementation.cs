using DO;
using DalApi;
using System.Collections.Generic;

namespace Dal;

public class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer v)
    {
        if (Read(v.Id) != null) {DataSource.Volunteers.Add(v)/*יש להוסיף את ההפניה לאובייקט שהתקבלה לרשימת האובייקטים מטיפוס T. */ }
        else { throw new NotImplementedException("the volunteer's id is already exist"); }
    }

    public void Delete(int id)
    {
        if (Read(id) != null) { /*delete from the list*/}
        else { throw new NotImplementedException("Volunteer with such Id doesnt exist"); }
    }

    public void DeleteAll()
    {
       /*delete all objects from the list of volunteers*/
    }

    public Volunteer? Read(int id)
    {
        if (/*the object is already exist*/) { return null; }
        else { return null; }
    }

    public List<Volunteer> ReadAll()
    {
        List<Volunteer> copyListOfStudent=
        retrun copyListOfStudent;
    }

    public void Update(Volunteer v)
    {
        if (Read(v.Id) != null) { }
        else { throw new NotImplementedException("Volunteer with such Id doesnt exist"); }
    }
}
