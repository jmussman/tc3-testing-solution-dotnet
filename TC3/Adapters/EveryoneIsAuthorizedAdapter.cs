// EveryoneIsAutorizedAdapater.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using TC3.Models;
using TC3.Services;
using EveryoneIsAuthorized.Client;

namespace TC3.Adapters {


    public class EveryoneIsAuthorizedAdapter : IMerchantServicesAuthorizer {

        AlwaysAuthorize awaysAuthorize;

        public EveryoneIsAuthorizedAdapter() {

            awaysAuthorize = new AlwaysAuthorize();
        }

        public string Authorize(decimal amount, CardInfo cardInfo) {

            return awaysAuthorize.Authorize((double)amount, cardInfo.Number);
        }
    }
}