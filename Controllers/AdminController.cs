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
    public class AdminController: Controller
    {
        PostDAO postList = null;
        public AdminController() => postList = new PostDAO();

        public IActionResult Index(){
            var postLists = postList.GetAllPosts().ToList();
            return View(postLists);
        }
    }
}