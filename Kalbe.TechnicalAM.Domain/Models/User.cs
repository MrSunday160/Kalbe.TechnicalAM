using Justin.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.Domain.Models {
    public class User : Base {

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public Claim[] GetClaims() {

            var claims = new List<Claim>();

            claims.Add(new Claim("UserId", base.Id.ToString() ?? ""));
            claims.Add(new Claim("Username", Username ?? ""));
            claims.Add(new Claim("Email", Email ?? ""));

            return claims.ToArray();

        }

    }
}
