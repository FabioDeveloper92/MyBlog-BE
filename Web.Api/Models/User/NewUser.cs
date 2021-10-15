using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.User
{
    public class NewUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ExternalToken { get; set; }
        public int LoginWith { get; set; }
    }
}
