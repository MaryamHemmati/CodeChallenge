using DataSample.Domain.Entities.Users;
using System.Security.Claims;

namespace EndPoint.API.Helper
{
    public interface IUser
    {
        string? Id { get; }
    }

    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        //public int GetCurrentUser()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    // will give the user's userId
        //    //userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName
        //    //email = User.FindFirstValue(ClaimTypes.Email);

        //}
    }

}
