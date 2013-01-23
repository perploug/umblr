using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umblr.Umblr
{
    public class Post
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public bool Draft { get; set; }
        public DateTime Date { get; set; }
    }
}