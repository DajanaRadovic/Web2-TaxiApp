using Common.DTOs;
using Common.Interface;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Fabric;
using WebService.InterfaceWebService;

namespace WebService.Helpers
{
    public class DriverService : IDriverService
    {
        private readonly Uri _userServiceUri = new Uri("fabric:/ServiceFabric/UserService");
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

        public async Task<bool> ChnageDriverStatus(Guid id, bool status)
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
    }
}
