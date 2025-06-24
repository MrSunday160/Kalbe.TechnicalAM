using Kalbe.TechnicalAM.DataAccess.Models;
using Kalbe.TechnicalAM.Domain.Models;
using Justin.EntityFramework.Model;
using Justin.EntityFramework.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalbe.TechnicalAM.DataAccess.Services.Common;

namespace Kalbe.TechnicalAM.DataAccess.Services {

    public interface IUserService : IBaseService<User> {
        Task<TokenResponse> AuthenticateUser(string username, string password);
    }

    public class UserService : BaseService<User>, IUserService {

        private readonly TechnicalAMDbContext _dbContext;
        private readonly IJwtService _jwtService;
        public UserService(TechnicalAMDbContext dbContext, IJwtService jwtService) : base(dbContext) {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        public async Task<TokenResponse> AuthenticateUser(string username, string password) {
        
            var data = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(data != null) {

                // check password
                if(string.Compare(data.Password, password) != 0)
                    return new TokenResponse() { 
                    
                        IsSuccess = false,
                        AccessToken = null,
                        Message = "Password does not match"
                    
                    };

                // generate token
                try {

                    var token = _jwtService.GenerateToken(data);
                    return new TokenResponse() {
                        IsSuccess = true,
                        AccessToken = token,
                        Message = $"{username} successfully logged in"
                    };
                }
                catch(Exception ex) {

                    throw new ArgumentException(ex.Message);
                
                }

            }
            return new TokenResponse() {

                IsSuccess = false,
                AccessToken= null,
                Message = "User not found"

            };

        }

    }

    public class TokenResponse {

        public bool IsSuccess { get; set; } = false;
        public string AccessToken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }

}
