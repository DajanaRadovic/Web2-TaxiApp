using Common.DTOs;
using Common.Model;

namespace WebService.InterfaceWebService
{
    public interface IUserService
    {
        Task<bool> RegisterUser(Signup user);
        Task<List<UserDetailsDTO>> GetUsers();
        Task<AuthenticatedUserDTO> LoginUser(LoginDTO user);
        Task<UserDetailsDTO> GetUserDetails(Guid id);
        Task<UserDetailsDTO> UpdateUser(UpdateUser user);
    }
}
