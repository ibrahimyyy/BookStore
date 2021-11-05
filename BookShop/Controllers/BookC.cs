using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;
using BookShop.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BookShop.Service;

namespace BookShop.Controllers
{
    [Route("[controller]/[action]")]
    public class BookC : Controller
    {
        private readonly IBookRepository _BookRep = null;
        private readonly ILanguageRepository _LanguageRep = null;
        private readonly IWebHostEnvironment _WebHostEnvironment = null;
        private readonly IUserService _userService;
        private readonly IAccountRepository _accountRepository;

        public BookC(IBookRepository bookRepository , ILanguageRepository LanguageRepository , IWebHostEnvironment webHostEnvironment , IUserService userService , IAccountRepository accountRepository)
        {
            _BookRep = bookRepository;
            _LanguageRep = LanguageRepository;
            _WebHostEnvironment = webHostEnvironment;
            _userService = userService;
            _accountRepository = accountRepository;
        }
        
        public async Task<IActionResult> GetAllBooks()
        {
            var data = await _BookRep.GetAllBook();
            ViewBag.search = false;
            ViewBag.none = false;
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllBooks(string title)
        {
            ViewBag.search = true;
            title = title.Trim();//to delete all the space from the text, Like that: "         his" it's become: "his".
            title = title.ToLower();
            if (!string.IsNullOrEmpty(title))
            {
                var data = await _BookRep.SearchBook(title);
                if(data.Count()==0)
                {
                    ViewBag.none = true;
                    BookModel x = new BookModel();
                    data.Add(x);
                    return View(data);
                }
                ViewBag.none = false;
                return View(data);
            }
            return View();
        }

        [Route("~/book-details/{id:int}", Name = "bookDetailsRoute")] // this row it's mean i change the name of URL when i'm calling (GetBookId) method, i put ~/  because i using [Route("[controller]/[action]")] this on all controller if i don't put ~/ so this [Route("~/book-details/{id}", Name = "bookDetailsRoute")] will not apllying. after this (:) in {id:int} i set a constraint on id parameter  and there is another constraint it's (alpha) it's mean i should to put char only or will error happen 404 and there are so many constraint.
        public async Task<IActionResult> GetBookId(int id)
        {
            var userid = _userService.GetUserId();
            var data = await _BookRep.GetBookById(id);
            if(userid != null)
                data.RoleName = await _accountRepository.GetUserRolesNames(userid);
            return View(data);
        }

        //[HttpPost]
        //public async Task<IActionResult> SearchBook(string title)
        //{
        //    title = title.Trim();//to delete all the space from the text, Like that: "         his" it's become: "his".
        //    title = title.ToLower();
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        var data = await _BookRep.SearchBook(title);
        //        return View(data);
        //    }
        //    return View();
        //}

        [Authorize] //this mean i can do this method only if i login.
        public IActionResult Add_Book(bool isSuccess=false , int bookid=0)  
        {
            //var booklanguage = new BookModel() // this object i pass it to view to make default selected language (Note: i use the Id when i assining the language when i set 1 it's mean English)
            //{
                //Language = "1",
            //};

            //ViewBag.Language = new SelectList(await _LanguageRep.GetAllLanguages(), "Id", "Name");

            //ViewBag.Language = new SelectList(GetLanguages(), "Id", "Text");//u should to make another one on the second method which i'll POST**(POST it's mean: send info from view to controller) or error will happen, and see the defination about SelectList() it's improtant because there are 4 prototype for SelectList().
            //var group1 = new SelectListGroup() { Name = "Group 1"};
            //var group2 = new SelectListGroup() { Name = "Group 2" };
            //var group3 = new SelectListGroup() { Name = "Group 3" };
            //the concept of group is very usefull because i can gathering the same language in one group and i can disable all of them from the group that's easier than disable one by one and sure i can disable a specific language if i want.
            //ViewBag.Language = new List<SelectListItem>()//this is anthor way to create a dropdown list it's not bad way.
            //{
            //    new SelectListItem(){Text="English", Value="1",Group= group1},
            //    new SelectListItem(){Text="Hindi", Value="2",Group=group1},
            //    new SelectListItem(){Text="French", Value="3",Group=group2},
            //    new SelectListItem(){Text="Dutch", Value="4",Group=group2},
            //};
            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookid;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add_Book(BookModel newBook)
        {
            if(ModelState.IsValid) //when all [Required] properties are not null(it has info) then will back true else false.
            {
                //before i added the book into database i should to save the path of photo and then i save the path into database.
                if(newBook.PhotoCover!=null)
                {
                    string folder = "books/cover/";
                    newBook.ImageURL = await UploadFile(folder , newBook.PhotoCover);                     
                }
                if(newBook.GalleryFiles != null)
                {
                    string folder = "books/Gallery/";
                    newBook.Gallery = new List<GalleryModel>();
                    foreach (var file in newBook.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = file.FileName,
                            URL = await UploadFile(folder, file),
                        };
                        newBook.Gallery.Add(gallery);
                    }
                }
                if (newBook.BookPdf!= null)
                {
                    string folder = "books/Pdf/";
                    newBook.BookPdfURL = await UploadFile(folder, newBook.BookPdf);
                }
                int ID = await _BookRep.AddNewBook(newBook);
                if (ID > 0)
                    {
                        return RedirectToAction(nameof(Add_Book) , new {isSuccess=true , bookid=ID});
                    }
            }
            //ViewBag.Language = new SelectList(await _LanguageRep.GetAllLanguages() ,"Id","Name");

            //ViewBag.Language = new SelectList(GetLanguages(), "Id", "Text");//here i make another one**. 
            //ViewBag.IsSuccess = false;
            //ViewBag.BookId = 0;
            //var group1 = new SelectListGroup() { Name = "Group 1" };
            //var group2 = new SelectListGroup() { Name = "Group 2" };
            //var group3 = new SelectListGroup() { Name = "Group 3" };
            //ViewBag.Language = new List<SelectListItem>()
            //{
            //    new SelectListItem(){Text="English", Value="1",Group= group1},
            //    new SelectListItem(){Text="Hindi", Value="2",Group=group1},
            //    new SelectListItem(){Text="French", Value="3",Group=group2},
            //    new SelectListItem(){Text="Dutch", Value="4",Group=group2},
            //};
            return View();
        }

        private async Task<string> UploadFile(string folderPath , IFormFile file)
        {            
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;            
            string serverFolder = Path.Combine(_WebHostEnvironment.WebRootPath, folderPath);//this to create a copy from image on the file of image which i named it "book/cover".
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));//here i copy the photo from the server(which is my PC now) to the project file.
            return "/" + folderPath;
        }

        //private List<LanguageModel> GetLanguages()
        //{
        //    return new List<LanguageModel>()
        //    {
        //        new LanguageModel(){Id=1,Text="English"},
        //        new LanguageModel(){Id=2,Text="French"},
        //        new LanguageModel(){Id=3,Text="Hindi"},
        //        new LanguageModel(){Id=4,Text="Dutch"},
        //    };
        //}
        //[Authorize]
        //public IActionResult Delete_Book()
        //{
        //    return View();
        //}
        //[HttpPost , Authorize]
        [Authorize]
        public async Task<IActionResult> Delete_Book(int BookID)
        {
            if(BookID>0)
            {
                await _BookRep.DeleteBook(BookID);
                return RedirectToAction("GetAllBooks");
            }            
            return RedirectToAction("GetAllBooks");
        }        

    }
}
