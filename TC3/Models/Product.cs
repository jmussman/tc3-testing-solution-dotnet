// Product.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;

namespace TC3.Models
{
    public partial class Product
    {
        public Product()
        {
            SalesOrderItems = new HashSet<SalesOrderItem>();
        }

        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; }
    }
}
