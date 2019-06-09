using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? UserFromId { get; set; }
        public User UserFrom { get; set; }

        public int? UserToId { get; set; }
        public User UserTo { get; set; }
    }
}
