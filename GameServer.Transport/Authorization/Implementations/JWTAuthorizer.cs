#if JWT

using System;
using System.Security.Cryptography;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Shared.MediatorCore;

namespace GameServer.Authorization.Implementations
{
    public class JWTAuthorizer : IAuthorizer
    {
        private readonly IJwtDecoder _decoder;

        public JWTAuthorizer(Mediator mediator)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //TODO: CERTIFICATE
            IJwtAlgorithm algorithm = new RS256Algorithm(new RSACryptoServiceProvider());
            
            _decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
        }
        
        public AuthorizeCredentials Authorize(byte[] authorizeData, out AuthorizeResult result)
        {
            try
            {
                string token = Encoding.UTF8.GetString(authorizeData);

                result = AuthorizeResult.Successful;
                
                return _decoder.DecodeToObject<AuthorizeCredentials>(token);
            }
            catch (TokenNotYetValidException)
            {
                result = AuthorizeResult.AuthorizeDataExpired;
            }
            catch (TokenExpiredException)
            {
                result = AuthorizeResult.AuthorizeDataExpired;
            }
            catch (SignatureVerificationException)
            {
                result = AuthorizeResult.InvalidData;
            }
            catch (Exception)
            {
                result = AuthorizeResult.InternalError;
            }

            return null;
        }
    }
}

#endif