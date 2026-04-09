using System.Security.Claims;

namespace FinanceManager.API.Helpers;

public static class UserHelper
{
    public static long GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new InvalidOperationException("User is authenticated but has no NameIdentifier claim.");

        return long.Parse(userIdClaim.Value);
    }
}
