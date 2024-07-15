using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class DriverStatusModificationDTO
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }

        public DriverStatusModificationDTO() { }
    }
}
