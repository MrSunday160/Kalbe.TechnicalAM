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
    public class CheckoutController : BaseController<Checkout> {

        private readonly ICheckoutService _checkoutService;
        private readonly ICartService _cartService;
        public CheckoutController(ICheckoutService checkoutService, ICartService cartService) : base(checkoutService) {
            _checkoutService = checkoutService;
            _cartService = cartService;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout([FromBody]CheckoutBody checkoutBody) {

            var cart = await _cartService.GetCartByUserId(checkoutBody.UserId);
            if(cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return BadRequest(new { message = "Cart is empty" });

            // create new Checkout record
            var checkout = new Checkout() {

                UserId = checkoutBody.UserId,
                Name = checkoutBody.Name,
                Address = checkoutBody.Address,
                Email = checkoutBody.Email,
                CartId = cart.Id

            };

            // call service save
            var res = await _checkoutService.Save(checkout, true);
            if(res.IsSuccess){

                // empty cart
                var delete = await _cartService.DeleteById(checkout.CartId.Value);

                if(!delete.IsSuccess) {
                    return BadRequest("Failed Deleting Cart");
                }

                var temp = new CrudResponse() {
                    IsSuccess = true,
                    Message = "Successfully checked out and emptied cart",
                    Entity = res.Entity
                };

                return Ok(temp);
            }

            return BadRequest(res);

        }

        [HttpGet("GetCheckoutsByUserId")]
        public async Task<IActionResult> GetCheckoutsByUserId(int userId) {

            var res = await _checkoutService.GetCheckoutsByUserId(userId);

            if(res == null) {
                return NotFound();
            }

            return Ok(res);

        }

    }

    public class CheckoutBody {

        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";

    }

}
