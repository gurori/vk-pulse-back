using Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infastructure.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission Permission { get; private set; }

        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }

        public PermissionRequirement(string permissionString)
        {
            Permission = Enum.Parse<Permission>(permissionString);
        }
    }
}
