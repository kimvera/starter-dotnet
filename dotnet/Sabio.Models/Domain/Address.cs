using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class Address : BaseAddress
    { // for data coming from the database
        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

		public bool IsActive { get;  set; }

        public double Lat { get; set; }

        public double Long { get; set; }

    }

}
