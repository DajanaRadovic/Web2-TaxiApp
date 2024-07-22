using Common.DTOs;
using Common.Model;

namespace WebService.InterfaceWebService
{
    public interface IDriverService
    {
        Task<List<Drive>> GetAllNotFinishedRides();
        Task<List<Drive>> GetFinishedRidesDriver(Guid idDriver);
        Task<Drive> AcceptNewDrive(AcceptRideDTO ride);
        Task<Drive> GetCurrentRideDriver(Guid idDriver);

    }
}
