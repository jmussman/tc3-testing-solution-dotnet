// SalesOrdersControllerTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TC3.Controllers;
using TC3.Dtos;
using TC3.Models;
using TC3.Services;
using TC3Test.Mocks;
using Xunit;

namespace TC3Test.Controllers {

    // These are gray-box/glass-box tests where the call to the service from the controller is verified to
    // have happened and the data the service receives is passed through correct. These could be made into
    // pure behavioral tests by taking out the Moq verifications (just the Verify) and leave the assertions.

    public class SalesOrdersControllerTest {

        Mock<ISalesOrdersService> moqSalesOrdersService;
        SalesOrdersController controller;
        private List<SalesOrder> salesOrders;
        private List<SalesOrderItem> salesOrderItems;
        private Mock<TC3Context> moqDbContext;
        private Mock<DbSet<SalesOrder>> moqSalesOrdersSet;
        private Mock<DbSet<SalesOrderItem>> moqSalesOrderItemsSet;

        private void GivenSalesOrdersController() {

            moqSalesOrdersService = new Mock<ISalesOrdersService>();
            controller = new SalesOrdersController(moqSalesOrdersService.Object);

            // Look to this class for the mockup of sales orders. We still need the mock objects, which is why the local fields are
            // passed by ref.

            new SalesOrdersMock(ref salesOrders, ref salesOrderItems, ref moqDbContext, ref moqSalesOrdersSet, ref moqSalesOrderItemsSet);
        }

        [Fact]
        public void DeleteSalesOrderSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersService.Setup(m => m.ReadSalesOrderById(1)).Returns(salesOrders[0]);

            ActionResult result = controller.DeleteSalesOrder(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteSalesOrderItemSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersService.Setup(m => m.ReadSalesOrderItemById(1)).Returns(salesOrderItems[0]);

            ActionResult result = controller.DeleteSalesOrderItem(1);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void GetSalesOrderByIdSuccess() {

            GivenSalesOrdersController();
            moqSalesOrdersService.Setup(m => m.ReadSalesOrderById(1)).Returns(salesOrders[0]);

            ActionResult<SalesOrderDto> result = controller.GetSalesOrderById(1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrderById(1));
            Assert.Equal(salesOrders[0], result.Value);
        }

        [Fact]
        public void GetSalesOrderByIdNotFound() {

            // Checks that a null result for a sales order not found returns properly.

            GivenSalesOrdersController();

            moqSalesOrdersService.Setup(m => m.ReadSalesOrderById(1)).Returns((SalesOrder)null);

            ActionResult<SalesOrderDto> result = controller.GetSalesOrderById(1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrderById(1));
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetSalesOrdersSuccess() {

            GivenSalesOrdersController();

            moqSalesOrdersService.Setup(m => m.ReadSalesOrders()).Returns(salesOrders);

            ActionResult<List<SalesOrderDto>> result = controller.GetSalesOrders(null);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrders());
            Assert.Equal(salesOrders.Count(), result.Value.Count());

            for (int i = 0; i < salesOrders.Count(); i++) {

                Assert.Equal(salesOrders[i], result.Value[i]);
            }
        }

        [Fact]
        public void GetSalesOrdersByCustomerIdSuccess() {

            GivenSalesOrdersController();

            IList<SalesOrder> expectedSalesOrders = salesOrders.Where(so => so.CustomerId == 1).ToList();
            moqSalesOrdersService.Setup(m => m.ReadSalesOrdersByCustomerId(1)).Returns(expectedSalesOrders);

            ActionResult<List<SalesOrderDto>> result = controller.GetSalesOrders(1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrdersByCustomerId(1));
            Assert.Equal(Enumerable.Count(expectedSalesOrders), Enumerable.Count(result.Value));

            for (int i = 0; i < expectedSalesOrders.Count(); i++) {

                Assert.Equal(expectedSalesOrders[i], result.Value[i]);
            }
        }

        [Fact]
        public void GetSalesOrdersByCustomerIdNotFound() {

            // This is checking that an empty list returns properly.

            GivenSalesOrdersController();

            while (salesOrders.Count > 0) {

                salesOrders.RemoveAt(0);
            }

            moqSalesOrdersService.Setup(m => m.ReadSalesOrdersByCustomerId(1)).Returns(salesOrders);

            ActionResult<List<SalesOrderDto>> result = controller.GetSalesOrders(1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrdersByCustomerId(1));
            Assert.Empty(result.Value);
        }

        [Fact]
        public void PostNewSalesOrderSuccess() {

            GivenSalesOrdersController();

            SalesOrderDto salesOrderDto = new SalesOrderDto(salesOrders[0]);

            salesOrderDto.SalesOrderId = 0;
            moqSalesOrdersService.Setup(m => m.CreateSalesOrder(salesOrderDto)).Returns((Func<SalesOrder, SalesOrder>)((s) => { s.SalesOrderId = 99; return new SalesOrder(s); }));

            ActionResult<SalesOrderDto> result = controller.PostSalesOrder(salesOrderDto);

            moqSalesOrdersService.Verify(m => m.CreateSalesOrder(salesOrderDto));
            salesOrderDto.SalesOrderId = 99;
            Assert.Equal(salesOrderDto, result.Value);
        }

        [Fact]
        public void PostNewSalesOrderItemSuccess() {

            GivenSalesOrdersController();

            SalesOrderItemDto salesOrderItemDto = new SalesOrderItemDto(salesOrderItems[0]);

            salesOrderItemDto.SalesOrderId = 0;
            moqSalesOrdersService.Setup(m => m.CreateSalesOrderItem(salesOrderItemDto)).Returns((Func<SalesOrderItem, SalesOrderItem>)((s) => { s.SalesOrderId = 99; return new SalesOrderItem(s); }));

            ActionResult<SalesOrderItemDto> result = controller.PostSalesOrderItem(salesOrderItemDto);

            moqSalesOrdersService.Verify(m => m.CreateSalesOrderItem(salesOrderItemDto));

            Assert.Equal(salesOrderItemDto, result.Value);
        }

        [Fact]
        public void PutSalesOrderSuccess() {

            GivenSalesOrdersController();

            SalesOrderDto salesOrderDto = new SalesOrderDto(salesOrders[0]);

            moqSalesOrdersService.Setup(m => m.UpdateSalesOrder(salesOrderDto));

            ActionResult result = controller.PutSalesOrder(salesOrderDto.SalesOrderId, salesOrderDto);

            moqSalesOrdersService.Verify(m => m.UpdateSalesOrder(salesOrderDto));
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void PutSalesOrderItemSuccess() {

            GivenSalesOrdersController();

            SalesOrderItemDto salesOrderItemDto = new SalesOrderItemDto(salesOrderItems[0]);

            moqSalesOrdersService.Setup(m => m.UpdateSalesOrderItem(salesOrderItemDto));

            ActionResult result = controller.PutSalesOrderItem(salesOrderItemDto.SalesOrderId, salesOrderItemDto);

            moqSalesOrdersService.Verify(m => m.UpdateSalesOrderItem(salesOrderItemDto));
            Assert.IsType<OkResult>(result);
        }
    }
}