﻿namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class VolunteerImplementation : IVolunteer
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Create(Volunteer item)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Volunteers.RemoveAll(it => it.Id == item.Id) > 0)
            throw new DalDoesNotExistException($"Volunteer with ID={item.Id} already exists");
        Volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int id)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Volunteers.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Volunteer with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Volunteer>(), Config.s_volunteers_xml);
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        Volunteer? readVolunteer = Volunteers.FirstOrDefault(filter);
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
        return readVolunteer;
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        IEnumerable<Volunteer>  readVolunteers = filter == null? Volunteers.Select(item => item) : Volunteers.Where(filter);
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
        return readVolunteers;
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Volunteer item)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Volunteers.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist");
        Volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }
}
