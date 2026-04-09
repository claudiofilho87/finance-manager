using FinanceManager.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[Authorize]
public class AuthenticatedController : ControllerBase
{
    protected long UserId => UserHelper.GetUserId(User);
}