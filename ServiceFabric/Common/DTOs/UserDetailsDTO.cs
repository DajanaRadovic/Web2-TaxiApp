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
    public class UserDetailsDTO
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public DateTime Birthday { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public double AvgRating { get; set; }

        [DataMember]
        public int SumRating { get; set; }

        [DataMember]
        public int NumRating { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public bool IsBlocked { get; set; }

        [DataMember]
        public Roles.Role TypeUser { get; set; }

        [DataMember]
        public UploadDTO Image { get; set; }

        [DataMember]
        public StatusEnum.Statusi Status { get; set; }

        public UserDetailsDTO(Guid id, string username, string password, string lastName, string firstName, string email, DateTime birthday, string address, double avgRating, int sumRating, int numRating, bool isVerified, bool isBlocked, Roles.Role typeUser, UploadDTO image, StatusEnum.Statusi status)
        {
            Id = id;
            Username = username;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            Birthday = birthday;
            Address = address;
            AvgRating = avgRating;
            SumRating = sumRating;
            NumRating = numRating;
            IsVerified = isVerified;
            IsBlocked = isBlocked;
            TypeUser = typeUser;
            Image = image;
            Status = status;

        }
        public UserDetailsDTO() { }
    }
}
