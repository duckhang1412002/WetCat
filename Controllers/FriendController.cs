using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class FriendController: Controller
    {
        WetCat_DBContext DB = new WetCat_DBContext();
        public FriendController(){}

        public IActionResult Index(){
            var Follows = DB.Friends.ToList();
            return View(Follows);
        }
        [HttpPost]
        public IActionResult Delte(){
            var Follows = DB.Follows.ToList();
            return View(Follows);
        }

    }
}