// Core/Exceptions/UnauthorizedException.cs
using System;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message) { }
    }
}
