using BookShop.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BookShop.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookShopContext _Context = null;
        private readonly IConfiguration _configuration = null;
        public BookRepository(BookShopContext context , IConfiguration configuration)
        {
            _Context = context;
            _configuration = configuration;
        }
        //int MaxId;
        public async Task<int> AddNewBook(BookModel book) //when i'm using async so the function should be return Task<return type>.
        {
            //var i = from Books in _Context.Books select Books;
            //List<Books> Booklist = i.ToList<Books>();
            //MaxId = Booklist[Booklist.Count-1].Id;          
            //MaxId++;
            var newBook = new Books()
            {
                Title = book.Title,
                Name = book.Name,
                Description = book.Description,
                Author = book.Author,
                LanguageId = book.LanguageId,
                TotalPages = book.TotalPages.HasValue ? book.TotalPages.Value : 0, //i do this because the properties(TotalPages) of book variable is int? so i must to check if it's has a value befor i assign it.
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ImageURL = book.ImageURL,
                BookpdfURL = book.BookPdfURL,
            };
            newBook.bookGalley = new List<BookGallery>();
            foreach (var file in book.Gallery)
            {
                newBook.bookGalley.Add(new BookGallery()
                {
                    Name = file.Name,
                    URL = file.URL,
                });
            }
            await _Context.Books.AddAsync(newBook); //here we are working with Books entity not Book Class so we add newBook to the Books Table.
            await _Context.SaveChangesAsync(); // and here we save the changes "the adding process".
            //when i'm using anything has async so the function should be Async.
            //when i'm using async i should to use "await" with mthod which contain "async".
            return newBook.Id;
        }
        public async Task<bool> DeleteBook(int Bookid)
        {
            var data = await _Context.Books.FindAsync(Bookid);
            if (data != null)
            {
                _Context.Books.Remove(data);
                await _Context.SaveChangesAsync();
                return true;
            }
            return false;
            //var i = from Books in _Context.Books select Books;
            //foreach (var v in i)
            //{
            //    if (v.Id == Bookid.Id)
            //    {
            //        _Context.Books.Remove(v);
            //        _Context.SaveChanges();
            //        break;
            //    }
            //}
        }
        public async Task<List<BookModel>> GetAllBook()
        {
            var allbooks = await _Context.Books.ToListAsync();//this mean get all book on database like a List and put them on 'allbooks' variable
            var allbooskmodel = new List<BookModel>();
            if (allbooks?.Any() == true)
            {
                foreach (var b in allbooks)
                {
                    allbooskmodel.Add(new BookModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        Category = b.Category,
                        LanguageId = b.LanguageId,
                        //Language = b.Language.Name,
                        TotalPages = b.TotalPages,
                        ImageURL = b.ImageURL,
                    });
                }
            }
            return allbooskmodel;
        }
        public async Task<BookModel> GetBookById(int id)
        {
            return await _Context.Books.Where(x => x.Id == id).Select(book => new BookModel()
            {
                Id = book.Id,
                Name = book.Name,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                TotalPages = book.TotalPages,
                ImageURL = book.ImageURL,
                Gallery = book.bookGalley.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList(),
                BookPdfURL = book.BookpdfURL,
            }).FirstOrDefaultAsync();//FirstOrDefault it's mean it's will return the first match or it will send the default value.
        }
        public async Task<List<BookModel>> GetTopBooks(int count)
        {
            var allbooks = await _Context.Books.Take(count).ToListAsync();
            var allbooskmodel = new List<BookModel>();
            if (allbooks?.Any() == true)
            {
                foreach (var b in allbooks)
                {
                    allbooskmodel.Add(new BookModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Title = b.Title,
                        Author = b.Author,
                        Description = b.Description,
                        Category = b.Category,
                        LanguageId = b.LanguageId,
                        //Language = b.Language.Name,
                        TotalPages = b.TotalPages,
                        ImageURL = b.ImageURL,
                    });
                }
            }
            return allbooskmodel;
        }
        public string GetAppName()
        {
            return _configuration["AppName"];
        }
        public async Task<List<BookModel>> SearchBook(string title)
        {
            var BookResult = new List<BookModel>();            
            var data = await _Context.Books.Where(x => x.Title.Contains(title)).ToListAsync();
            if(data?.Any() == true)
            {
                foreach(var book in data)
                {
                    BookResult.Add(new BookModel()
                    {
                        Id = book.Id,
                        Name = book.Name,
                        Title = book.Title,
                        Author = book.Author,
                        Description = book.Description,
                        Category = book.Category,
                        LanguageId = book.LanguageId,
                        //Language = b.Language.Name,
                        TotalPages = book.TotalPages,
                        ImageURL = book.ImageURL,
                    });
                }
            }
            return BookResult;
        }
    }
}