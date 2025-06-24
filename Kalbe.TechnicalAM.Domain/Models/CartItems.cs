using Justin.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.Domain.Models {
    public class CartItems : Base {

        public int? CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart? Cart { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int? Quantity { get; set; }

    }
}
