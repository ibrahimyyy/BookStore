using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using train.Models;

namespace train.Controllers
{
    public class givemeController : Controller
    {
        private readonly BookRep _BookRead = null;
        public givemeController()
        {
            _BookRead = new BookRep();
        }
        public IActionResult GetAllBook ()
        {
            var data = _BookRead.GetallBook();
            return View(data);
        }


        public IActionResult GetBookId(int id)
        {
            return View(_BookRead.GetBookById(id));
        }
        public List<Book> SearchBook(string title , string author)
        {
            return _BookRead.SearchBook(title, author);
        }
    }
}