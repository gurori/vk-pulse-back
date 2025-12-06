// Server/Controllers/BaseController.cs
using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Linq;

namespace Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetTokenFromHeaders()
        {
            var authorizationHeader = Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new UnauthorizedException("Authorization token not found or invalid format.");
            }
            return authorizationHeader.Replace("Bearer ", "");
        }

        protected string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   User.FindFirst(Core.Structs.CustomClaims.UserId)?.Value ??
                   throw new UnauthorizedException("User ID not found in token.");
        }
    }
}
