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

    public interface ICartService : IBaseService<Cart> {
        Task<CrudResponse> AddProductToCart(int cartId, int productId, int quantity);
        Task<CrudResponse> DeleteById(int cartId);
        Task<Cart> GetCartByUserId(int userId);
    }
    public class CartService : BaseService<Cart>, ICartService {

        private readonly TechnicalAMDbContext _dbContext;
        private readonly ICartItemService _cartItemService;
        public CartService(TechnicalAMDbContext dbContext, ICartItemService cartItemService) : base(dbContext) {
            _dbContext = dbContext;
            _cartItemService = cartItemService;
        }

        public async Task<Cart> GetCartByUserId(int userId) {

            try {
                var data = await _dbContext.Carts
                    .Include(x => x.CartItems.Where(y => !y.IsDeleted))
                        .ThenInclude(a => a.Product)
                    .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);
                return data;
            }
            catch(Exception ex) {
                throw new ArgumentException(ex.Message);
            }

        }

        public async Task<CrudResponse> DeleteById(int cartId) {

            try {

                var cart = await _dbContext.Carts.FindAsync(cartId);
                // detatch cart
                var local = _dbContext.ChangeTracker.Entries<Cart>().FirstOrDefault(x => x.Entity.Id == cartId);

                if(local != null)
                    local.State = EntityState.Detached;
                
                return await base.Delete(cart, true); // soft delete

            } catch(Exception ex) {
                throw new ArgumentException(ex.Message);
            }

        }

        public async Task<CrudResponse> AddProductToCart(int cartId, int productId, int quantity) {

            try {

                var existingItem = await _dbContext.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);

                if(existingItem != null) {

                    // update quantity of existing item
                    existingItem.Quantity += quantity;
                    await _cartItemService.Update(existingItem, true);

                    return new CrudResponse() {

                        IsSuccess = true,
                        Message = "Successfully add product to cart"

                    };

                }
                else {

                    var cartItem = new CartItems() {

                        CartId = cartId,
                        ProductId = productId,
                        Quantity = quantity

                    };

                    var cartItemRes = await _cartItemService.Save(cartItem, true);

                    if(!cartItemRes.IsSuccess) {
                        throw new ArgumentException("Insertion Failed on Cart Items");
                    }

                    await _dbContext.SaveChangesAsync();

                    return new CrudResponse() {

                        IsSuccess = true,
                        Message = "Successfully add product into cart"

                    };

                }

            }
            catch(Exception ex) {

                throw new ArgumentException(ex.Message);
            
            }

        }

    }
}
