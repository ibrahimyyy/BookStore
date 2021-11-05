using BookShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Repository
{
    public interface IBookRepository
    {
        Task<List<BookModel>> SearchBook(string title);
        Task<int> AddNewBook(BookModel book);
        Task<bool> DeleteBook(int Bookid);
        Task<List<BookModel>> GetAllBook();
        Task<BookModel> GetBookById(int id);
        Task<List<BookModel>> GetTopBooks(int count);
        string GetAppName();
    }
}