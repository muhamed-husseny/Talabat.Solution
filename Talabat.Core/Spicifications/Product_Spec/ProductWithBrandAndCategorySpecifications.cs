using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;

namespace Talabat.Core.Spicifications.Product_Spec
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications() : base()
        {
            AddIncludes();
        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }

    

    
}
