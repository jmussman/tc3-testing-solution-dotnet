// Customer.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using System.Collections.Generic;

namespace TC3.Models
{
    public partial class Customer
    {
        public Customer()
        {
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string CountryCodeId { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Confirmation { get; set; }

        public virtual CountryCode CountryCode { get; set; }
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
