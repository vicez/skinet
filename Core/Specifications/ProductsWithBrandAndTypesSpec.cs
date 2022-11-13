using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandAndTypesSpec : BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypesSpec()
        {
            AddIncludes(x => x.ProductBrand);
            AddIncludes(x => x.ProductType);
        }

        public ProductsWithBrandAndTypesSpec(int id) : base(x => x.Id == id)
        {
            AddIncludes(x => x.ProductBrand);
            AddIncludes(x => x.ProductType);
        }
    }
}