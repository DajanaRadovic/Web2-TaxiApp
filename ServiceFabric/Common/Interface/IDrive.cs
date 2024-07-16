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
    public interface IDrive:IService
    {
        [OperationContract]
        Task<Drive> AcceptDrive(Drive drive);

        [OperationContract]
        Task<Drive> GetCurrentDriving(Guid id);

        [OperationContract]
        Task<List<Drive>> GetDrives();

        [OperationContract]
        Task<Drive> AcceptDriveDriver(Guid idRide, Guid idDriver);

        [OperationContract]
        Task<List<Drive>> GetRidesForDriver(Guid idDriver);

        [OperationContract]
        Task<List<Drive>> GetRidesForRider(Guid idDriver);

        [OperationContract]
        Task<List<Drive>> GetRidesAdmin();

        [OperationContract]
        Task<Drive> GetCurrentRide(Guid id);

        [OperationContract]
        Task<Drive> GetCurrentRideDriver(Guid id);

        [OperationContract]
        Task<List<Drive>> GetAllNotRatedRides();

        [OperationContract]
        Task<bool> SubmitFeedback(Guid idTrip, int rating);
    }
}
