using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Books> Books { get; set; } //this mean to create a relationship between Books Table and Language Table, it's mean each language can go with many book but each book has just one language (one to many realationship).
    }
}
