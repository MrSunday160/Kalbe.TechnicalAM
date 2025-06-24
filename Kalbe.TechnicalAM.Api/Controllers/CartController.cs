using Justin.EntityFramework.Controller;
using Justin.EntityFramework.Model;
using Justin.EntityFramework.Service;
using Kalbe.TechnicalAM.DataAccess.Services;
using Kalbe.TechnicalAM.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kalbe.TechnicalAM.Api.Controllers {

    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class CartController : BaseController<Cart> {

        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        public CartController(ICartService cartService, ICartItemService cartItemService) : base(cartService) {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(int userId) {

            var cart = await _cartService.GetCartByUserId(userId);

            if(cart == null)
                return NotFound();

            return Ok(cart);

        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddItem(int userId, int productId, int quantity) {

            // check if cart exist for user or not, if no -> create cart for user w/ userid
            var cart = await _cartService.GetCartByUserId(userId);
            if (cart == null) {
                // create new cart
                var temp = await _cartService.Save(new Cart() {

                    UserId = userId

                }, saveChanges: true);

                cart = (Cart)temp.Entity;

            }
            // add products into cart
            var res = await _cartService.AddProductToCart(cart.Id, productId, quantity);

            if(res.IsSuccess)
                return Ok(res);

            return BadRequest(res);
        
        }

        [HttpPut("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity) {

            var res = await _cartItemService.GetById(cartItemId);
            res.Quantity = quantity;

            var update = await _cartItemService.Update(res, true);

            if(!update.IsSuccess) {
                return BadRequest(update);
            }
             
            return Ok(update);

        }

        [HttpDelete("RemoveProduct")]
        public async Task<IActionResult> RemoveProductInCart(int cartItemId) {

            var cartItem = await _cartItemService.GetById(cartItemId);
            var res = await _cartItemService.Delete(cartItem, true);

            if(!res.IsSuccess)
                return BadRequest(res);

            return Ok(res);



        }

    }


}
