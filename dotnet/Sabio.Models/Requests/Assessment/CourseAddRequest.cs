using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests.Assessment
{
    public class CourseAddRequest
    {
        public string Name { get; set; }
        public string CourseDesc { get; set; }
        public int SeasonTermId { get; set; }
        public int TeacherId { get; set; }
    }
}
