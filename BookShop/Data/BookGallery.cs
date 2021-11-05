using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data
{
    public class BookGallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        //[ForeignKey("BookId")]
        public int BookId { get; set; } //here i made BookId a FK. Or i can use [ForeignKey("THE NAME OF PROPERETY TO MAKE IT A FK")].
        public Books Book { get; set; } //here i said each photo has one book (one to many relationship).
    }
}
