using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class UserEmailOptions
    {
        public List<string> ToEmails { get; set; } // to send the email for many people.
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<KeyValuePair<string,string>> PlaceHolders { get; set; }
    }
}
