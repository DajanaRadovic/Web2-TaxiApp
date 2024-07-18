using Common.DTOs;
using Common.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface
{
    [ServiceContract]
    public interface IUser:IService
    {
        [OperationContract]
        Task<bool> AddNewUser(User user);

        [OperationContract]
        Task<AuthenticatedUserDTO> LoginUser(LoginDTO loginDTO);

        [OperationContract]
        Task<List<UserDetailsDTO>> ListUsers();


        [OperationContract]
        Task<UserDetailsDTO> GetUser(Guid id);

        [OperationContract]
        Task<UserDetailsDTO> ChangeUser(Network user);

        [OperationContract]
        Task<List<DriverStatusDTO>> ListDrivers();

        [OperationContract]
        Task<bool> ChangeStatusDriver(Guid id, bool status);


        [OperationContract]
        Task<bool> VerifyDriver(Guid id, string email, string task);

        [OperationContract]
        Task<List<DriverStatusDTO>> NotVerifDrivers();
    }
}
