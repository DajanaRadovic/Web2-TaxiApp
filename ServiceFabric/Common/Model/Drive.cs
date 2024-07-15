using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class Drive
    {
        [DataMember]
        public string FromLocation { get; set; }

        [DataMember]
        public string ToLocation { get; set; }

        [DataMember]
        public Guid IdRider { get; set; }

        [DataMember]
        public Guid IdDriver { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public bool Accepted { get; set; }

        [DataMember]
        public Guid IdDrive { get; set; }

        [DataMember]
        public int TimeToDriverArrivalSeconds { get; set; }

        [DataMember]
        public int TimeToEndTripInSeconds { get; set; }

        [DataMember]
        public bool IsFinished { get; set; }

        [DataMember]
        public bool IsRated { get; set; }

        public Drive() { }

        public Drive(Guid idRider, Guid idDriver, string fromLocation, string toLocation, double price, bool accepted)
        {
            IdRider = idRider;
            IdDriver = idDriver;
            FromLocation = fromLocation;
            ToLocation = toLocation;
            Price = price;
            Accepted = accepted;
            IdDrive = Guid.NewGuid();
        }

        public Drive(Guid idRider, string fromLocation, string toLocation, double price, bool accepted, int minutes)
        {
            IdRider = idRider;
            FromLocation = fromLocation;
            ToLocation = toLocation;
            IdDriver = new Guid("00000000-0000-0000-0000-000000000000"); ;
            Price = price;
            Accepted = accepted;
            IdDrive = Guid.NewGuid();
            TimeToDriverArrivalSeconds = minutes * 60;
            IsFinished = false;
            IsRated = false;
        }

        public Drive(Guid idRider, Guid idDriver, Guid idDrive, string fromLocation, string toLocation, double price, bool accepted, int minutesToDriverArr, int minuteEnd, bool isFinished, bool isRated) : this(idRider, idDriver, fromLocation, toLocation, price, accepted)
        {
            IdDrive = idDrive;
            TimeToDriverArrivalSeconds = minutesToDriverArr;
            TimeToEndTripInSeconds = minuteEnd;
            IsFinished = isFinished;
            IsRated = isRated;
        }
    }
}
