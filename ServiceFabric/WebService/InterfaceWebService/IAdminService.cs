using Common.DTOs;
using Common.Model;

namespace WebService.InterfaceWebService
{
    public interface IAdminService
    {
       
       // Task<Drive> AcceptGivenDrive(AcceptRides acceptRides);
       // Task<Drive> AcceptNewDrive(Guid idRide, Guid idDriver);
        Task<List<DriverStatusDTO>> DriverForVerification();
        Task<List<Drive>> GetFinishedRidesAdmin();
        Task<List<DriverStatusDTO>> AllDrivers();
        Task<bool> ChangeDriverStatus(Guid id, bool status);
        Task<bool> VerifyDriver(Guid driverId, string email, string task);

    }
}
