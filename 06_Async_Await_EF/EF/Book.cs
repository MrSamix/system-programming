using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Async_Await_EF.EF
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }

        // conn

        public Author Author { get; set; }
    }
}
