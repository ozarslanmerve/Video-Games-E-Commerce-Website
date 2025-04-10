using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGames.Shared.ComplexTypes;

namespace VideoGames.Entity.Concrete
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}
