using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Fabric;
using WebService.InterfaceWebService;

namespace WebService.Helpers
{
    public class RiderService : IRiderService
    {
        private readonly Uri _driverServiceUri = new Uri("fabric:/ServiceFabric/DriverService");
        private readonly Uri _userServiceUri = new Uri("fabric:/ServiceFabric/UserService");

        public async Task<Drive> AcceptGivenDrive(AcceptRides acceptRides)
        {
            if (string.IsNullOrEmpty(acceptRides.ToLocation))
                throw new ArgumentException("You must send destination!");

            if (string.IsNullOrEmpty(acceptRides.FromLocation))
                throw new ArgumentException("You must send location!");

            if (acceptRides.Accepted)
                throw new ArgumentException("Ride cannot be automatically accepted!");

            if (acceptRides.Price <= 0.0)
                throw new ArgumentException("Invalid price!");

            var fabricClient = new FabricClient();
            Drive res = null;
            Drive drive = new Drive(acceptRides.IdRider, acceptRides.FromLocation, acceptRides.ToLocation, acceptRides.Price, acceptRides.Accepted, acceptRides.Minutes);

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var partitionResult = await proxy.AcceptDrive(drive);
                if (partitionResult != null)
                {
                    res = partitionResult;
                    break;
                }
            }

            if (res == null)
            {
                throw new Exception("You already submitted ticket!");
            }

            return res;
        }

        public async Task<Drive> GetCurrentDrive(Guid idDrive)
        {
            var fabricClient = new FabricClient();
            Drive result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.GetCurrentRide(idDrive);
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

        public async Task<List<Drive>> GetNotRated()
        {
            var fabricClient = new FabricClient();
            List<Drive> result = null;

            var list = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in list)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var partitionResult = await proxy.GetAllNotRatedRides();
                if (partitionResult != null)
                {
                    result = partitionResult;
                    break;
                }
            }

            return result;
        }

        public async Task<bool> Rating(Guid idDrive, int rating)
        {
            var fabricClient = new FabricClient();
            bool result = false;

            var partitionList = await fabricClient.QueryManager.GetPartitionListAsync(_driverServiceUri);
            foreach (var partition in partitionList)
            {
                var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                var proxy = ServiceProxy.Create<IDrive>(_driverServiceUri, partitionKey);
                var res = await proxy.SubmitFeedback(idDrive, rating);
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
