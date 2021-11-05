using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookShop.Models
{
    public class BookModel
    {
        //[DataType(DataType.EmailAddress)] //DataType is mean the kind of information what i'll input.
        //[EmailAddress]
        //[Display(Name="Email: ")]
        //public string Email { get; set; }        
        public int Id { get; set; }
        [Required] // [Required] it's mean if i make a form for them so i must to insert info and i can't let them null.
        [StringLength(50, MinimumLength = 5)] // i can add many attribute valdition this row mean it must component from 5 to 50 charicatir.
        public string Name { get; set; }
        [Required(ErrorMessage = "you should to put the title of your book")]
        //[DataType(DataType.Password)] : this make the input like a password ****** like this.
        public string Title { get; set; }
        [Required(ErrorMessage = "you should to put the author of your book")] // and it can take a parameter for custom error message to don't depand on <span> defalut message.
        public string Author { get; set; }
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }        
        public string Category { get; set; }
        //[Required(ErrorMessage = "Please enter your language of your book")]
        public int LanguageId { get; set; }
        public string ImageURL { get; set; }
        public string Language { get; set; }
        [Required(ErrorMessage = "you should to put the total pages of your book")] //this error message will not appear because it's int not string so i should to make it int?
        [Display(Name="Total pages of book")]//this can change the name of "TotalPages" to "Total pages of book".
        public int? TotalPages { get; set; }
        [Required]
        [Display(Name="Choose your photo book:")]
        public IFormFile PhotoCover { get; set; } // IFormFile using with file attribute like a photo.
        [Display(Name = "Choose your photos collection book:")]
        public IFormFileCollection GalleryFiles { get; set; }//IFormFileCollection it's a data type such as (List<IFormFile>) or such as (IEnumreable<IFormFile>) in simple it's a list from IFormFile data type.
        [Display(Name="Choose multiple image:")]        
        public List<GalleryModel> Gallery { get; set; }
        [Display(Name="Choose your Pdf file:")]
        public IFormFile BookPdf { get; set; }
        public string BookPdfURL { get; set; }
        public List<string> RoleName { get; set; }
    }
}
