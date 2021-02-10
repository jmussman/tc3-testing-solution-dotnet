// SalesOrdersControllers.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TC3.Dtos;
using TC3.Models;
using TC3.Services;

namespace TC3.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrdersController : ControllerBase {

        ISalesOrdersService salesOrdersService;

        public SalesOrdersController(ISalesOrdersService salesOrdersService) {

            this.salesOrdersService = salesOrdersService;
        }

        // DELETE api/salesorders/5
        [HttpDelete("{salesOrderId}")]
        public ActionResult DeleteSalesOrder(int salesOrderId) {

            ActionResult result = new OkResult();

            SalesOrder salesOrder = salesOrdersService.ReadSalesOrderById(salesOrderId);

            if (salesOrder != null) {

                salesOrdersService.DeleteSalesOrder(salesOrder);

            } else {

                result = new NotFoundResult();
            }

            return result;
        }

        // DELETE api/salesorders/salesorderitem/5
        [HttpDelete("salesorderitem/{salesOrderId}")]
        public ActionResult DeleteSalesOrderItem(int salesOrderItemId) {

            ActionResult result = new OkResult();

            SalesOrderItem salesOrderItem = salesOrdersService.ReadSalesOrderItemById(salesOrderItemId);

            if (salesOrderItem != null) {

                salesOrdersService.DeleteSalesOrderItem(salesOrderItem);

            } else {

                result = new NotFoundResult();
            }

            return result;
        }

        // GET api/salesorders/
        [HttpGet]
        public ActionResult<List<SalesOrderDto>> GetSalesOrders([FromQuery] int? customerId)
        {

            // Query string parameters are allowed here; the name of the parameter defines what the search will be.

            IList<SalesOrder> salesOrders = null;

            if (customerId != null) {

                salesOrders = salesOrdersService.ReadSalesOrdersByCustomerId((int)customerId).ToList();

            } else {

                salesOrders = salesOrdersService.ReadSalesOrders().ToList();
            }

            List<SalesOrderDto> salesOrderDtos = new List<SalesOrderDto>();

            foreach (SalesOrder salesOrder in salesOrders) {

                salesOrderDtos.Add(new SalesOrderDto(salesOrder));
            }

            return salesOrderDtos;
        }

        // GET api/salesorders/5
        [HttpGet("{salesOrderId}")]
        public ActionResult<SalesOrderDto> GetSalesOrderById(int salesOrderId)
        {

            SalesOrder salesOrder = salesOrdersService.ReadSalesOrderById(salesOrderId);

            // FYI this works as a conditional statment (?:) in C# 9.0+

            if (salesOrder != null) {

                return new SalesOrderDto(salesOrder);

            } else {

                return new NotFoundResult();
            }
        }

        // POST api/salesorders
        [HttpPost]
        public ActionResult<SalesOrderDto> PostSalesOrder([FromBody] SalesOrderDto salesOrderDto) {

            SalesOrder salesOrder = salesOrdersService.CreateSalesOrder(salesOrderDto);

            return new SalesOrderDto(salesOrder);
        }

        // POST api/salesorders
        [HttpPost("salesorderitems")]
        public ActionResult<SalesOrderItemDto> PostSalesOrderItem([FromBody] SalesOrderItemDto salesOrderItemDto) {

            SalesOrderItem salesOrderItem = salesOrdersService.CreateSalesOrderItem(salesOrderItemDto);

            return new SalesOrderItemDto(salesOrderItem);
        }

        // PUT api/salesorders/5
        [HttpPut("{salesOrderId}")]
        public ActionResult PutSalesOrder(int salesOrderId, SalesOrderDto salesOrderDto) {

            ActionResult result = new BadRequestResult();

            if (salesOrderId == salesOrderDto.SalesOrderId) {

                salesOrdersService.UpdateSalesOrder(salesOrderDto);
                result = new OkResult();
            }

            return result;
        }

        // PUT api/salesorders/5
        [HttpPut("/salesorderitems/{salesOrderItemId}")]
        public ActionResult PutSalesOrderItem(int salesOrderItemId, SalesOrderItemDto salesOrderItemDto) {

            ActionResult result = new BadRequestResult();

            if (salesOrderItemId == salesOrderItemDto.SalesOrderItemId) {

                salesOrdersService.UpdateSalesOrderItem(salesOrderItemDto);
                result = new OkResult();
            }

            return result;
        }
    }
}
