using Common.DTOs;
using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Fabric;
using WebService.InterfaceWebService;

namespace WebService.Helpers
{
    public class UserService : IUserService
    {
        private readonly Uri _userServiceUri = new Uri("fabric:/ServiceFabric/UserService");
        private readonly IEmailSender _emailSender;

   
        public async Task<UserDetailsDTO> GetUserDetails(Guid id)
        {
            try
            {
                var fabricClient = new FabricClient();
                UserDetailsDTO result = null;

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    var partitionResult = await proxy.GetUser(id);
                    if (partitionResult != null) {
                        result = partitionResult;
                        break;
                    }
                }
                if (result == null)
                {
                    throw new Exception("User not found");
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserDetailsDTO>> GetUsers()
        {
            try
            {
                var fabricClient = new FabricClient();
                var result = new List<UserDetailsDTO>();

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    var partitionResult = await proxy.ListUsers();
                    result.AddRange(partitionResult);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AuthenticatedUserDTO> LoginUser(LoginDTO user)
        {
            try
            {
                var fabricClient = new FabricClient();
                AuthenticatedUserDTO result = null;

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    var partitionResult = await proxy.LoginUser(user);

                    if (partitionResult != null)
                    {
                        result = partitionResult;
                        break;
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RegisterUser(Signup user)
        {
            try
            {
                User userForRegister = new User(user);

                var fabricClient = new FabricClient();
                bool result = false;

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    result = await proxy.AddNewUser(userForRegister);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserDetailsDTO> UpdateUser(UpdateUser user)
        {
            Network network = new Network(user);
            try
            {
               
                var fabricClient = new FabricClient();
                UserDetailsDTO result = null;

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    var partitionResult = await proxy.ChangeUser(network);

                    if (partitionResult != null)
                    {
                        result = partitionResult;
                        break;
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }

        }
       
    }
}
