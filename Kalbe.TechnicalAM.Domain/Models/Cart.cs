using Justin.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.Domain.Models {
    public class Cart : Base {

        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public List<CartItems>? CartItems { get; set; }

    }
}
