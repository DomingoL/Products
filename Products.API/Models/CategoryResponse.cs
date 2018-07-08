
namespace Products.API.Models
{
    using System.Collections.Generic;

    public class CategoryResponse
    {
        public int CategoryID { get; set; }

        public string Description { get; set; }

        public virtual List<ProductResponse> Products { get; set; }
    }
}