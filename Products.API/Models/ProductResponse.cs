using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.API.Models
{
    public class ProductResponse
    {

        public int ProductID { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastPurchase { get; set; }

        public string Image { get; set; }

        public double stock { get; set; }

        public string Remark { get; set; }
        
    }
}