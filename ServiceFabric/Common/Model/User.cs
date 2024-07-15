using Common.DTOs;
using Common.Enums;
using Common.FileOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [DataContract]
    public class User
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

        public string ImageUrl { get; set; }

        public User(Signup signup)
        {
            FirstName = signup.FirstName;
            LastName = signup.LastName;
            Birthday = DateTime.ParseExact(signup.Birthday, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Address = signup.Address;
            Email = signup.Email;
            Password = signup.Password;
            TypeUser = Enum.TryParse<Roles.Role>(signup.TypeUser, true, out var role) ? role : Roles.Role.Rider;
            Username = signup.Username;
            Id = Guid.NewGuid();
            switch (TypeUser)
            {
                case Roles.Role.Admin:
                    IsVerified = true;
                    break;
                case Roles.Role.Rider:
                    IsVerified = true;
                    break;
                case Roles.Role.Driver:
                    AvgRating = 0.0;
                    IsVerified = false;
                    NumRating = 0;
                    SumRating = 0;
                    IsBlocked = false;
                    Status = StatusEnum.Statusi.UProcesu;
                    break;

            }
            Image = FileHelper.UploadFileOverNetwork(signup.ImageUrl);
        }
        public User() { }

        public User(Guid id, string username, string password, string lastName, string firstName, string email, DateTime birthday, string address, double averageRating, int sumOfRatings, int numOfRatings, bool isVerified, bool isBlocked, Roles.Role typeUser, UploadDTO imageFile)
        {
            Id = id;
            Username = username;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            Birthday = birthday;
            Address = address;
            AvgRating = averageRating;
            SumRating = sumOfRatings;
            NumRating = numOfRatings;
            IsVerified = isVerified;
            IsBlocked = isBlocked;
            TypeUser = typeUser;
            Image = imageFile;
        }

        public User(Guid id, string username, string password, string lastName, string firstName, string email, DateTime birthday, string address, double averageRating, int sumOfRatings, int numOfRatings, bool isVerified, bool isBlocked, Roles.Role typeUser, UploadDTO imageFile, string imageUrl, StatusEnum.Statusi st)
       : this(id, username, password, lastName, firstName, email, birthday, address, averageRating, sumOfRatings, numOfRatings, isVerified, isBlocked, typeUser, imageFile)
        {
            Id = id;
            Username = username;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            Birthday = birthday;
            Address = address;
            AvgRating = averageRating;
            SumRating = sumOfRatings;
            NumRating = numOfRatings;
            IsVerified = isVerified;
            IsBlocked = isBlocked;
            TypeUser = typeUser;
            Image = imageFile;
            ImageUrl = imageUrl;
            Status = st;

        }
    }
}
