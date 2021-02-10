// CountryCode.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;

namespace TC3.Models
{
    public partial class CountryCode
    {

        public string CountryCodeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public CountryCode() {

            Customers = new HashSet<Customer>();
        }
    }
}
