using Common.DTOs;

namespace WebService.InterfaceWebService
{
    public interface IDriverService
    {
        Task<List<DriverStatusDTO>> AllDrivers();
        Task<bool> ChangeDriverStatus(Guid id, bool status); 

    }
}
