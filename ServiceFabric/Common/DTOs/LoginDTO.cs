using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [DataContract]
    public class LoginDTO
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        public LoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
