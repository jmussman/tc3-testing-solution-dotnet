// SalesOrderManagerControllerTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using Microsoft.AspNetCore.Mvc;
using System;
using TC3.Controllers;
using TC3.Models;
using TC3.Services;
using Moq;
using Xunit;

namespace TC3Test.Controllers {

    // These are gray-box/glass-box tests where the call to the service from the controller is verified to
    // have happened. To make them behavioral tests take out the Moq verifications and leave the asserts.

    public class SalesOrderManagerControllerTest {


        Mock<ISalesOrdersService> moqSalesOrdersService;
        Mock<ISalesOrderManager> moqSalesOrderManager;
        SalesOrderManagerController controller;
        CardInfo cardInfo;

        private void GivenPurchaseController() {

            moqSalesOrdersService = new Mock<ISalesOrdersService>();
            moqSalesOrderManager = new Mock<ISalesOrderManager>();
            controller = new SalesOrderManagerController(moqSalesOrdersService.Object, moqSalesOrderManager.Object);
            cardInfo = new CardInfo { Number = "378282246310005", Expires = DateTime.Now, CCV = "123", Name = "John Doe" };
        }

        [Fact]
        public void SalesOrderNotFound() {

            GivenPurchaseController();

            moqSalesOrdersService.Setup(m => m.ReadSalesOrderById(1)).Returns((SalesOrder)null);
            moqSalesOrderManager.Setup(m => m.CompletePurchase(It.IsAny<SalesOrder>(), It.IsAny<CardInfo>())).Throws(new NullReferenceException());

            ActionResult result = controller.Put(cardInfo, 1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrderById(1), Times.Once());
            moqSalesOrderManager.Verify(m => m.CompletePurchase(It.IsAny<SalesOrder>(), It.IsAny<CardInfo>()), Times.Never());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PurchasedAuthorized() {

            GivenPurchaseController();

            SalesOrder salesOrder = new SalesOrder { SalesOrderId = 1 };

            moqSalesOrdersService.Setup(m => m.ReadSalesOrderById(1)).Returns(salesOrder);
            moqSalesOrdersService.Setup(m => m.UpdateSalesOrder(It.IsAny<SalesOrder>()));
            moqSalesOrderManager.Setup(m => m.CompletePurchase(It.IsAny<SalesOrder>(), It.IsAny<CardInfo>())).Returns("Authorized");

            ActionResult result = controller.Put(cardInfo, 1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrderById(1), Times.Once());
            moqSalesOrdersService.Verify(m => m.UpdateSalesOrder(salesOrder), Times.Once());
            moqSalesOrderManager.Verify(m => m.CompletePurchase(salesOrder, cardInfo), Times.Once());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void PurchaseDeclined() {

            GivenPurchaseController();

            SalesOrder salesOrder = new SalesOrder { SalesOrderId = 1 };

            moqSalesOrdersService.Setup(m => m.ReadSalesOrderById(1)).Returns(salesOrder);
            moqSalesOrdersService.Setup(m => m.UpdateSalesOrder(It.IsAny<SalesOrder>()));
            moqSalesOrderManager.Setup(m => m.CompletePurchase(It.IsAny<SalesOrder>(), It.IsAny<CardInfo>())).Returns((string)null);

            ActionResult result = controller.Put(cardInfo, 1);

            moqSalesOrdersService.Verify(m => m.ReadSalesOrderById(1), Times.Once());
            moqSalesOrdersService.Verify(m => m.UpdateSalesOrder(It.IsAny<SalesOrder>()), Times.Never());
            moqSalesOrderManager.Verify(m => m.CompletePurchase(salesOrder, cardInfo), Times.Once());
            Assert.IsType<OkResult>(result);
        }
    }
}
