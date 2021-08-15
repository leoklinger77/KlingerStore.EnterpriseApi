using KSE.Catalog.Data;
using KSE.Catalog.Interfaces;
using KSE.Catalog.Models;
using KSE.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Catalog.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Category>> FindAllCategory()
        {
            return await _context.Category.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindAllProduct()
        {
            return await _context.Product.AsNoTracking().ToListAsync();
        }

        public async Task<Category> FindByIdCategory(Guid id)
        {
            return await _context.Category.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> FindByIdProduct(Guid id)
        {
            return await _context.Product.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Insert(Product product)
        {
            await _context.Product.AddAsync(product);
        }

        public async Task Insert(Category category)
        {
            await _context.Category.AddAsync(category);
        }

        public async Task Update(Product product)
        {
            _context.Product.Update(product);
        }

        public async Task Update(Category category)
        {
            _context.Category.Update(category);
        }

        public void Dispose()
        {
            _context?.DisposeAsync();
        }
    }
}
