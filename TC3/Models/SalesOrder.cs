// SalesOrder.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

#nullable enable

using System;
using System.Collections.Generic;

namespace TC3.Models
{
    public partial class SalesOrder {

        public SalesOrder() {
            SalesOrderItems = new HashSet<SalesOrderItem>();
        }

        public SalesOrder(SalesOrder salesOrder) : this() {

            SalesOrderId = salesOrder.SalesOrderId;
            OrderDate = salesOrder.OrderDate;
            CustomerId = salesOrder.CustomerId;
            Total = salesOrder.Total;
            CardNumber = salesOrder.CardNumber;
            CardExpires = salesOrder.CardExpires;
            CardAuthorized = salesOrder.CardAuthorized;
            Filled = salesOrder.Filled;
        }

        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
        public string? CardNumber { get; set; }
        public DateTime? CardExpires { get; set; }
        public string? CardAuthorized { get; set; }
        public DateTime? Filled { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; }

        public override bool Equals(object? obj) {
            return obj is SalesOrder order &&
                   SalesOrderId == order.SalesOrderId &&
                   OrderDate == order.OrderDate &&
                   CustomerId == order.CustomerId &&
                   Total == order.Total &&
                   CardNumber == order.CardNumber &&
                   CardExpires == order.CardExpires &&
                   CardAuthorized == order.CardAuthorized &&
                   Filled == order.Filled;
        }

        public override int GetHashCode() {
            return HashCode.Combine(SalesOrderId, OrderDate, CustomerId, Total, CardNumber, CardExpires, CardAuthorized, Filled);
        }
    }
}
