using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class Calculation
    {
        [DataMember]
        public double CalculatePrice { get; set; }

        [DataMember]
        public TimeSpan TimeRide { get; set; }
        [DataMember]
        public TimeSpan DriversSeconds { get; set; }

        public Calculation(double calculatePrice, TimeSpan timeRide, TimeSpan driversSeconds)
        {
            CalculatePrice = calculatePrice;
            TimeRide = timeRide;
            DriversSeconds = driversSeconds;
        }
    }
}
