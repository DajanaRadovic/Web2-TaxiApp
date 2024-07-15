using Common.DTOs;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper
{
    public class FullUserMapper
    {
        public static UserDetailsDTO MapUserDetailsDto(User user)
        {
            return new UserDetailsDTO(user.Id, user.Username, user.Password, user.LastName, user.FirstName, user.Email, user.Birthday, user.Address, user.AvgRating, user.SumRating, user.NumRating, user.IsVerified, user.IsBlocked, user.TypeUser, user.Image, user.Status);
        }
    }
}
