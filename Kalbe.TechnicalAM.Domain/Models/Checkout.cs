using Justin.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.Domain.Models {
    public class Checkout : Base {

        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public int? CartId { get; set; }
        [ForeignKey(nameof (CartId))]
        public Cart? Cart { get; set; }
        
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
}
