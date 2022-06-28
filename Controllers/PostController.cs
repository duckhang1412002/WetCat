using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class PostController : Controller
    {
        WetCat_DBContext DB = new WetCat_DBContext();
        public PostController(){}

        public IActionResult Index(){
            var posts = DB.Posts.ToList();
            return View(posts);
        }    
    }
}