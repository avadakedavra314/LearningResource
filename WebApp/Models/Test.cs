﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Test
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CourseId { get; set; }
        public Course Course { get; set; }
    }
}
