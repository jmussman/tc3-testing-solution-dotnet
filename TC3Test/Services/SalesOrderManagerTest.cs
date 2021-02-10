// SalesOrderManagerTest.cs
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

    public class SalesOrderManagerTest {

        // Look to the SalesOrdersServiceTest class for details about how Moq is used with Entity Framework.
        
        private Mock<ICardValidator> moqCardValidator;
        private Mock<IMerchantServicesAuthorizer> moqMerchantServicesAuthorizer;
        private List<SalesOrder> salesOrders;
        private List<SalesOrderItem> salesOrderItems;
        private Mock<TC3Context> moqDbContext;
        private Mock<DbSet<SalesOrder>> moqSalesOrdersSet;
        private Mock<DbSet<SalesOrderItem>> moqSalesOrderItemsSet;
        private SalesOrderManager salesOrderManager;
        private SalesOrder salesOrder;
        private CardInfo cardInfo;
        
        private void GivenSalesOrderAndCardInfo() {

            moqCardValidator = new Mock<ICardValidator>();
            moqMerchantServicesAuthorizer = new Mock<IMerchantServicesAuthorizer>();

            // Look to this class for the mockup of sales orders. We still need the mock objects, which is why the local fields are
            // passed by ref.

            new SalesOrdersMock(ref salesOrders, ref salesOrderItems, ref moqDbContext, ref moqSalesOrdersSet, ref moqSalesOrderItemsSet);

            salesOrderManager = new SalesOrderManager(moqCardValidator.Object, moqMerchantServicesAuthorizer.Object, moqDbContext.Object);
            salesOrder = salesOrders[0];

            cardInfo = new CardInfo {
                Number = "378282246310005",
                Name = "John Doe",
                Expires = ExpirationMock.ExpiresNow(),
                CCV = "001"
            };
        }

        [Fact]
        public void AuthorizedPurchaseSuccess() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(true);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns(Guid.NewGuid().ToString());
            moqSalesOrdersSet.Setup(m => m.Update(It.IsAny<SalesOrder>()));

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.Once());
            moqMerchantServicesAuthorizer.Verify(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>()), Times.Once());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Once());
            Assert.NotNull(authorizationCode);
        }

        [Fact]
        public void RejectedWithZeroTotal() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(true);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns((string)null);

            salesOrder.SalesOrderItems.Clear();
            salesOrder.Total = 0;

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.AtMostOnce());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Never());
            Assert.Null(authorizationCode);
        }

        [Fact]
        public void RejectedWithNegativeOneTotal() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(true);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns((string)null);


            salesOrder.SalesOrderItems.Clear();
            salesOrder.Total = -1;

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.AtMostOnce());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Never());
            Assert.Null(authorizationCode);
        }

        [Fact]
        public void RejectedWith251Total() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(true);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns((string)null);

            salesOrder.SalesOrderItems.ElementAt(1).Price = 250;
            salesOrder.Total = 251;

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.AtMostOnce());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Never());
            Assert.Null(authorizationCode);
        }

        [Fact]
        public void RejectedWithBadCardNumber() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(false);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns((string)null);

            cardInfo.Number = "378282246310006";

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.AtMostOnce());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Never());
            Assert.Null(authorizationCode);
        }

        [Fact]
        public void RejectedWithExpiredCard() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(false);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns((string)null);

            cardInfo.Expires = ExpirationMock.Normalize(DateTime.Now.AddMonths(-1));

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.AtMostOnce());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Never());
            Assert.Null(authorizationCode);
        }

        [Fact]
        public void RejectedWithCardExpirationPast5Years() {

            GivenSalesOrderAndCardInfo();
            moqCardValidator.Setup(m => m.Validate(It.IsAny<CardInfo>())).Returns(false);
            moqMerchantServicesAuthorizer.Setup(m => m.Authorize(It.IsAny<decimal>(), It.IsAny<CardInfo>())).Returns((string)null);

            cardInfo.Expires = ExpirationMock.Normalize(DateTime.Now.AddYears(5).AddMonths(1));

            string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

            moqCardValidator.Verify(m => m.Validate(cardInfo), Times.AtMostOnce());
            moqSalesOrdersSet.Verify(m => m.Update(salesOrder), Times.Never());
            Assert.Null(authorizationCode);
        }
    }
}
