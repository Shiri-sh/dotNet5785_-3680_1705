using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class VolunteerInList
    {
        public int Id { get; init; }
        public string Name { get; set; }
       
        public bool Active { get; set; }
        public int SumCancledCalls { get; set; }
        public int SumCaredCalls { get; set; }
        public int sumIrelevantCalls { get; set; }
        public int? IdOfCall {  get; set; }
        public KindOfCall KindOfCall { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}

