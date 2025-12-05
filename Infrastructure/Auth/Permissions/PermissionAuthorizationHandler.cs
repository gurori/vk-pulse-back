using Core.Enums;
using Core.Structs;
using Microsoft.AspNetCore.Authorization;

namespace Infastructure.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement
        )
        {
            var permissionsIds = context
                .User.Claims.Where(c => c.Type == CustomClaims.Permissions)
                .Select(c => c.Value)
                .First()
                .Split()
                .Select(p => Enum.Parse<Permission>(p));

            if (permissionsIds.Contains(requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
