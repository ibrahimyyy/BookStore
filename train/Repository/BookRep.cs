using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace train.Models
{
    public class BookRep
    {
        public List<Book> GetallBook()
        {
            return DataSources();

        }
        public Book GetBookById(int id)
        {
            return DataSources().Where(x => x.Id == id).FirstOrDefault();
        }
        public List<Book> SearchBook(string title , string author)
        {
            return DataSources().Where(x => x.Title.Contains(title) && x.Author.Contains(author)).ToList();
        }
        private List<Book> DataSources()
        {
            return new List<Book>()
            {
                new Book{Id=1, Name="Java",Title="BigJ",Author="Bero", Description="this Book of Java is for biginners people",Category="Programming",Languech="English",TotalPages=200},
                new Book{Id=2, Name="Php",Title="BigP",Author="Ahmad",Description="this Book of PHP is for biginners people",Category="FrameWork",Languech="English",TotalPages=100},
                new Book{Id=3, Name="C#",Title="BigC",Author="Hinata",Description="this Book of C# is for biginners people",Category="Design",Languech="French",TotalPages=300},
                new Book{Id=4, Name="MVC",Title="BigM",Author="Naruto",Description="this Book of MVC is for biginners people",Category="3D",Languech="chines",TotalPages=785},
                new Book{Id=5, Name="Dot Net Core",Title="BigD",Author="Boruto",Description="this Book of ASP is for biginners people",Category=".NET",Languech="japanes",TotalPages=555}
            };
        }

    }
}
