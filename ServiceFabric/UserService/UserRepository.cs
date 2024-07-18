using Common.Entity;
using Common.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService
{
    public class UserRepository
    {
        private CloudStorageAccount cloud;
        private CloudTableClient tableClient;
        private CloudTable _user;
        private CloudBlobClient blobClient;

        public CloudStorageAccount Cloud { get => cloud; set => cloud = value; }
        public CloudTableClient TableClient { get => tableClient; set => tableClient = value; }
        public CloudTable User { get => _user; set => _user = value; }
        public CloudBlobClient BlobClient { get => blobClient; set => blobClient = value; }


        public UserRepository(string table)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                Cloud = CloudStorageAccount.Parse(connectionString); 

                BlobClient = Cloud.CreateCloudBlobClient();

                TableClient = Cloud.CreateCloudTableClient();

                User = TableClient.GetTableReference(table);
                User.CreateIfNotExistsAsync().Wait();
            }
            catch (Exception ex) {
                throw;
            }
        }

        public async Task<CloudBlockBlob> GetBlobRef(string nameContainter, string nameBlob) { 
            CloudBlobContainer blobCont = blobClient.GetContainerReference(nameContainter);
            await blobCont.CreateIfNotExistsAsync();
            CloudBlockBlob b = blobCont.GetBlockBlobReference(nameBlob);
            return b;
        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            var query = new TableQuery<UserEntity>();
            var result = User.ExecuteQuerySegmentedAsync(query, null).GetAwaiter().GetResult();
            return result.Results;
        }

        public async Task<bool> EntityUpdate(Guid id, bool status)
        {
            TableQuery<UserEntity> q = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));
            TableQuerySegment<UserEntity> result = await User.ExecuteQuerySegmentedAsync(q, null);

            if (result.Results.Count > 0)
            {
                UserEntity user = result.Results[0];
                user.IsBlocked = status;
                var o = TableOperation.Replace(user);
                await User.ExecuteAsync(o);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateStatus(Guid id, string status)
        {
            TableQuery<UserEntity> usersQuery = new TableQuery<UserEntity>()
       .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));
            TableQuerySegment<UserEntity> result = await User.ExecuteQuerySegmentedAsync(usersQuery, null);


            if (result.Results.Count > 0)
            {
                UserEntity userTable = result.Results[0];
                userTable.Status = status;
                if (status == "Prihvaceno") userTable.IsVerified = true;
                else userTable.IsVerified = false;
                var operation = TableOperation.Replace(userTable);
                await User.ExecuteAsync(operation);

            }
        }

        public async Task UpdateRating(Guid id, int sumRating, int numRating, double avgRating)
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>()
       .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));
            TableQuerySegment<UserEntity> result = await User.ExecuteQuerySegmentedAsync(query, null);

            if (result.Results.Count > 0)
            {
                UserEntity userTable = result.Results[0];
                userTable.SumRating = sumRating;
                userTable.NumRating = numRating;
                userTable.AvgRating = avgRating;
                var operation = TableOperation.Replace(userTable);
                await User.ExecuteAsync(operation);

            }

        }

        public async Task UpdateUser(Network network, User user)
        {

            TableQuery<UserEntity> query = new TableQuery<UserEntity>()
       .Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, network.Id));

            TableQuerySegment<UserEntity> result = await User.ExecuteQuerySegmentedAsync(query, null);

            if (result.Results.Count > 0)
            {
                UserEntity userTable = result.Results[0];
                userTable.Email = user.Email;
                userTable.FirstName = user.FirstName;
                userTable.LastName = user.LastName;
                userTable.Address = user.Address;
                userTable.Birthday = user.Birthday;
                userTable.Username = user.Username;
                userTable.Username = user.Username;
                userTable.ImageUrl = user.ImageUrl;
                var operation = TableOperation.Replace(userTable);
                await User.ExecuteAsync(operation);
            }
        }

        public async Task<byte[]> DownloadImage(UserRepository userRepo, UserEntity user, string nameContainer)
        {

            CloudBlockBlob blob = await userRepo.GetBlobRef(nameContainer, $"image_{user.Id}");


            await blob.FetchAttributesAsync();

            long blobLength = blob.Properties.Length;

            byte[] byteArray = new byte[blobLength];
            await blob.DownloadToByteArrayAsync(byteArray, 0);

            return byteArray;

        }


    }
}
