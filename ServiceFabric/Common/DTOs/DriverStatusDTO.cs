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
    public class DriverStatusDTO
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public double AvgRating { get; set; }

        [DataMember]
        public bool IsBlocked { get; set; }

        [DataMember]
        public StatusEnum.Statusi Status { get; set; }

        public DriverStatusDTO(Guid id, string username, string name, string lastName, string email, double avgRating, bool isBlocked, StatusEnum.Statusi status)
        {
            Id = id;
            Username = username;
            Name = name;
            LastName = lastName;
            Email = email;
            AvgRating = avgRating;
            IsBlocked = isBlocked;
            Status = status;
        }
    }
}
