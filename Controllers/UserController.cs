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
    public class UserController : Controller
    {
        UserDAO userDAO = null;
        public UserController() {
            userDAO = new UserDAO();
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Edit(User user)
        {
            return View();
        }
        
    }
}