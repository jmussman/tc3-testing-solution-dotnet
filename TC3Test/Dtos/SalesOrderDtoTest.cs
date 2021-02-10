// SalesOrderDtoTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using TC3.Dtos;
using TC3.Models;
using Xunit;

namespace TC3Test.Dtos {

    public class SalesOrderDtoTest {

        [Fact]
        public void BlocksOnlySensitiveFields() {

            SalesOrder salesOrder = new SalesOrder {

                SalesOrderId = 1,
                OrderDate = DateTime.Now,
                CustomerId = 1,
                Total = 1,
                CardNumber = "378282246310005",
                CardExpires = DateTime.Now,
                Filled = DateTime.Now
            };

            SalesOrderDto salesOrderDto = new SalesOrderDto(salesOrder);

            Assert.Equal(salesOrder.SalesOrderId, salesOrderDto.SalesOrderId);
            Assert.Equal(salesOrder.OrderDate, salesOrderDto.OrderDate);
            Assert.Equal(salesOrder.CustomerId, salesOrderDto.CustomerId);
            Assert.Equal(salesOrder.Total, salesOrderDto.Total);
            Assert.Null(salesOrderDto.CardNumber);
            Assert.Null(salesOrderDto.CardExpires);
            // Assert.Null(salesOrderDto.CardAuthorized);
            Assert.Equal(salesOrder.Filled, salesOrderDto.Filled);
        }
    }
}
