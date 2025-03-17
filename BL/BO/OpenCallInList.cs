using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;

namespace BO
{
    public class OpenCallInList
    {
        public int Id { get; init; }
        public KindOfCall KindOfCall { get; set; }
        public string AddressOfCall { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public string? Description { get; set; }
        public double DistanceFromVol { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
