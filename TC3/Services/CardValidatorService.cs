// CardValidatorService.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using TC3.Models;

namespace TC3.Services {

    public class CardValidatorService {

        public bool Validate(CardInfo cardInfo) {

            return ValidateCardNumber(cardInfo.Number) && ValidateCardExpiration(cardInfo.Expires);
        }

        public bool ValidateCardNumber(string cardNumber) {

            bool result = false;
            int parity = cardNumber.Length % 2;
            int sum = 0;

            for (int i = cardNumber.Length - 1; i >= 0; --i) {

                int digit = (int)Char.GetNumericValue(cardNumber[i]) * ((i % 2 == parity) ? 2 : 1);

                sum += (digit > 9) ? digit - 9 : digit;
            }

            if (sum > 0 && sum % 10 == 0) {

                result = true;
            }

            return result;
        }

        private bool ValidateCardExpiration(DateTime expiration) {

            DateTime thisMonth = NormalizeExpiration(DateTime.Now);
            DateTime fiveYearsPlusOneMonth = NormalizeExpiration(thisMonth.AddYears(5).AddMonths(1));
            expiration = NormalizeExpiration(expiration);

            return expiration >= thisMonth && expiration < fiveYearsPlusOneMonth;
        }

        private DateTime NormalizeExpiration(DateTime expiration) {

            // Set the expiration date to 00:00:00 on the first day of the month. The check is against
            // the normalized date of the current month, so >= will always work for comparison.

            return (new DateTime(expiration.Year, expiration.Month, 1, 0, 0, 0));
        }
    }
}
