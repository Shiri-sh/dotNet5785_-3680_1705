using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Volunteer
    {

        public int Id {  get; init; }
        public string Name {  get; set; }
        public string PhoneNumber { get; set; }
        public string Email {  get; set; }
        public Position Position {  get; set; }
        public string? Password {  get; set; }
        public bool Active {  get; set; }
        public string? CurrentAddress {  get; set; }
        public double? Latitude {  get; set; }
        public double? Longitude {  get; set; }
        public double? MaximumDistanceForReading {  get; set; }
        public TypeOfDistance TypeOfDistance { get; set; }
        public int SumCancledCalls {  get; set; }
        public int SumCaredCalls {  get; set; }
        public BO.CallInProgress? CallInProgress { get; set; }
        public int SumIrelevantCalls {  get; set; }
     
        public override string ToString() => this.ToStringProperty();

    }

}
