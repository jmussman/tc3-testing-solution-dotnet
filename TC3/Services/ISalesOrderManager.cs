// SalesOrderManger.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using TC3.Models;
using TC3.Services;

namespace TC3.Services {

    public interface ISalesOrderManager {

        public string CompletePurchase(SalesOrder salesOrder, CardInfo cardInfo);
    }
}
