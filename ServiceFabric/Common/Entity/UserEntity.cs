using Common.Enums;
using Common.Model;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    public class UserEntity : TableEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public double AvgRating { get; set; }

        public int SumRating { get; set; }

        public int NumRating { get; set; }

        public bool IsVerified { get; set; }

        public bool IsBlocked { get; set; }

        public string TypeUser { get; set; }

        public string Status { get; set; }

        public string ImageUrl { get; set; }

        public UserEntity(User user, string urlImage)
        {
            RowKey = user.Username; //Kljuc za user-a
            PartitionKey = user.TypeUser.ToString(); //tip korisnika-partition key
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            LastName = user.LastName;
            FirstName = user.FirstName;
            Email = user.Email;
            Birthday = user.Birthday;
            Address = user.Address;
            AvgRating = user.AvgRating;
            SumRating = user.SumRating;
            NumRating = user.NumRating;
            IsVerified = user.IsVerified;
            IsBlocked = user.IsBlocked;
            TypeUser = user.TypeUser.ToString();
            Status = user.Status.ToString();
            ImageUrl = urlImage;
        }

        public UserEntity(User user)
        {
            RowKey = user.Username; //Kljuc za user-a
            PartitionKey = user.TypeUser.ToString(); //tip korisnika - partiton key
           // Id = user.Id; 
            Username = user.Username;
            Password = user.Password;
            LastName = user.LastName;
            FirstName = user.FirstName;
            Email = user.Email;
            Birthday = user.Birthday;
            Address = user.Address;
            AvgRating = user.AvgRating;
            SumRating = user.SumRating;
            NumRating = user.NumRating;
            IsVerified = user.IsVerified;
            IsBlocked = user.IsBlocked;
            TypeUser = user.TypeUser.ToString();
            Status = user.Status.ToString();
            ImageUrl = user.ImageUrl;
        }

        public UserEntity() { }
    }
}
