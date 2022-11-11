using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> getProductBrandsAsync()
        {
            return await _context.ProductBrand.ToListAsync();
        }

        public async Task<Product> getProductByIdAsync(int id)
        {
            var product = await _context.Products
            .Include(p => p.ProductType).Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<IReadOnlyList<Product>> getProductsAsync()
        {
            return await _context.Products
            .Include(p => p.ProductType).Include(p => p.ProductBrand)
            .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> getProductTypesAsync()
        {
            return await _context.ProductType.ToListAsync();
        }
    }
}