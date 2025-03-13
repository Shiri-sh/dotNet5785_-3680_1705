using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class VolunteerImplementation:IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(Volunteer volunteer)
    {
        throw new NotImplementedException();
    }

    public void DeleteVolunteer(int id)
    {
        throw new NotImplementedException();
    }

    public Position Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Volunteer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<VolunteerInList> ReadAll(bool? activity = null, VoluteerInListObjects? objectToSort = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateVolunteer(int id, Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
