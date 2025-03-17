//using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IVolunteer
{
    BO.Position Login(string username, string password);
    IEnumerable<BO.VolunteerInList>ReadAll(bool? activity=null, BO.VoluteerInListObjects? objectToSort=null);
    BO.Volunteer Read(int id);
    void UpdateVolunteer(int id, BO.Volunteer volunteer);
    void DeleteVolunteer(int id);
    void AddVolunteer(BO.Volunteer boVolunteer);
    void ValidateVolunteer(BO.Volunteer boVolunteer);
    bool IsValidIsraeliID(int id);
    bool IsValidAddress(double? lon, double? lat);

}
