// SalesOrdersControllerIntegrationTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TC3.Controllers;
using TC3.Dtos;
using TC3.Models;
using TC3.Services;
using TC3Test.Mocks;
using TC3IntegrationTest.Mocks;
using Xunit;

namespace TC3IntegrationTest.Controllers {

    // These are gray-box/glass-box tests where the call through the service to the data access layer 
    // (Entity Framework) is verified to have happened, and the data is passed correctly. These could be
    // made into pure behavioral tests by taking out the Moq verifications (just the Verify) and leave
    // the assertions.

    public class SalesOrdersControllerIntegrationTest {

        private SalesOrdersService salesOrdersService;
        private SalesOrdersController controller;
        private List<SalesOrder> salesOrders;
        private List<SalesOrderItem> salesOrderItems;
        private Mock<TC3Context> moqDbContext;
        private Mock<DbSet<SalesOrder>> moqSalesOrdersSet;
        private Mock<DbSet<SalesOrderItem>> moqSalesOrderItemsSet;
        protected TC3Context dbContext;

        private void GivenSalesOrdersController() {

            // Look to this class for the mockup of sales orders. We still need the mock objects, which is why the local fields are
            // passed by ref.

            new SalesOrdersMock(ref salesOrders, ref salesOrderItems, ref moqDbContext, ref moqSalesOrdersSet, ref moqSalesOrderItemsSet);

            salesOrdersService = new SalesOrdersService(moqDbContext.Object);
            controller = new SalesOrdersController(salesOrdersService);
        }

        private void GivenSalesOrderControllerWithInMemoryDatabase() {

            // Follow this method into the factory to see how it sets up the in-memory database.

            InMemoryDatabaseFactory factory = new InMemoryDatabaseFactory();
            DbContextOptions<TC3Context> options = new DbContextOptionsBuilder<TC3Context>().UseSqlite(factory.Instance.Connection).Options;

            dbContext = new TC3Context(options);
            dbContext.Database.EnsureCreated();

            salesOrdersService = new SalesOrdersService(dbContext);
            controller = new SalesOrdersController(salesOrdersService);
        }

        [Fact]
        public void DeleteSalesOrderSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersSet.Setup(m => m.Remove(It.IsAny<SalesOrder>()));
            moqSalesOrdersSet.Setup(m => m.Find(1)).Returns(salesOrders[0]);

            ActionResult result = controller.DeleteSalesOrder(1);

            moqSalesOrdersSet.Verify(m => m.Remove(It.IsAny<SalesOrder>()), Times.Once());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteSalesOrderItemSuccess() {

            GivenSalesOrdersController();
            moqSalesOrderItemsSet.Setup(m => m.Remove(It.IsAny<SalesOrderItem>()));
            moqSalesOrderItemsSet.Setup(m => m.Find(1)).Returns(salesOrderItems[0]);

            ActionResult result = controller.DeleteSalesOrderItem(1);

            moqSalesOrderItemsSet.Verify(m => m.Remove(It.IsAny<SalesOrderItem>()), Times.Once());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void GetDatabaseSalesOrderByIdSuccess() {

            // This test method gets a database record from an in-memory database, demonstrating using a
            // transaction to protect the database state during an integration test.

            GivenSalesOrderControllerWithInMemoryDatabase();

            IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            ActionResult<SalesOrderDto> result = controller.GetSalesOrderById(1);

            transaction.Rollback();

            Assert.Equal(1, result.Value.SalesOrderId);
        }

        [Fact]
        public void GetSalesOrderByIdNotFound() {

            // Checks that a null result for a sales order not found returns properly.

            GivenSalesOrdersController();
            moqSalesOrdersSet.Setup(m => m.Find(1)).Returns((SalesOrder)null);

            ActionResult<SalesOrderDto> result = controller.GetSalesOrderById(1);

            moqSalesOrdersSet.Verify(m => m.Find(1), Times.Once());
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetSalesOrderByIdSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersSet.Setup(m => m.Find(1)).Returns(salesOrders[0]);

            ActionResult<SalesOrderDto> reesult = controller.GetSalesOrderById(1);

            moqSalesOrdersSet.Verify(m => m.Find(1), Times.Once());
            Assert.Equal(salesOrders[0], reesult.Value);
        }

        [Fact]
        public void GetSalesOrdersSuccess() {

            GivenSalesOrdersController();

            ActionResult<List<SalesOrderDto>> result = controller.GetSalesOrders(null);

            Assert.Equal(salesOrders.Count(), result.Value.Count());

            for (int i = 0; i < salesOrders.Count(); i++) {

                Assert.Equal(salesOrders[i], result.Value[i]);
            }
        }

        [Fact]
        public void GetSalesOrdersByCustomerIdSuccess() {

            GivenSalesOrdersController();

            IList<SalesOrder> expectedSalesOrders = salesOrders.Where(so => so.CustomerId == 1).ToList();
            ActionResult<List<SalesOrderDto>> resultSalesOrderDtos = controller.GetSalesOrders(1);

            Assert.Equal(Enumerable.Count(expectedSalesOrders), Enumerable.Count(resultSalesOrderDtos.Value));

            for (int i = 0; i < expectedSalesOrders.Count(); i++) {

                Assert.Equal(expectedSalesOrders[i], resultSalesOrderDtos.Value[i]);
            }
        }

        [Fact]
        public void GetSalesOrdersByCustomerIdNotFound() {

            // This is checking that an empty list returns properly.

            GivenSalesOrdersController();

            while (salesOrders.Count > 0) {

                salesOrders.RemoveAt(0);
            }

            ActionResult<List<SalesOrderDto>> result = controller.GetSalesOrders(1);

            Assert.Empty(result.Value);
        }

        [Fact]
        public void PostNewSalesOrderSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersSet.Setup(m => m.Add(It.IsAny<SalesOrder>())).Callback<SalesOrder>((s) => { s.SalesOrderId = 99; salesOrders.Add(s); });

            SalesOrderDto salesOrderDto = new SalesOrderDto(salesOrders[0]);

            salesOrderDto.SalesOrderId = 0;

            ActionResult<SalesOrderDto> result = controller.PostSalesOrder(salesOrderDto);

            moqSalesOrdersSet.Verify(m => m.Add(It.IsAny<SalesOrder>()), Times.Once());
            salesOrderDto.SalesOrderId = 99;
            Assert.Equal(salesOrderDto, result.Value);
        }

        [Fact]
        public void PostNewSalesOrderItemSuccess() {

            GivenSalesOrdersController();
            moqSalesOrderItemsSet.Setup(m => m.Add(It.IsAny<SalesOrderItem>())).Callback<SalesOrderItem>((s) => { s.SalesOrderItemId = 99; salesOrderItems.Add(s); });

            SalesOrderItemDto salesOrderItemDto = new SalesOrderItemDto(salesOrderItems[0]);

            salesOrderItemDto.SalesOrderId = 0;

            ActionResult<SalesOrderItemDto> result = controller.PostSalesOrderItem(salesOrderItemDto);

            moqSalesOrderItemsSet.Verify(m => m.Add(It.IsAny<SalesOrderItem>()), Times.Once());
            Assert.Equal(salesOrderItemDto, result.Value);
        }

        [Fact]
        public void PutSalesOrderSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersSet.Setup(m => m.Update(It.IsAny<SalesOrder>()));

            SalesOrderDto salesOrderDto = new SalesOrderDto(salesOrders[0]);

            ActionResult result = controller.PutSalesOrder(salesOrderDto.SalesOrderId, salesOrderDto);

            moqSalesOrdersSet.Verify(m => m.Update(salesOrders[0]), Times.Once());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void PutSalesOrderItemSuccess() {

            GivenSalesOrdersController();
            moqSalesOrderItemsSet.Setup(m => m.Update(It.IsAny<SalesOrderItem>()));

            SalesOrderItemDto salesOrderItemDto = new SalesOrderItemDto(salesOrderItems[0]);

            ActionResult result = controller.PutSalesOrderItem(salesOrderItemDto.SalesOrderId, salesOrderItemDto);

            moqSalesOrderItemsSet.Verify(m => m.Update(salesOrderItems[0]), Times.Once());
            Assert.IsType<OkResult>(result);
        }
    }
}