// ProductType.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;

namespace TC3.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
        }

        public int ProductTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
