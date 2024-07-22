using Common.DTOs;
using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Fabric;
using WebService.InterfaceWebService;

namespace WebService.Helpers
{
    public class DriverService : IDriverService
    {
        private readonly Uri _driverServiceUri = new Uri("fabric:/ServiceFabric/DriverService");
        private readonly Uri _userServiceUri = new Uri("fabric:/ServiceFabric/UserService");

        public async Task<Drive> AcceptNewDrive(AcceptRideDTO ride)
        {
            var fabricClient = new FabricClient();
            Drive result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.AcceptDriveDriver(ride.IdRide, ride.IdDriver);
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }

        public async Task<List<Drive>> GetAllNotFinishedRides()
        {
            var fabricClient = new FabricClient();
            List<Drive> result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.GetDrives();
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }

        public async Task<Drive> GetCurrentRideDriver(Guid idDriver)
        {
            var fabricClient = new FabricClient();
            Drive result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var partRes = await proxy.GetCurrentRideDriver(idDriver);
                if (partRes != null)
                {
                    result = partRes;
                    break;
                }
            }

            return result;
        }

        public async Task<List<Drive>> GetFinishedRidesDriver(Guid idDriver)
        {
            var fabricClient = new FabricClient();
            List<Drive> result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.GetRidesForDriver(idDriver);
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }

       

        public async Task<List<Drive>> GetFinishedRidesRider(Guid idRider)
        {
            var fabricClient = new FabricClient();
            List<Drive> result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.GetRidesForRider(idRider);
                if (res != null)
                {
                    result = res;
                    break;
                }
            }

            return result;
        }
    }
}
