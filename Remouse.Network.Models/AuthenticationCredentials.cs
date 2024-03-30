using System;
using Remouse.Serialization;

namespace Remouse.Network.Models
{
    public class AuthenticationCredentials
    {
        public AuthenticationCredentials(string authorizationString) { AuthorizationString = authorizationString; }
        public string AuthorizationString { get; }
    }
}