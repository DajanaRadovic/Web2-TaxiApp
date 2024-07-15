using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class AcceptRides
    {
        public string ToLocation { get; set; }//destination
        public string FromLocation { get; set; }
        public Guid IdRider { get; set; }
        public double Price { get; set; }
        public bool Accepted { get; set; }
        public int Minutes { get; set; }

        public AcceptRides() { }
    }
}
