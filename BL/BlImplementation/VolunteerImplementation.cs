using BlApi;
//using BO;
//using DalApi;
using Helpers;


//using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class VolunteerImplementation: IVolunteer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddVolunteer(BO.Volunteer boVolunteer)
    {
        DO.Volunteer doVolunteer =
        new(boVolunteer.Id,
        //ClockManager.Now,
        boVolunteer.Name,
        boVolunteer.PhoneNumber,
        boVolunteer.Email,
        (DO.Position)boVolunteer.Position,
        boVolunteer.Password,
        boVolunteer.Active,
        boVolunteer.CurrentAddress,
        boVolunteer.Latitude,
        boVolunteer.Longitude,
        boVolunteer.MaximumDistanceForReading,
        (DO.TypeOfDistance)boVolunteer.TypeOfDistance
        );
        try
        {
            _dal.Volunteer.Create(doVolunteer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Volunteer with ID={boVolunteer.Id} already exists", ex);
        }

    }

    public void DeleteVolunteer(int id)
    {
        var boVolunteer =Read(id);
        if(boVolunteer.CallInProgress==null && boVolunteer.SumCaredCalls==0 && boVolunteer.SumCancledCalls==0 && boVolunteer.SumIrelevantCalls==0)//////////צריך לבוק אם נרשם לזיכרון בכללי?
        {
            _dal.Volunteer.Delete(boVolunteer.Id);
        }

        /* try
         {
             _dal.Volunteer.Delete(doVolunteer);
         }
         catch (DO.DalAlreadyExistsException ex)
         {
             throw new BO.BlAlreadyExistsException($"Volunteer with ID={id} already exists", ex);
         }*/

    }

    public BO.Position Login(string username, string password)
    {
        var doVolunteer = _dal.Volunteer.Read(vol =>  vol.Name == username && vol.Password == password) ??
        throw new BO.BlDoesNotExistException($"Volunteer with Name ={username} and Password={password} does Not exist");//need tocreate it later
        return (BO.Position)doVolunteer.Position;
    }


    public BO.Volunteer Read(int id)
    {
        var doVolunteer = _dal.Volunteer.Read(vol => vol.Id == id) ??
        throw new BO.BlDoesNotExistException($"Volunteer with ID={id} does Not exist");//need tocreate it later
        return new()
        {
            Id = id,
            Name = doVolunteer.Name,
            PhoneNumber= doVolunteer.PhoneNumber,
            Email = doVolunteer.Email,
            Position = doVolunteer.Position,
            Password = doVolunteer.Password,
            Active = doVolunteer.Active,
            CurrentAddress = doVolunteer.CurrentAddress,
            Latitude = doVolunteer.Latitude,
            Longitude = doVolunteer.Longitude,
            MaximumDistanceForReading=doVolunteer?.MaximumDistanceForReading,
            TypeOfDistance=doVolunteer?.TypeOfDistance,
            SumCancledCalls=doVolunteer?.SumCancledCalls,
           // SumCaredCalls=this.SumCaredCalls
           // SumIrelevantCalls =
            //CallInProgress=
        };

    }

    public IEnumerable<BO.VolunteerInList> ReadAll(bool? activity = null, BO.VoluteerInListObjects? objectToSort = null)
    {
        IEnumerable<DO.Volunteer> volunteers = _dal.Volunteer.ReadAll();
        IEnumerable<BO.VolunteerInList> readVolunteers;
        volunteers = activity == null ? volunteers.Select(item => item) : volunteers.Where(v => v.Active == activity);
        volunteers = objectToSort == null ? volunteers.OrderBy(v.ID) : volunteers.OrderBy();
        readVolunteers = (BO.VolunteerInList)volunteers;
        return readVolunteers;
    }

    public void UpdateVolunteer(int id, BO.Volunteer volunteer)
    {
        throw new NotImplementedException();
    }
}
