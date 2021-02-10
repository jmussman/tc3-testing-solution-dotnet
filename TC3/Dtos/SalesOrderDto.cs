// SalesOrderDto.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using TC3.Models;

namespace TC3.Dtos {

    public class SalesOrderDto : SalesOrder {

        public SalesOrderDto() {
        }

        public SalesOrderDto(SalesOrder salesOrder) : base(salesOrder) {

            // Credit card information used is restricted for external applications.

            CardNumber = null;
            CardExpires = null;
            CardAuthorized = null;
        }
    }
}
