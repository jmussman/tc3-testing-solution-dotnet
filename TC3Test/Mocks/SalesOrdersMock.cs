// SalesOrdersMock.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TC3.Models;

namespace TC3Test.Mocks {

    public class SalesOrdersMock {

        public SalesOrdersMock(ref List<SalesOrder> salesOrders, ref List<SalesOrderItem> salesOrderItems, ref Mock<TC3Context> moqDbContext, ref Mock<DbSet<SalesOrder>> moqSalesOrdersSet, ref Mock<DbSet<SalesOrderItem>> moqSalesOrderItemsSet) {

            moqDbContext = new Mock<TC3Context>();

            // This is a full mockup of the lists for sales order, each with two sales order items. The items for each
            // order are mocked because we don't know if the service will calculate the total or just used the value
            // in the field.

            salesOrderItems = new List<SalesOrderItem> {
                new SalesOrderItem { SalesOrderItemId = 1, SalesOrderId = 1, ProductId = 1, Quantity = 1, Price = 1 },
                new SalesOrderItem { SalesOrderItemId = 2, SalesOrderId = 1, ProductId = 2, Quantity = 2, Price = 1 },
                new SalesOrderItem { SalesOrderItemId = 3, SalesOrderId = 2, ProductId = 1, Quantity = 1, Price = 1 },
                new SalesOrderItem { SalesOrderItemId = 4, SalesOrderId = 2, ProductId = 2, Quantity = 2, Price = 1 },
                new SalesOrderItem { SalesOrderItemId = 5, SalesOrderId = 3, ProductId = 1, Quantity = 1, Price = 1 },
                new SalesOrderItem { SalesOrderItemId = 6, SalesOrderId = 3, ProductId = 2, Quantity = 2, Price = 1 }
,           };

            salesOrders = new List<SalesOrder> {
                new SalesOrder {
                    SalesOrderId = 1,
                    OrderDate = new DateTime(2018, 10, 15, 20, 12, 32),
                    CustomerId = 1,
                    Total = 3,
                    CardNumber = null,
                    CardExpires = null,
                    // CardAuthorized = null,
                    Filled = null,
                    SalesOrderItems = new List<SalesOrderItem> { salesOrderItems[0], salesOrderItems[1] }
                },
                new SalesOrder {
                    SalesOrderId = 2,
                    OrderDate = new DateTime(2019, 8, 20, 22, 25, 16),
                    CustomerId = 1,
                    Total = 3,
                    CardNumber = null,
                    CardExpires = null,
                    // CardAuthorized = null,
                    Filled = null,
                    SalesOrderItems = new List<SalesOrderItem> { salesOrderItems[2], salesOrderItems[3] }
                },
                new SalesOrder {
                    SalesOrderId = 3,
                    OrderDate = new DateTime(2019, 7, 12, 21, 9, 14),
                    CustomerId = 2,
                    Total = 3,
                    CardNumber = null,
                    CardExpires = null,
                    // CardAuthorized = null,
                    Filled = null,
                    SalesOrderItems = new List<SalesOrderItem> { salesOrderItems[4], salesOrderItems[5] }
                }
            };

            // Mock up the DBContext because the SalesOrderManager will try to search and modify the database. Mocking the three
            // properties and GetEnumerator provides support for the DBSet methods and Linq queries against the data.

            IQueryable<SalesOrder> queryableSalesOrders = salesOrders.AsQueryable();

            moqSalesOrdersSet = new Mock<DbSet<SalesOrder>>();
            moqSalesOrdersSet.As<IQueryable<SalesOrder>>().Setup(m => m.Provider).Returns(queryableSalesOrders.Provider);
            moqSalesOrdersSet.As<IQueryable<SalesOrder>>().Setup(m => m.Expression).Returns(queryableSalesOrders.Expression);
            moqSalesOrdersSet.As<IQueryable<SalesOrder>>().Setup(m => m.ElementType).Returns(queryableSalesOrders.ElementType);
            moqSalesOrdersSet.As<IQueryable<SalesOrder>>().Setup(m => m.GetEnumerator()).Returns(queryableSalesOrders.GetEnumerator());

            IQueryable<SalesOrderItem> queryableSalesOrderItems = salesOrderItems.AsQueryable();

            moqSalesOrderItemsSet = new Mock<DbSet<SalesOrderItem>>();
            moqSalesOrderItemsSet.As<IQueryable<SalesOrderItem>>().Setup(m => m.Provider).Returns(queryableSalesOrderItems.Provider);
            moqSalesOrderItemsSet.As<IQueryable<SalesOrderItem>>().Setup(m => m.Expression).Returns(queryableSalesOrderItems.Expression);
            moqSalesOrderItemsSet.As<IQueryable<SalesOrderItem>>().Setup(m => m.ElementType).Returns(queryableSalesOrderItems.ElementType);
            moqSalesOrderItemsSet.As<IQueryable<SalesOrderItem>>().Setup(m => m.GetEnumerator()).Returns(queryableSalesOrderItems.GetEnumerator());

            moqDbContext.Setup(m => m.SalesOrders).Returns(moqSalesOrdersSet.Object);
            moqDbContext.Setup(m => m.SalesOrderItems).Returns(moqSalesOrderItemsSet.Object);
        }
    }
}