using Dapper;
using KSE.Catalog.Data;
using KSE.Catalog.Interfaces;
using KSE.Catalog.Models;
using KSE.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<PagedResult<Product>> FindAllProduct(int pageSize, int pageIndex, string query = null)
        {
            var sql = @$"SELECT * FROM TB_PRODUCT
                        WHERE (@Query is null Or Name Like '%' + @Query + '%')
                        Order by [Name]
                        OFFSET {pageSize * (pageIndex - 1)} rows
                        FETCH NEXT {pageSize} ROWS ONLY
                        SELECT COUNT(Id) FROM TB_Product
                        Where (@Query is null Or Name Like '%' + @Query + '%')";

            var multi = await _context.Database.GetDbConnection().QueryMultipleAsync(sql, new { Query = query });

            var product = multi.Read<Product>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Product>()
            {
                List = product,
                TotalResult = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
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

        public async Task<IEnumerable<Product>> FindAllProductPerIds(string productIds)
        {
            var idsGuid = productIds.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Product>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Product.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Active).ToListAsync();

        }


    }
}
