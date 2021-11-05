using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data
{
    //[Table("NewNameOfTheTable")] //this can help me to change the name of the Table.
    //[Table("NewNameOfTheTable",Schema ="Here I can put new Schema")] //This can change the name of Table and the Schema. Note: the default Schema it's 'dbo' which i can see it in database.
    //[Index(nameof(BookpdfURL))] //here i made the property BookpdfURL as index, and i can made a composite index like this ([Index(nameof(BookpdfURL),nameof(Author))]), and i can made it unique like this ([Index(nameof(BookpdfURL),IsUnique=true)]), and i can chnage the name of index like this ([Index(nameof(BookpdfURL), Name="Index_Name")]).
    public class Books
    {        // if i want to set PK it's name doesn't has "id" so i should to [Key]
        //[Key]
        public int Id { get; set; } //any property it's name is "id" or it's end with "id" like this "Bookid" it will be the PK for the table   .
        //[Column("Name of book")] //this can change the name of column in database.
        //[Column(TypeName="varchar(200)")] //this can change the datatype of this property[in defalut it's nvarchar(max) but with this i made it varchar(200) and 200 mean the MaxLength].
        //[MaxLength(50)] //this can help me to change the maxLength without change the datatype.
        public string Name { get; set; }
        //[Comment("the title of book")] //i can make a comment for this column.
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int LanguageId { get; set; }
        public int TotalPages { get; set; }
        public string ImageURL { get; set; }
        public string BookpdfURL { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Language Language { get; set; }//this is a field come from Language Table(so i can say there are a relationship between Books Table and Language Table) it's mean each book has one language only (one to mant realationship).
        public ICollection<BookGallery> bookGalley { get; set; }//this mean i create a realationship between 2 Table (Book and BookGallery) and it's mean each book has a collection(list | many) of photos.
    }
}