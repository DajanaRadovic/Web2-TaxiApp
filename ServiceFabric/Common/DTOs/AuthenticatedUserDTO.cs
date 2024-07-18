using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [DataContract]
    public class AuthenticatedUserDTO
    {

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public Roles.Role Roles { get; set; }

        public AuthenticatedUserDTO(Guid id, Roles.Role roles) {
            Id = id;
            Roles = roles;
        }
       /* public Guid Id { get; set; }
        public string Email { get; set; }
        public string Task { get; set; }*/

    }
}
