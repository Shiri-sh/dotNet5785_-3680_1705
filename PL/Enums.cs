using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

internal class KindOfCallCollection : IEnumerable
{
    static readonly IEnumerable<BO.KindOfCall> s_enums =
(Enum.GetValues(typeof(BO.KindOfCall)) as IEnumerable<BO.KindOfCall>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class CloseCallInListCollection : IEnumerable
{
    static readonly IEnumerable<BO.CloseCallInListObjects> s_enums =
(Enum.GetValues(typeof(BO.CloseCallInListObjects)) as IEnumerable<BO.CloseCallInListObjects>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class PositionCollection : IEnumerable
{
    static readonly IEnumerable<BO.Position> s_enums =
(Enum.GetValues(typeof(BO.Position)) as IEnumerable<BO.Position>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class StatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> s_enums =
(Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class StatusCallInProgressCollection : IEnumerable
{
    static readonly IEnumerable<BO.StatusCallInProgress> s_enums =
(Enum.GetValues(typeof(BO.StatusCallInProgress)) as IEnumerable<BO.StatusCallInProgress>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class TypeOfDistanceCollection : IEnumerable
{
    static readonly IEnumerable<BO.TypeOfDistance> s_enums =
(Enum.GetValues(typeof(BO.TypeOfDistance)) as IEnumerable<BO.TypeOfDistance>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class TypeOfTimeCollection : IEnumerable
{
    static readonly IEnumerable<BO.TypeOfTime> s_enums =
(Enum.GetValues(typeof(BO.TypeOfTime)) as IEnumerable<BO.TypeOfTime>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
internal class TypeOfTreatmentTerminationCollection : IEnumerable
{
    static readonly IEnumerable<BO.TypeOfTreatmentTermination> s_enums =
(Enum.GetValues(typeof(BO.TypeOfTreatmentTermination)) as IEnumerable<BO.TypeOfTreatmentTermination>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
