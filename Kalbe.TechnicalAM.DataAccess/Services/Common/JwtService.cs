using Kalbe.TechnicalAM.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.DataAccess.Services.Common {

    public interface IJwtService {
        string GenerateToken(User user);
    }
    public class JwtService : IJwtService {

        private readonly JwtConfiguration _jwtConfiguration;

        public JwtService(IOptions<JwtConfiguration> jwtConfiguration) {

            _jwtConfiguration = jwtConfiguration.Value;

        }

        public string GenerateToken(User user) {

            try {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor() {

                    Subject = new System.Security.Claims.ClaimsIdentity(user.GetClaims()),
                    Expires = DateTime.Now.AddDays(_jwtConfiguration.ExpireDays),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtConfiguration.Issuer,

                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

            }
            catch(Exception ex) {

                throw new ArgumentException(ex.Message);

            }

        }

        public class JwtConfiguration {

            public string Secret { get; set; }
            public int ExpireDays { get; set; }
            public string Issuer { get; set; }

        }
    }

}
