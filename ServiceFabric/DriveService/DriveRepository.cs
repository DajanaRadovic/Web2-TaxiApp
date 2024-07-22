using Common.Entity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveService
{
    public class DriveRepository
    {
        private CloudStorageAccount cloud;
        private CloudTableClient tableClient;
        private CloudTable _drive;

        public CloudStorageAccount Cloud { get => cloud; set => cloud = value; }
        public CloudTableClient TableClient { get => tableClient; set => tableClient = value; }
        public CloudTable Drive { get => _drive; set => _drive = value; }

        public DriveRepository(string table)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("DataConnectionString");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("DataConnectionString environment variable is not set.");
                }

                Console.WriteLine("Connection string: " + connectionString);
                Cloud = CloudStorageAccount.Parse(connectionString);
                TableClient = Cloud.CreateCloudTableClient();
                Drive = TableClient.GetTableReference(table);

                Console.WriteLine("Attempting to create table: " + table);
                var result = Drive.CreateIfNotExistsAsync().Result;

                if (result)
                {
                    Console.WriteLine("Table created successfully.");
                }
                else
                {
                    Console.WriteLine("Table already exists.");
                }
            }
            catch (StorageException ex)
            {
                Console.WriteLine("StorageException: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                throw;
            }
            /*  try
              {
                  string connectionString = Environment.GetEnvironmentVariable("DataConnectionString");
                  Cloud = CloudStorageAccount.Parse(connectionString);
                  TableClient = Cloud.CreateCloudTableClient();
                  Drive = TableClient.GetTableReference(table);
                  Drive.CreateIfNotExistsAsync().Wait();

              }
              catch (Exception ex)
              {
                  throw;
              }*/
        }

        public IEnumerable<DriveEntity> GetAllDrives()
        {
            var query = new TableQuery<DriveEntity>();
            var result = Drive.ExecuteQuerySegmentedAsync(query, null).GetAwaiter().GetResult();
            return result.Results;
        }

        public async Task RateDrive(Guid idDrive)
        {
            TableQuery<DriveEntity> q = new TableQuery<DriveEntity>().Where(TableQuery.GenerateFilterConditionForGuid("IdDrive", QueryComparisons.Equal, idDrive));
            TableQuerySegment<DriveEntity> result = await Drive.ExecuteQuerySegmentedAsync(q, null);

            if (result.Results.Count > 0)
            {
                DriveEntity drive = result.Results[0];
                drive.IsRated = true;
                var o = TableOperation.Replace(drive);
                await Drive.ExecuteAsync(o);
            }
        }

        public async Task<bool> FinishDrive(Guid idDrive)
        {
            TableQuery<DriveEntity> q = new TableQuery<DriveEntity>().Where(TableQuery.GenerateFilterConditionForGuid("IdDrive", QueryComparisons.Equal, idDrive));
            TableQuerySegment<DriveEntity> result = await Drive.ExecuteQuerySegmentedAsync(q, null);

            if (result.Results.Count > 0)
            {
                DriveEntity drive = result.Results[0];
                drive.IsFinished = true;
                var o = TableOperation.Replace(drive);
                await Drive.ExecuteAsync(o);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Update(Guid idDriver, Guid idRide)
        {
            TableQuery<DriveEntity> q = new TableQuery<DriveEntity>().Where(TableQuery.GenerateFilterConditionForGuid("IdDrive", QueryComparisons.Equal, idRide));
            TableQuerySegment<DriveEntity> result = await Drive.ExecuteQuerySegmentedAsync(q, null);

            if (result.Results.Count > 0)
            {
                DriveEntity drive = result.Results[0];
                drive.Accepted = true;
                drive.TimeToEndTripInSeconds = 60;
                drive.IdDriver = idDriver;
                var o = TableOperation.Replace(drive);
                await Drive.ExecuteAsync(o);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
