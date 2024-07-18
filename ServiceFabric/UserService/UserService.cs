using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.DTOs;
using Common.Entity;
using Common.Enums;
using Common.Interface;
using Common.Mapper;
using Common.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using static System.Collections.Specialized.BitVector32;

namespace UserService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public sealed class UserService : StatefulService, IUser, IRateable
    {

        public UserRepository userRepo;
        public UserService(StatefulServiceContext context)
            : base(context)
        {
            userRepo = new UserRepository("UsersServiceFavric");
        }

        public async Task<bool> AddNewUser(User user)
        {
            var userDict = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");

            try
            {
                using (var t = StateManager.CreateTransaction())
                {
                    if (!await IfUserAlreadyExists(user))
                    {

                        await userDict.AddAsync(t, user.Id, user);

                        CloudBlockBlob blob = await userRepo.GetBlobRef("users", $"image_{user.Id}");
                        blob.Properties.ContentType = user.Image.TypeContent;
                        await blob.UploadFromByteArrayAsync(user.Image.File, 0, user.Image.File.Length);
                        string imageUrl = blob.Uri.AbsoluteUri;

                        UserEntity userEntity = new UserEntity(user, imageUrl);
                        TableOperation o = TableOperation.Insert(userEntity);
                        await userRepo.User.ExecuteAsync(o);

                        await t.CommitAsync();
                        return true;
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> IfUserAlreadyExists(User user)
        {
            var users = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {
                    var enumm = await users.CreateEnumerableAsync(t);

                    using (var pom = enumm.GetAsyncEnumerator())
                    {
                        while (await pom.MoveNextAsync(default(CancellationToken)))
                        {
                            if (pom.Current.Value.Email == user.Email || pom.Current.Value.Id == user.Id || pom.Current.Value.Username == user.Username)
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

        public async Task<bool> AddRating(Guid idDriver, int rating)
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            bool o = false;
            using (var t = this.StateManager.CreateTransaction())
            {
                var enumm = await users.CreateEnumerableAsync(t);
                using (var pom = enumm.GetAsyncEnumerator())
                {
                    while (await pom.MoveNextAsync(default(CancellationToken)))
                    {
                        if (pom.Current.Value.Id == idDriver)
                        {
                            var user = pom.Current.Value;
                            user.NumRating++;
                            user.SumRating += rating;
                            user.AvgRating = (double)user.SumRating / (double)user.NumRating;


                            await users.SetAsync(t, idDriver, user);

                            await userRepo.UpdateRating(user.Id, user.SumRating, user.NumRating, user.AvgRating);

                            await t.CommitAsync();

                            o = true;
                            break;
                        }
                    }
                }
            }

            return o;
        }

        public async Task<bool> ChangeStatusDriver(Guid id, bool status)
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            using (var t = this.StateManager.CreateTransaction())
            {
                ConditionalValue<User> result = await users.TryGetValueAsync(t, id);
                if (result.HasValue)
                {
                    User user = result.Value;
                    user.IsBlocked = status;
                    await users.SetAsync(t, id, user);

                    await userRepo.EntityUpdate(id, status);

                    await t.CommitAsync();

                    return true;
                }
                else return false;


            }
        }

        public async Task<UserDetailsDTO> ChangeUser(Network user)
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("");
            using (var tx = this.StateManager.CreateTransaction())
            {
                ConditionalValue<User> result = await users.TryGetValueAsync(tx, user.Id);
                if (result.HasValue)
                {
                    User userFromReliable = result.Value;

                    if (!string.IsNullOrEmpty(user.Email)) userFromReliable.Email = user.Email;

                    if (!string.IsNullOrEmpty(user.FirstName)) userFromReliable.FirstName = user.FirstName;

                    if (!string.IsNullOrEmpty(user.LastName)) userFromReliable.LastName = user.LastName;

                    if (!string.IsNullOrEmpty(user.Address)) userFromReliable.Address = user.Address;

                    if (user.Birthday != DateTime.MinValue) userFromReliable.Birthday = user.Birthday;

                    if (!string.IsNullOrEmpty(user.Password)) userFromReliable.Password = user.Password;

                    if (!string.IsNullOrEmpty(user.Username)) userFromReliable.Username = user.Username;

                    if (user.Image != null && user.Image.File != null && user.Image.File.Length > 0) userFromReliable.Image = user.Image;

                    await users.TryRemoveAsync(tx, user.Id); // ukloni ovog proslog 

                    await users.AddAsync(tx, userFromReliable.Id, userFromReliable); // dodaj ga prvo u reliable 

                    if (user.Image != null && user.Image.File != null && user.Image.File.Length > 0) // ako je promenjena slika u reliable upisi je i u blob 
                    {
                        CloudBlockBlob blob = await userRepo.GetBlobRef("users", $"image_{userFromReliable.Id}"); // nadji prethodni blok u blobu
                        await blob.DeleteIfExistsAsync(); // obrisi ga 

                        CloudBlockBlob newblob = await userRepo.GetBlobRef("users", $"image_{userFromReliable.Id}"); // kreiraj za ovaj novi username
                        newblob.Properties.ContentType = userFromReliable.Image.TypeContent;
                        await newblob.UploadFromByteArrayAsync(userFromReliable.Image.File, 0, userFromReliable.Image.TypeContent.Length); // upload novu sliku 
                    }

                    await userRepo.UpdateUser(user, userFromReliable); // sacuva ga u bazu 
                    await tx.CommitAsync();
                    return FullUserMapper.MapUserDetailsDto(userFromReliable);

                }
                else return null;
            }
        }

        public async Task<UserDetailsDTO> GetUser(Guid id)
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            using (var t = this.StateManager.CreateTransaction())
            {
                ConditionalValue<User> result = await users.TryGetValueAsync(t, id);
                if (result.HasValue)
                {
                    UserDetailsDTO user = FullUserMapper.MapUserDetailsDto(result.Value);
                    return user;
                }
                else return new UserDetailsDTO();


            }
        }

        public async Task<List<DriverStatusDTO>> ListDrivers()
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            List<DriverStatusDTO> drivers = new List<DriverStatusDTO>();
            using (var t = this.StateManager.CreateTransaction())
            {
                var enumm = await users.CreateEnumerableAsync(t);
                using (var pom = enumm.GetAsyncEnumerator())
                {
                    while (await pom.MoveNextAsync(default(CancellationToken)))
                    {
                        if (pom.Current.Value.TypeUser == Roles.Role.Driver)
                        {
                            drivers.Add(new DriverStatusDTO(pom.Current.Value.Id, pom.Current.Value.Username, pom.Current.Value.FirstName, pom.Current.Value.LastName, pom.Current.Value.Email, pom.Current.Value.AvgRating, pom.Current.Value.IsBlocked, pom.Current.Value.Status));
                        }
                    }
                }
            }

            return drivers;
        }

        public async Task<List<UserDetailsDTO>> ListUsers()
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");

            List<UserDetailsDTO> userList = new List<UserDetailsDTO>();

            using (var t = this.StateManager.CreateTransaction())
            {
                var enumm = await users.CreateEnumerableAsync(t);

                using (var pom = enumm.GetAsyncEnumerator())
                {
                    while (await pom.MoveNextAsync(default(CancellationToken)))
                    {
                        userList.Add(FullUserMapper.MapUserDetailsDto(pom.Current.Value));
                    }
                }
            }

            return userList;
        }

        public async Task<AuthenticatedUserDTO> LoginUser(LoginDTO loginDTO)
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");

            using (var t = this.StateManager.CreateTransaction())
            {
                var enumm = await users.CreateEnumerableAsync(t);

                using (var pom = enumm.GetAsyncEnumerator())
                {
                    while (await pom.MoveNextAsync(default(CancellationToken)))
                    {
                        if (pom.Current.Value.Email == loginDTO.Email && pom.Current.Value.Password == loginDTO.Password)
                        {
                            return new AuthenticatedUserDTO(pom.Current.Value.Id, pom.Current.Value.TypeUser);
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<DriverStatusDTO>> NotVerifDrivers()
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            List<DriverStatusDTO> listDrivers = new List<DriverStatusDTO>();
            using (var t = this.StateManager.CreateTransaction())
            {
                var enumm = await users.CreateEnumerableAsync(t);
                using (var pom = enumm.GetAsyncEnumerator())
                {
                    while (await pom.MoveNextAsync(default(CancellationToken)))
                    {
                        if (pom.Current.Value.TypeUser == Roles.Role.Driver && pom.Current.Value.Status != StatusEnum.Statusi.Odbijeno)
                        {
                            listDrivers.Add(new DriverStatusDTO(pom.Current.Value.Id, pom.Current.Value.Username, pom.Current.Value.FirstName, pom.Current.Value.LastName, pom.Current.Value.Email, pom.Current.Value.AvgRating, pom.Current.Value.IsBlocked, pom.Current.Value.Status));
                        }
                    }
                }
            }

            return listDrivers;
        }

        public async Task<bool> VerifyDriver(Guid id, string email, string task)
        {
            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            using (var t = this.StateManager.CreateTransaction())
            {
                ConditionalValue<User> res = await users.TryGetValueAsync(t, id);
                if (res.HasValue)
                {
                    User change = res.Value;
                    if (task == "Prihvaceno")
                    {
                        change.IsVerified = true;
                        change.Status = StatusEnum.Statusi.Prihvaceno;
                    }
                    else change.Status = StatusEnum.Statusi.Odbijeno;

                    await users.SetAsync(t, id, change);

                    await userRepo.UpdateStatus(id, task);

                    await t.CommitAsync();
                    return true;

                }
                else return false;
            }
        }

        private async Task LoadUsers()
        {
            var userDictionary = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            try
            {
                using (var t = StateManager.CreateTransaction())
                {
                    var users = userRepo.GetAllUsers();
                    if (users.Count() == 0) return;
                    else
                    {
                        foreach (var user in users)
                        {
                            byte[] image = await userRepo.DownloadImage(userRepo, user, "users");
                            await userDictionary.AddAsync(t, user.Id, UserMapper.MapUserEntity(user, image));
                        }
                    }

                    await t.CommitAsync();

                }

            }
            catch (Exception)
            {
                throw;
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
        => this.CreateServiceRemotingReplicaListeners();

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

            var users = await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, User>>("Users");
            await LoadUsers();

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
