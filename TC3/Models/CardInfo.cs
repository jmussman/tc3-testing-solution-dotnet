// CardInfo.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;

namespace TC3.Models {

    public class CardInfo {

        public String Number { get; set; }
        public DateTime Expires { get; set; }
        public String CCV { get; set; }
        public String Name { get; set; }
    }
}
