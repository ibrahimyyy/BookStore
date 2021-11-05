using BookShop.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly BookShopContext _Context = null;
        public LanguageRepository(BookShopContext context)
        {
            _Context = context;
        }
        public async Task<List<LanguageModel>> GetAllLanguages()
        {
            var languages = await _Context.Language.Select(x => new LanguageModel() //the parameter of Select it's mean [x mean a row from Language Table], [=> this mean when a Select a row then do (create new LanguageModel)]. in the end save the info in a new object and then add him to list.
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return languages;
        }
    }
}
