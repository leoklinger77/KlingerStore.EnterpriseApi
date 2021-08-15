using KSE.Catalog.Models;
using KSE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Catalog.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> FindAllProduct();
        Task<Product> FindByIdProduct(Guid id);

        Task<IEnumerable<Category>> FindAllCategory();
        Task<Category> FindByIdCategory(Guid id);

        Task Insert(Product product);
        Task Update(Product product);

        Task Insert(Category category);
        Task Update(Category category);
    }
}
