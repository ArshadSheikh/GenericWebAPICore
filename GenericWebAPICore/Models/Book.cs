using System;
using System.Collections.Generic;

#nullable disable

namespace DynamicAndGenericControllersSample.Models
{
    public partial class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
