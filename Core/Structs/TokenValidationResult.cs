// Core/Structs/TokenValidationResult.cs
using System.Collections.Generic;

namespace Core.Structs
{
    public struct TokenValidationResult
    {
        public bool IsValid { get; set; }
        public Dictionary<string, object> Claims { get; set; }
        public string ErrorMessage { get; set; }

        public TokenValidationResult()
        {
            IsValid = false;
            Claims = new Dictionary<string, object>();
            ErrorMessage = string.Empty;
        }
    }
}
