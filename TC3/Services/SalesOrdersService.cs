// SalesOrdersService.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System.Collections.Generic;
using System.Linq;
using TC3.Models;

namespace TC3.Services {

    public class SalesOrdersService : ISalesOrdersService {

        private TC3Context dbContext;

        public SalesOrdersService(TC3Context tc3Context) {

            this.dbContext = tc3Context;
        }

        public SalesOrder CreateSalesOrder(SalesOrder salesOrder) {

            dbContext.SalesOrders.Add(salesOrder);
            dbContext.SaveChanges();

            return salesOrder;
        }

        public SalesOrderItem CreateSalesOrderItem(SalesOrderItem salesOrderItem) {

            dbContext.SalesOrderItems.Add(salesOrderItem);
            dbContext.SaveChanges();

            return salesOrderItem;
        }

        public void DeleteSalesOrder(SalesOrder salesOrder) {

            dbContext.SalesOrders.Remove(salesOrder);
            dbContext.SaveChanges();
        }

        public void DeleteSalesOrderItem(SalesOrderItem salesOrderItem) {

            dbContext.SalesOrderItems.Remove(salesOrderItem);
            dbContext.SaveChanges();
        }

        public IEnumerable<SalesOrder> ReadSalesOrders() {

            return dbContext.Set<SalesOrder>();
        }

        public IEnumerable<SalesOrder> ReadSalesOrdersByCustomerId(int customerId) {

            return dbContext.SalesOrders.Where(so => so.CustomerId == customerId);
        }

        public SalesOrder ReadSalesOrderById(int salesOrderId) {

            return dbContext.SalesOrders.Find(salesOrderId);
        }

        public SalesOrderItem ReadSalesOrderItemById(int salesOrderItemId) {

            return dbContext.SalesOrderItems.Find(salesOrderItemId);
        }

        public void UpdateSalesOrder(SalesOrder salesOrder) {

            dbContext.SalesOrders.Update(salesOrder);
            dbContext.SaveChanges();
        }

        public void UpdateSalesOrderItem(SalesOrderItem salesOrderItem) {

            dbContext.SalesOrderItems.Update(salesOrderItem);
            dbContext.SaveChanges();
        }
    }
}