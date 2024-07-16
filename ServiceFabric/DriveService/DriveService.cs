using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Entity;
using Common.Interface;
using Common.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.WindowsAzure.Storage.Table;

namespace DriveService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class DriveService : StatefulService, IDrive
    {
        DriveRepository driveRepo;
        public DriveService(StatefulServiceContext context)
            : base(context)
        {
            driveRepo = new DriveRepository("ServiceFabricDriving");
        }

        public async Task<Drive> AcceptDrive(Drive drive)
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {
                    if (!await DriveTripCheck(drive))
                    {
                        var pom = await drives.CreateEnumerableAsync(t);

                        using (var enumm = pom.GetAsyncEnumerator())
                        {

                            await drives.AddAsync(t, drive.IdDrive, drive);
                            DriveEntity entity = new DriveEntity(drive.IdRider, drive.IdDriver, drive.IdDrive, drive.FromLocation, drive.ToLocation, drive.Price, drive.Accepted, drive.TimeToDriverArrivalSeconds);
                            TableOperation operation = TableOperation.Insert(entity);
                            await driveRepo.Drive.ExecuteAsync(operation);

                            ConditionalValue<Drive> result = await drives.TryGetValueAsync(t, drive.IdDrive);
                            await t.CommitAsync();
                            return result.Value;

                        }

                    }
                    else return null;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> DriveTripCheck(Drive drive)
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var pom = await drives.CreateEnumerableAsync(t);

                    using (var enumm = pom.GetAsyncEnumerator())
                    {
                        while (await enumm.MoveNextAsync(default(CancellationToken)))
                        {
                            if ((enumm.Current.Value.IdRider == drive.IdRider && enumm.Current.Value.Accepted == false)) 
                            {                                                                                                   
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Drive> AcceptDriveDriver(Guid idRide, Guid idDriver)
        {
            var drive = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            Guid compare = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {
                    ConditionalValue<Drive> res = await drive.TryGetValueAsync(t, idRide);

                    if (res.HasValue && res.Value.IdDriver == compare)
                    {
                        // azuriranje 
                        Drive accept = res.Value;
                        accept.TimeToEndTripInSeconds = 60;
                        accept.IdDriver = idDriver;
                        accept.Accepted = true;
                        await drive.SetAsync(t, accept.IdRider, accept);
                        if (await driveRepo.Update(idDriver, idRide))
                        {
                            await t.CommitAsync();
                            return accept;
                        }
                        else return null;
                    }
                    else return null;

                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Drive>> GetAllNotRatedRides()
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            List<Drive> list = new List<Drive>();
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drives.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if (!pom.Current.Value.IsRated && pom.Current.Value.IsFinished)
                            {
                                list.Add(pom.Current.Value);
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Drive> GetCurrentDriving(Guid id)
        {
            var drive = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drive.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if ((pom.Current.Value.IdRider == id && pom.Current.Value.IsFinished == false))
                            {
                                return pom.Current.Value;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Drive> GetCurrentRide(Guid id)
        {
            var drive = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drive.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if ((pom.Current.Value.IdRider == id && pom.Current.Value.IsFinished == false))
                            {
                                return pom.Current.Value;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Drive> GetCurrentRideDriver(Guid id)
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drives.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if ((pom.Current.Value.IdDriver == id && pom.Current.Value.IsFinished == false))
                            {
                                return pom.Current.Value;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Drive>> GetDrives()
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            List<Drive> list = new List<Drive>();
            Guid compare = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drives.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if (pom.Current.Value.IdDriver == compare)
                            {
                                list.Add(pom.Current.Value);
                            }
                        }
                    }
                    await t.CommitAsync();
                }

                return list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Drive>> GetRidesAdmin()
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            List<Drive> list = new List<Drive>();
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drives.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {

                            list.Add(pom.Current.Value);
                        }
                    }
                    await t.CommitAsync();
                }

                return list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Drive>> GetRidesForDriver(Guid idDriver)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Drive>> GetRidesForRider(Guid idDriver)
        {
            var drives = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            List<Drive> list = new List<Drive>();
            try
            {
                using (var t = StateManager.CreateTransaction())
                {

                    var enumm = await drives.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if (pom.Current.Value.IdRider == idDriver && pom.Current.Value.IsFinished)
                            {
                                list.Add(pom.Current.Value);
                            }
                        }
                    }
                    await t.CommitAsync();
                }

                return list;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SubmitFeedback(Guid idTrip, int rating)
        {
            var drive = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Drive>>("Drives");
            bool result = false;
            var fabricClient = new FabricClient();
            try
            {
                using (var t = StateManager.CreateTransaction())
                {
                    var pom = await drive.TryGetValueAsync(t, idTrip);
                    if (!pom.HasValue)
                    {
                        return false;
                    }

                    Guid idDriver = pom.Value.IdDriver;
                    var list = await fabricClient.QueryManager.GetPartitionListAsync(new Uri("fabric:/ServiceFabric/UserService"));

                    foreach (var partition in list)
                    {
                        var partitionKey = new ServicePartitionKey(((Int64RangePartitionInformation)partition.PartitionInformation).LowKey);
                        var proxy = ServiceProxy.Create<IRateable>(new Uri("fabric:/ServiceFabric/UserService"), partitionKey);

                        try
                        {
                            var resultP = await proxy.AddRating(idDriver, rating);
                            if (resultP)
                            {
                                result = true;
                                pom.Value.IsRated = true;
                                await drive.SetAsync(t, pom.Value.IdDriver, pom.Value);
                                await driveRepo.RateDrive(pom.Value.IdDriver);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    await t.CommitAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ApplicationException($"Failed to submit rating for TripId: {idTrip}", ex);
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
