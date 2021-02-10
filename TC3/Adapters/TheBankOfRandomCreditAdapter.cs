// TheBankOfRandomCreditAdapter
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using TC3.Models;
using TC3.Services;
using TheBankOfRandomCredit.CardServices;

namespace TC3.Adapters {

    public class TheBankOfRandomCreditAdapter : IMerchantServicesAuthorizer {

        AuthorizationService authorizationService;

        public TheBankOfRandomCreditAdapter() {

            authorizationService = new AuthorizationService();
        }

        public string Authorize(decimal amount, CardInfo cardInfo) {

            return authorizationService.Submit(cardInfo.Number, cardInfo.Name, cardInfo.Expires.ToString("MM/yyyy"), cardInfo.CCV, amount);            
        }
    }
}
