using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class BaseAddress
    { // for data coming from the database
        public int Id { get; set; }
        public string LineOne { get; set; }
        public int SuiteNumber { get; set; }
    }
}
