using Common.DTOs;
using Common.Entity;
using Common.Enums;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums.StatusEnum;

namespace Common.Mapper
{
    public class UserMapper
    {
        public static User MapUserEntity(UserEntity userEntity, byte[] image)
        {

            var status = userEntity.Status;
            Statusi myStatusEnum;

            if (Enum.TryParse(status, out myStatusEnum))
            {
                return new User(
                     userEntity.Id,
                     userEntity.Username,
                     userEntity.Password,
                     userEntity.LastName,
                     userEntity.FirstName,
                     userEntity.Email,
                     userEntity.Birthday,
                     userEntity.Address,
                     userEntity.AvgRating,
                     userEntity.SumRating,
                     userEntity.NumRating,
                     userEntity.IsVerified,
                     userEntity.IsBlocked,
                     (Roles.Role)Enum.Parse(typeof(Roles.Role), userEntity.PartitionKey),
                    new UploadDTO(image),
                    userEntity.ImageUrl,
                    myStatusEnum
                 );
            }
            return null;
        }
    }
}
