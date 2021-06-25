using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests.Assessment
{
    public class CourseUpdateRequest : CourseAddRequest
    {
        public int Id { get; set; }
    }
}
