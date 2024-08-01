using Common.DTOs;
using Common.Interface;
using Common.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Fabric;
using WebService.InterfaceWebService;

namespace WebService.Helpers
{
    public class AdminService : IAdminService
    {
        private readonly Uri _driverServiceUri = new Uri("fabric:/ServiceFabric/DriveService");
        private readonly Uri _userServiceUri = new Uri("fabric:/ServiceFabric/UserService");

       // private readonly IEmailSender _emailSender;
       private readonly IEmail _emailSender;
       

        public async Task<List<DriverStatusDTO>> AllDrivers()
        {
            try
            {
                var fabricClient = new FabricClient();
                List<DriverStatusDTO> result = null;

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    var partitionResult = await proxy.ListDrivers();
                    if (partitionResult != null)
                    {
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

        public async Task<Drive> AcceptNewDrive(Guid idRide, Guid idDriver)
        {
            var fabricClient = new FabricClient();
            Drive result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.AcceptDriveDriver(idRide, idDriver);
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }

        public async Task<List<DriverStatusDTO>> DriverForVerification()
        {
            var fabricClient = new FabricClient();
            List<DriverStatusDTO> result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                var res = await proxy.NotVerifDrivers();
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }

        public async Task<List<Drive>> GetFinishedRidesAdmin()
        {
            var fabricClient = new FabricClient();
            List<Drive> result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.GetRidesAdmin();
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }

        public async Task<bool> ChangeDriverStatus(Guid id, bool status)
        {
            try
            {
                var fabricClient = new FabricClient();
                bool result = false;

                var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
                foreach (var partition in partitionList)
                {
                    var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                    var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                    result = await proxy.ChangeStatusDriver(id, status);

                    if (result) break;
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> VerifyDriver(Guid driverId, string email, string task)
        {
            var fabricClient = new FabricClient();
            bool result = false;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_userServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IUser>(_userServiceUri, partitionKey);
                var res = await proxy.VerifyDriver(driverId, email, task);
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            if (result && task == "Prihvaceno")
            {
                await _emailSender.SendEmail(email, "Account verification", "Successfuly verified on taxi app now you can drive!");
            }

            return result;
        }
    }
}
