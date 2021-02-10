// SalesOrderManger.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using TC3.Models;

namespace TC3.Services {

    public class SalesOrderManager: ISalesOrderManager {

        private ICardValidator cardValidator;
        private IMerchantServicesAuthorizer authorizer;
        private TC3Context dbContext;

        public SalesOrderManager(ICardValidator cardValidator, IMerchantServicesAuthorizer authorizer, TC3Context dbContext) {

            this.cardValidator = cardValidator;
            this.authorizer = authorizer;
            this.dbContext = dbContext;
        }

        public string CompletePurchase(SalesOrder salesOrder, CardInfo cardInfo) {

            // This is a controller that follows four steps to complete the sale.

            string authorizationCode = null;

            if (Validate(salesOrder) && cardValidator.Validate(cardInfo)) {

                authorizationCode = authorizer.Authorize((decimal)salesOrder.Total, cardInfo);

                if (authorizationCode != null) {

                    UpdateSalesOrder(salesOrder, cardInfo, authorizationCode);
                }
            }

            return authorizationCode;
        }

        private string MaskCardNumber(string cardNumber) {

            string result = cardNumber;

            if (result.Length > 4) {

                // The String constructor can repeat a sequence!

                result = (new String('*', result.Length - 4)) + result.Substring(result.Length - 4);
            }

            return result;
        }

        private void UpdateSalesOrder(SalesOrder salesOrder, CardInfo cardInfo, string authorizationCode) {

            salesOrder.CardNumber = MaskCardNumber(cardInfo.Number);
            salesOrder.CardExpires = cardInfo.Expires;
            // salesOrder.CardAuthorized = authorizationCode;
            salesOrder.Filled = DateTime.Now;

            dbContext.SalesOrders.Update(salesOrder);
        }

        private bool Validate(SalesOrder salesOrder) {

            // Requirement: the sales order must be > 0 and < $250.

            return salesOrder.Total > 0 && salesOrder.Total <= 250;
        }
    }
}
