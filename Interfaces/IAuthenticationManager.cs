using System.Threading.Tasks;
using Entities.DataTransferObjects;

namespace Interfaces
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserAuthenticationDto userForAuth);
        Task<string> CreateToken();
    }
}