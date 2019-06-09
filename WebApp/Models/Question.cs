using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int? TestId { get; set; }
        public Test Test { get; set; }
    }
}
