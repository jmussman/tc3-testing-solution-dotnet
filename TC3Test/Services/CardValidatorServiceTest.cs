// CardValidatorServiceTest.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Threading;
using TC3.Models;
using TC3.Services;
using TC3Test.Mocks;
using Xunit;

namespace TC3Test.Services {

    public class CardValidatorServiceTest {

        CardValidatorService cardValidatorService;
        CardInfo cardInfo;

        private void GivenCardValidatorServiceAndCardInfo() {

            cardValidatorService = new CardValidatorService();
            cardInfo = new CardInfo {
                Number = "378282246310005",
                Name = "John Doe",
                Expires = ExpirationMock.ExpiresNow(),
                CCV = "001"
            };
        }

        [Fact]
        public void AcceptsValidCardInfo() {

            GivenCardValidatorServiceAndCardInfo();

            Assert.True(cardValidatorService.Validate(cardInfo));
        }

        [Fact]
        public void RejectsNull() {

            GivenCardValidatorServiceAndCardInfo();

            cardInfo.Number = null;

            Assert.Throws<NullReferenceException>(() => cardValidatorService.Validate(cardInfo));
        }

        [Theory]
        [MemberData(nameof(InvalidCardInfoData))]
        public void RejectsInvalidCardInfo(string number, DateTime expires) {

            GivenCardValidatorServiceAndCardInfo();

            cardInfo.Number = number;
            cardInfo.Expires = expires;

            Assert.False(cardValidatorService.Validate(cardInfo));
        }

        public static TheoryData<string, DateTime> InvalidCardInfoData() {

            TheoryData<string, DateTime> data = new TheoryData<string, DateTime>();
            CardInfo cardInfo = new CardInfo {
                Number = "378282246310005",
                Name = "John Doe",
                Expires = ExpirationMock.ExpiresNow(),
                CCV = "001"
            };

            data.Add("378282246310006", cardInfo.Expires);
            data.Add("", cardInfo.Expires);
            data.Add("37828224631000A", cardInfo.Expires);
            data.Add("37828224631000a", cardInfo.Expires);
            data.Add("37828224631000?", cardInfo.Expires);
            data.Add("378282246310005", cardInfo.Expires.AddMonths(-1));
            data.Add("378282246310005", cardInfo.Expires.AddYears(5).AddMonths(1));

            return data;
        }
    }
}
