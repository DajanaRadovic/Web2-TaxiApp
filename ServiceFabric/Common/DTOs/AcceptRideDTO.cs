using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class AcceptRideDTO
    {
        public Guid IdDriver { get; set; }
        public Guid IdRide { get; set; }

        public AcceptRideDTO() { }
    }
}
