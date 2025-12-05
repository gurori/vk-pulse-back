using Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Auth
{
    public sealed class HasPermissionAttribute(Permission permission)
        : AuthorizeAttribute(policy: permission.ToString()) { }
}
