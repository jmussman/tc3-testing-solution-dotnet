// ICardValidator.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using TC3.Models;

namespace TC3.Services {

    public interface ICardValidator {

        bool Validate(CardInfo cardInfo);
    }
}
