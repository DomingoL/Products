
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Backend.Models
{
    using Products.Domain;
    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<Products.Domain.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<Products.Domain.Ubication> Ubications { get; set; }
    }
}