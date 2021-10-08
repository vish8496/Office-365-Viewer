using com.microsoft.dx.officewopi.Utils;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;

namespace com.microsoft.dx.officewopi.Security
{
    /// <summary>
    /// Class handles token generation and validation for the WOPI host
    /// </summary>
    public class WopiSecurity
    {
        /// <summary>
        /// Generates an access token specific to a user and file
        /// </summary>
        public static bool ValidateToken(string tokenString, string container, string docId)
        {

            try
            {
                // Try to validate the token

                
                //return valid or not (true or false)
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts the user information from a provided access token
        /// </summary>
        public static string GetUserFromToken(string tokenString)
        {
            // Initialize the token handler and validation parameters
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidation = new TokenValidationParameters()
            {
                ValidAudience = SettingsHelper.Audience,
                ValidIssuer = SettingsHelper.Audience,
               // IssuerSigningToken = new X509SecurityToken(getCert())
            };

            try
            {
                // Try to extract the user principal from the token
                Microsoft.IdentityModel.Tokens.SecurityToken token = null;
                var principal = tokenHandler.ValidateToken(tokenString, tokenValidation, out token);
                return principal.Identity.Name;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Private token handler used in instance classes
        /// </summary>
        private JwtSecurityTokenHandler tokenHandler;

        /// <summary>
        /// Generates an access token for the user and file
        /// </summary>
        //public JwtSecurityToken GenerateToken(string user, string container, string docId)
        //{
        //    var now = DateTime.UtcNow;
        //    tokenHandler = new JwtSecurityTokenHandler();
        //    var signingCert = getCert();
        //    var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //            {
        //                new Claim(ClaimTypes.Name, user),
        //                new Claim("container", container),
        //                new Claim("docid", docId)
        //        }),
        //        Issuer = SettingsHelper.Audience,
        //        Audience = SettingsHelper.Audience,
        //        Expires = DateTime.Now.AddHours(1),
        //        EncryptingCredentials = new Microsoft.IdentityModel.Tokens.X509EncryptingCredentials(signingCert),
        //        SigningCredentials = new Microsoft.IdentityModel.Tokens.X509SigningCredentials(signingCert)
        //    };
        //    //var absoluteField = typeof(SignatureProviderFactory).GetField(nameof(SignatureProviderFactory.AbsoluteMinimumAsymmetricKeySizeInBitsForSigning), BindingFlags.Public | BindingFlags.Static);
        //    //absoluteField.SetValue(null, 512);
        //    //SignatureProviderFactory.MinimumAsymmetricKeySizeInBitsForSigning = 512;
        //    try
        //    {
        //        var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
        //        return token;
        //    }
        //    catch(Exception e)
        //    {
        //        throw e;
        //    }

        //}

        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static Microsoft.IdentityModel.Tokens.SecurityToken GenerateToken(string username, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Audience = SettingsHelper.Audience,
                Issuer = SettingsHelper.Audience,

                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username)
        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(symmetricKey),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            //  var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        /// <summary>
        /// Converts the JwtSecurityToken to a Base64 string that can be used by the Host
        /// </summary>
        public string WriteToken(JwtSecurityToken token)
        {
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gets the self-signed certificate used to sign the access tokens
        /// </summary>
        private static X509Certificate2 getCert()
        {
            var certPath = @"D:\OffiseWopitest.pfx";
            var certfile = System.IO.File.OpenRead(certPath);
            var certificateBytes = new byte[certfile.Length];
            certfile.Read(certificateBytes, 0, (int)certfile.Length);
            var cert = new X509Certificate2(
                certificateBytes,
                ConfigurationManager.AppSettings["CertPassword"],
                X509KeyStorageFlags.Exportable |
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.PersistKeySet);

            return cert;

        }
    }
}
