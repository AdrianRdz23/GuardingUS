using System.Security.Claims;

namespace GuardingUS.Services
{
    public interface IUserService
    {
        int GetUserId();
    }
    public class UserService : IUserService
    {
        private readonly HttpContext httpContext;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUserId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);

                return id;
            }
            else
            {
                throw new ApplicationException("The user is not authenticated");
            }
        }
    }
}
