// Core/Exceptions/ConflictException.cs
using System;

namespace Core.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
