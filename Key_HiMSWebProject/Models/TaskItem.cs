using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Key_HiMSWebProject.Models
{
    public class TaskItem
    {
        public TodoUser user { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}