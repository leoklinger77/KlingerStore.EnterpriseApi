using KSE.Cart.Extensions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Cart.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Models.Cart> _cart;
                
        public CartRepository(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);            
            _cart = database.GetCollection<Models.Cart>(settings.BooksCollectionName);            
        }

        public async Task<List<Models.Cart>> GetToList() =>
           await _cart.FindAsync(x => true).Result.ToListAsync();

        public async Task<Models.Cart> Get(Guid id) =>
           await _cart.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();

        public async Task<Models.Cart> GetClient(Guid id)
        {            
            return await _cart.FindAsync(x => x.ClientId == id).Result.FirstOrDefaultAsync();
        }

        public async Task<Models.Cart> Create(Models.Cart cart)
        {            
            await _cart.InsertOneAsync(cart);
            return cart;
        }

        public async Task Update(Guid id, Models.Cart cart) =>
            await _cart.ReplaceOneAsync(x => x.Id == id, cart);

        public async Task Remove(Models.Cart cart) =>
            await _cart.DeleteOneAsync(x => x.Id == cart.Id);

        public async Task Remove(Guid id) =>
            await _cart.DeleteOneAsync(x => x.Id == id);
    }
}
