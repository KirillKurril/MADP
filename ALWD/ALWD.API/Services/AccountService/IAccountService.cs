using ALWD.Domain.Models;

namespace ALWD.API.Services.AccountService
{
    public interface IAccountService
    {
        Task<ResponseData<bool>> UpdateAvatar(string userUri, string newAvatarUri);
    }
}
