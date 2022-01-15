using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool? Gender { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateOnDate { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public string CreateByUserId { get; set; }
        public string LastModifiedByUserId { get; set; }
    }
}
