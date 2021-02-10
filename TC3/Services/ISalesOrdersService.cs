// ISalesOrdersService.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;
using TC3.Models;

namespace TC3.Services {

    public interface ISalesOrdersService {

        public SalesOrder CreateSalesOrder(SalesOrder salesOrder);
        public SalesOrderItem CreateSalesOrderItem(SalesOrderItem salesOrderItem);
        public void DeleteSalesOrder(SalesOrder salesOrder);
        public void DeleteSalesOrderItem(SalesOrderItem salesOrderItem);
        public IEnumerable<SalesOrder> ReadSalesOrders();
        public IEnumerable<SalesOrder> ReadSalesOrdersByCustomerId(int customerId);
        public SalesOrder ReadSalesOrderById(int salesOrderId);
        public SalesOrderItem ReadSalesOrderItemById(int salesOrderItemId);
        public void UpdateSalesOrder(SalesOrder salesOrder);
        public void UpdateSalesOrderItem(SalesOrderItem salesOrderItem);
    }
}
