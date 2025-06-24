using Justin.EntityFramework.Controller;
using Justin.EntityFramework.Service;
using Kalbe.TechnicalAM.DataAccess.Services;
using Kalbe.TechnicalAM.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kalbe.TechnicalAM.Api.Controllers {

    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class ProductController : BaseController<Product> {
        public ProductController(IProductService productService) : base(productService) {
        }

        [AllowAnonymous]
        public override Task<IActionResult> Get() {
            return base.Get();
        }

    }

}
