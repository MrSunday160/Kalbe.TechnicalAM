using Justin.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalbe.TechnicalAM.Domain.Models {
    public class Product : Base {

        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }

    }
}
