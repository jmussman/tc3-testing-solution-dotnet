// SalesOrderItmDto.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using TC3.Models;

namespace TC3.Dtos {

    public class SalesOrderItemDto : SalesOrderItem {

        public SalesOrderItemDto() {
        }

        public SalesOrderItemDto(SalesOrderItem salesOrderItem) : base(salesOrderItem) {
        }
    }
}
