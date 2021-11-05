using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Components
{
    public class TopBooksViewComponent : ViewComponent
    {
        private readonly IBookRepository _BookRep;
        public TopBooksViewComponent(IBookRepository BookRep)
        {
            _BookRep = BookRep;
        }
        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var data = await _BookRep.GetTopBooks(count);
            return View(data); // this calling Default view which it's in /Shared/Components/TopBooks/Default
        }
    }
}
