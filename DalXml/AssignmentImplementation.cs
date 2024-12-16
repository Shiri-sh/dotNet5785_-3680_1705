namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class AssignmentImplementation : IAssignment
{
    static Assignment getAssignment(XElement a)
    {
        return new DO.Assignment()
        {
            Id = a.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            CalledId = a.ToIntNullable("CalledId") ?? throw new FormatException("can't convert Call Id"),
            VolunteerId = a.ToIntNullable("VolunteerId") ?? throw new FormatException("can't convert Volunteer Id"),
            TreatmentEntryTime = a.ToDateTimeNullable("TreatmentEntryTime") ?? throw new FormatException("can't convert Volunteer Id"),
            TreatmentEndTime = a.ToDateTimeNullable("TreatmentEndTime")

            // Name = (string?)a.Element("CalledId") ?? "",
            // Alias = (string?)a.Element("Alias") ?? null,
            // IsActive = (bool?)a.Element("IsActive") ?? false,
            //CurrentYear = s.ToEnumNullable<Year>("CurrentYear") ?? Year.FirstYear,

        };
    }

    public Assignment? Read(int id)
    {
        XElement? assignmentElem =
    XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().FirstOrDefault(asi => (int?)asi.Element("Id") == id);
        return assignmentElem is null ? null : getAssignment(assignmentElem);
    }

    public Assignment? Read(Func<Assignment, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().Select(asi => getAssignment(asi)).FirstOrDefault(filter);
    }

    public void Update(Assignment item)
    {
        XElement assignmentsRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);

        (assignmentsRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id)
        ?? throw new DO.DalDoesNotExistException($"Assignment with ID={item.Id} does Not exist"))
                .Remove();
        XElement assignmentsRoot = new XElement("Assignment",
          new XElement("Id", item.Id),
          new XElement("CalledId", item.CalledId),
          new XElement("VolunteerId", item.VolunteerId),
          new XElement("TreatmentEntryTime", item.TreatmentEntryTime),
          new XElement("TreatmentEndTime", item.TreatmentEndTime)
          );

        assignmentsRootElem.Add(new XElement("Assignment", assignmentsRoot));

        XMLTools.SaveListToXMLElement(assignmentsRootElem, Config.s_assignments_xml);
    }

    public void Create(Assignment item)
    {
        XElement assignmentsRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        XElement assignmentsRoot = new XElement("Assignment",
            new XElement("Id", Config.NextAssignmentId),
            new XElement("CalledId", item.CalledId),
            new XElement("VolunteerId", item.VolunteerId),
            new XElement("TreatmentEntryTime", item.TreatmentEntryTime),
            new XElement("TreatmentEndTime", item.TreatmentEndTime)
            );
        assignmentsRootElem.Add(new XElement("Assignment", assignmentsRoot));

        XMLTools.SaveListToXMLElement(assignmentsRootElem, Config.s_assignments_xml);
    }
    public void Delete(int id)
    {
        XElement assignmentsRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        (assignmentsRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)
        ?? throw new DO.DalDoesNotExistException($"Assignment with ID={id} does Not exist"))
                .Remove();
        XMLTools.SaveListToXMLElement(assignmentsRootElem, Config.s_assignments_xml);
    }
    public void DeleteAll()
    {
        XElement assignmentsRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        assignmentsRootElem.Elements().Remove();
        XMLTools.SaveListToXMLElement(assignmentsRootElem, Config.s_assignments_xml);
    }
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        XElement assignmentsRootElem = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        IEnumerable<Assignment> readAssignment = filter == null ? assignmentsRootElem.Elements().Select(item =>getAssignment( item)) : assignmentsRootElem.Elements().Select(item => getAssignment(item)).Where(filter);
        return readAssignment;
    }
}
