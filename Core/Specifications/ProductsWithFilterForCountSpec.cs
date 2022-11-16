using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithFilterForCountSpec : BaseSpecification<Product>
    {
        public ProductsWithFilterForCountSpec(ProductsSpecParams productParams) 
        : base(x => 
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains
        (productParams.Search)) && 
        (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {
        }
    }
}