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

    public interface ICheckoutService : IBaseService<Checkout> {
        Task<IEnumerable<Checkout>> GetCheckoutsByUserId(int userId);
    }
    public class CheckoutService : BaseService<Checkout>, ICheckoutService {

        private readonly TechnicalAMDbContext _dbContext;
        public CheckoutService(TechnicalAMDbContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Checkout>> GetCheckoutsByUserId(int userId) {

            try {

                var data = await _dbContext.Checkouts.Where(x => x.UserId == userId)
                    .Include(x => x.Cart)
                        .ThenInclude(y => y.CartItems)
                            .ThenInclude(z => z.Product)
                    .ToListAsync();
                return data;

            }
            catch(Exception ex) {

                throw new ArgumentException(ex.Message);
            
            }
        
        }
    }
}
