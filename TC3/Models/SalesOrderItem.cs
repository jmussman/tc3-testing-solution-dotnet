// SalesOrderItem.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;

namespace TC3.Models
{
    public partial class SalesOrderItem
    {
        public int SalesOrderItemId { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual SalesOrder SalesOrder { get; set; }

        public SalesOrderItem() {
        }

        public SalesOrderItem(SalesOrderItem salesOrderItem) {

            SalesOrderItemId = salesOrderItem.SalesOrderItemId;
            SalesOrderId = salesOrderItem.SalesOrderId;
            ProductId = salesOrderItem.ProductId;
            Quantity = salesOrderItem.Quantity;
            Price = salesOrderItem.Price;
        }

        public override bool Equals(object obj) {
            return obj is SalesOrderItem item &&
                   SalesOrderItemId == item.SalesOrderItemId &&
                   SalesOrderId == item.SalesOrderId &&
                   ProductId == item.ProductId &&
                   Quantity == item.Quantity &&
                   Price == item.Price;
        }

        public override int GetHashCode() {
            return HashCode.Combine(SalesOrderItemId, SalesOrderId, ProductId, Quantity, Price);
        }
    }
}
