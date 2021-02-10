// ExpirationMock.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;

namespace TC3Test.Mocks {

    /// <summary>
    /// The methods in this class are static because TheoryData providers, which are static, depend on them.
    /// </summary>
    public class ExpirationMock {

        public static DateTime ExpiresNow() {

            DateTime expires = Normalize(DateTime.Now);

            if (expires.Add(new TimeSpan(0, 0, 60)).Day == 1) {

                // Delay test for sixty seconds and try in next month.

                expires = Normalize(expires.AddDays(1));
            }

            return expires;
        }

        public static DateTime Normalize(DateTime expiration) {

            return new DateTime(expiration.Year, expiration.Month, 1, 0, 0, 0);
        }
    }
}
