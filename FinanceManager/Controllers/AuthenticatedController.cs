using FinanceManager.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Controllers;

[Authorize]
public class AuthenticatedController : ControllerBase
{
    protected long UserId => UserHelper.GetUserId(User);
}