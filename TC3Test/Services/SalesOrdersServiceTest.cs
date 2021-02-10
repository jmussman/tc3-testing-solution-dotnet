// SalesOrdersServiceTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TC3.Models;
using TC3.Services;
using TC3Test.Mocks;
using Xunit;

namespace TC3Test.Services {

    public class SalesOrdersServiceTest {

        private SalesOrdersService salesOrdersService;
        private List<SalesOrder> salesOrders;
        private List<SalesOrderItem> salesOrderItems;
        private Mock<TC3Context> moqDbContext;
        private Mock<DbSet<SalesOrder>> moqSalesOrdersSet;
        private Mock<DbSet<SalesOrderItem>> moqSalesOrderItemsSet;

        private void GivenSalesOrdersService() {

            // Look to this class for the mockup of sales orders. We still need the mock objects, which is why the local fields are
            // passed by ref.

            new SalesOrdersMock(ref salesOrders, ref salesOrderItems, ref moqDbContext, ref moqSalesOrdersSet, ref moqSalesOrderItemsSet);

            salesOrdersService = new SalesOrdersService(moqDbContext.Object);
        }

        // Some of these tests will verify the correct DBSet methods were called, others will actually use and modify the data using the
        // mocked properties and methods from above.

        [Fact]
        public void CreateSalesOrderSuccess() {

            GivenSalesOrdersService();

            SalesOrder newSalesOrder = new SalesOrder(salesOrders[0]);

            newSalesOrder.SalesOrderId = 0;
            moqSalesOrdersSet.Setup(m => m.Add(It.IsAny<SalesOrder>())).Callback<SalesOrder>((s) => { s.SalesOrderId = 99; salesOrders.Add(s); });

            SalesOrder resultSalesOrder = salesOrdersService.CreateSalesOrder(newSalesOrder);

            moqSalesOrdersSet.Verify(m => m.Add(newSalesOrder), Times.Once());
            newSalesOrder.SalesOrderId = 99;
            Assert.Equal(newSalesOrder, resultSalesOrder);
        }

        [Fact]
        public void CreateSalesOrderItemSuccess() {

            GivenSalesOrdersService();

            SalesOrderItem newSalesOrderItem = new SalesOrderItem(salesOrderItems[0]);

            newSalesOrderItem.SalesOrderItemId = 0;
            moqSalesOrderItemsSet.Setup(m => m.Add(It.IsAny<SalesOrderItem>())).Callback<SalesOrderItem>((s) => { s.SalesOrderItemId = 99; salesOrderItems.Add(s); });

            SalesOrderItem resultSalesOrderItem = salesOrdersService.CreateSalesOrderItem(newSalesOrderItem);

            moqSalesOrderItemsSet.Verify(m => m.Add(newSalesOrderItem), Times.Once());
            newSalesOrderItem.SalesOrderId = 99;
            Assert.Equal(newSalesOrderItem, resultSalesOrderItem);
        }

        [Fact]
        public void DeleteSalesOrderSuccess() {

            GivenSalesOrdersService();
            moqSalesOrdersSet.Setup(m => m.Remove(It.IsAny<SalesOrder>()));

            salesOrdersService.DeleteSalesOrder(salesOrders[0]);

            moqSalesOrdersSet.Verify(m => m.Remove(salesOrders[0]), Times.Once());
        }

        [Fact]
        public void DeleteSalesOrderItemSuccess() {

            GivenSalesOrdersService();
            moqSalesOrderItemsSet.Setup(m => m.Remove(It.IsAny<SalesOrderItem>()));

            SalesOrderItem itemToDelete = salesOrderItems[0];

            salesOrdersService.DeleteSalesOrderItem(itemToDelete);

            moqSalesOrderItemsSet.Verify(m => m.Remove(itemToDelete), Times.Once());
        }

        // Now here is a quandry. To do a behvioral test there are two ways the service could search: Linq or SQL.
        // So, a gray-box test: we know Linq is used. Moq cannot effecitvely mock the Linq request to see if it was
        // called, so all of the read test methods rely on checking the results from the dataset provided above to see
        // if Linq queries were crafted correctly: they produced the correct results!.

        [Fact]
        public void ReadSalesOrdersSuccess() {

            GivenSalesOrdersService();

            IList<SalesOrder> resultSalesOrders = salesOrdersService.ReadSalesOrders().ToList();

            Assert.Equal(salesOrders.Count(), resultSalesOrders.Count());

            for (int i = 0; i < salesOrders.Count(); i++) {

                Assert.Equal(salesOrders[i], resultSalesOrders[i]);
            }
        }

        [Fact]
        public void ReadSalesOrdersByCustomerIdSucccess() {

            GivenSalesOrdersService();

            IList<SalesOrder> expectedSalesOrders = salesOrders.Where(so => so.CustomerId == 1).ToList();
            IList<SalesOrder> resultSalesOrders = salesOrdersService.ReadSalesOrdersByCustomerId(1).ToList();

            Assert.Equal(expectedSalesOrders.Count(), resultSalesOrders.Count());

            for (int i = 0; i < expectedSalesOrders.Count(); i++) {

                Assert.Equal(expectedSalesOrders[i], resultSalesOrders[i]);
            }
        }

        // The following Read*ById methods could go one of three ways: SQL, Linq, or
        // the Find method. Gray-box test: we know the find method is used so it will
        // have to be mocked.

        [Fact]
        public void ReadSalesOrderByIdSuccess() {

            GivenSalesOrdersService();
            moqSalesOrdersSet.Setup(m => m.Find(1)).Returns(salesOrders[0]);

            SalesOrder resultSalesOrder = salesOrdersService.ReadSalesOrderById(1);

            moqSalesOrdersSet.Verify(m => m.Find(1), Times.Once());
            Assert.Equal(salesOrders[0], resultSalesOrder);
        }

        [Fact]
        public void ReadSalesOrderByIdNotFound() {

            GivenSalesOrdersService();
            moqSalesOrdersSet.Setup(m => m.Find(99)).Returns((SalesOrder)null);

            SalesOrder resultSalesOrder = salesOrdersService.ReadSalesOrderById(99);

            Assert.Null(resultSalesOrder);
        }

        [Fact]
        public void ReadSalesOrderItemByIdSuccess() {

            GivenSalesOrdersService();
            moqSalesOrderItemsSet.Setup(m => m.Find(1)).Returns(salesOrderItems[0]);

            SalesOrderItem resultSalesOrderItem = salesOrdersService.ReadSalesOrderItemById(1);

            moqSalesOrderItemsSet.Verify(m => m.Find(1), Times.Once());
            Assert.Equal(salesOrderItems[0], resultSalesOrderItem);
        }

        [Fact]
        public void ReadSalesOrderItemByIdNotFound() {

            GivenSalesOrdersService();
            moqSalesOrderItemsSet.Setup(m => m.Find(99)).Returns((SalesOrderItem)null);

            SalesOrderItem resultSalesOrderItem = salesOrdersService.ReadSalesOrderItemById(99);

            Assert.Null(resultSalesOrderItem);
        }

        [Fact]
        public void UpdateSalesOrderSuccess() {

            GivenSalesOrdersService();
            moqSalesOrdersSet.Setup(m => m.Update(It.IsAny<SalesOrder>()));

            salesOrdersService.UpdateSalesOrder(salesOrders[0]);

            moqSalesOrdersSet.Verify(m => m.Update(salesOrders[0]), Times.Once());
        }

        [Fact]
        public void UpdateSalesOrderItemSuccess() {

            GivenSalesOrdersService();
            moqSalesOrderItemsSet.Setup(m => m.Update(It.IsAny<SalesOrderItem>()));

            salesOrdersService.UpdateSalesOrderItem(salesOrderItems[0]);

            moqSalesOrderItemsSet.Verify(m => m.Update(salesOrderItems[0]), Times.Once());
        }
    }
}
