using System;
using System.Collections.Generic;

#nullable disable

namespace DynamicAndGenericControllersSample.Models
{
    public partial class Album
    {
        public Album()
        {
            Books = new HashSet<Book>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
