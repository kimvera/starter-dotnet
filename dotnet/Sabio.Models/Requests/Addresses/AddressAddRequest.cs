using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests.Addresses
{
    public class AddressAddRequest
    { 
        // don't grab the Id property because we don't need it for this "add" request

        [Required]
        public string LineOne { get; set; }

        public int SuiteNumber { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public bool IsActive { get; set; }
   
        public float Lat { get; set; }

        public float Long { get; set; }
        
        // don't be tempted to use the BaseAddress/Address Models for AddRequest because it just gets messy - make it separate.
    }
}
