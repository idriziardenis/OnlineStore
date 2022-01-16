using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Store.Controllers
{
    public class OrderController : Controller
    {
        [Area("Store")]
        [Authorize("Client")]
        public IActionResult Completed()
        {
            return View();
        }
    }
}
