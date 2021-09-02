using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public HashSet<Book> Books { get; set; }
    }
}
