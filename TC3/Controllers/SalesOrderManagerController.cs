// SalesOrderManagerController.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using Microsoft.AspNetCore.Mvc;
using TC3.Models;
using TC3.Services;

namespace TC3.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderManagerController : ControllerBase {

        ISalesOrderManager salesOrderManager;
        ISalesOrdersService salesOrdersService;

        public SalesOrderManagerController(ISalesOrdersService salesOrdersService, ISalesOrderManager salesOrderManager) {

            this.salesOrdersService = salesOrdersService;
            this.salesOrderManager = salesOrderManager;
        }

        // PUT api/[controller]/5
        [HttpPut("{salesOrderId}")]
        public ActionResult Put([FromBody] CardInfo cardInfo, int salesOrderId) {

            ActionResult result = new OkResult();
            SalesOrder salesOrder = salesOrdersService.ReadSalesOrderById(salesOrderId);

            if (salesOrder != null) {

                string authorizationCode = salesOrderManager.CompletePurchase(salesOrder, cardInfo);

                if (authorizationCode != null) {

                    salesOrder.CardNumber = cardInfo.Number;
                    salesOrder.CardExpires = cardInfo.Expires;
                    salesOrder.Filled = DateTime.Now;
                    salesOrdersService.UpdateSalesOrder(salesOrder);
                }


            } else {

                result = NotFound();
            }

            return result;
        }
    }
}
