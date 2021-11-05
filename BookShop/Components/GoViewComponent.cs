using BookShop.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Components
{
    public class GoViewComponent : ViewComponent
    {
        private readonly BookRepository _BookRep;
        public GoViewComponent(BookRepository BookRep)
        {
            _BookRep = BookRep;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {     
            return View();
        }
    }
}
