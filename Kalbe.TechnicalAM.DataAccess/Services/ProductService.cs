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

    public interface IProductService : IBaseService<Product> {
        
    }
    public class ProductService : BaseService<Product>, IProductService {
        public ProductService(TechnicalAMDbContext dbContext) : base(dbContext) {
        }
    }
}
