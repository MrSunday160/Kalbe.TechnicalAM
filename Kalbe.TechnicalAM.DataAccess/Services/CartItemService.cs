using Justin.EntityFramework.Model;
using Justin.EntityFramework.Service;
using Kalbe.TechnicalAM.DataAccess.Models;
using Kalbe.TechnicalAM.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.DataAccess.Services {

    public interface ICartItemService : IBaseService<CartItems> {
        
    }
    public class CartItemService : BaseService<CartItems>, ICartItemService {

        private readonly TechnicalAMDbContext _dbContext;
        public CartItemService(TechnicalAMDbContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }

        public async Task<CrudResponse> AddMany(List<CartItems> items) {

            try {

                foreach(var entity in items) {

                    _dbContext.CartItems.Add(entity);

                }
                await _dbContext.SaveChangesAsync();

                return new CrudResponse() {

                    IsSuccess = true,
                    Message = "Successfully added items into cart"

                };

            }
            catch(Exception ex) {
            
                throw new ArgumentException(ex.Message);
            
            }
        
        }

    }
}
