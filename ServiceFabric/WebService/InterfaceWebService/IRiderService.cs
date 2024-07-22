using Common.Model;

namespace WebService.InterfaceWebService
{
    public interface IRiderService
    {
        Task<List<Drive>> GetFinishedRidesRider(Guid idRider);
        Task<Drive> AcceptGivenDrive(AcceptRides acceptRides);
        Task<bool> Rating(Guid idDrive, int rating);
        Task<List<Drive>> GetNotRated();
        Task<Drive> GetCurrentDrive(Guid idDrive);
    }
}
