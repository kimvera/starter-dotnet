using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseDesc { get; set; }
        public string SeasonTerm { get; set; }
        public string Teacher { get; set; }
        public List<Student> Students { get; set; }

    }
}
