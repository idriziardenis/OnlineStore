using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Models
{
    public class ClientViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool? Gender { get; set; }
        public SelectList Genders { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
        public string ImagePath { get; set; }
        public bool IsPasswordChanged { get; set; }
    }
}
