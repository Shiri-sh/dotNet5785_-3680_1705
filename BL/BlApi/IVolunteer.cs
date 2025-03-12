using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IVolunteer
{
    Position Login(string username, string password);
    IEnumerable<BO.VolunteerInList>ReadAll(bool? activity=null, VoluteerInListObjects? objectToSort=null);
    BO.Volunteer Read(int id);
    void UpdateVolunteer(int id, BO.Volunteer volunteer);
    void DeleteVolunteer(int id);
    void AddVolunteer(BO.Volunteer volunteer);
}
