using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Async_Await_EF.EF
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        // conn
        public ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}
