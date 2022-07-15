using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            string usn = HttpContext.Session.GetString("username");
            User user = userDAO.GetUserByUsername(usn);
            return View(user);
        }
        
        public IActionResult Edit()
        {
            string username = HttpContext.Session.GetString("username");
            User user = userDAO.GetUserByUsername(username);
            return View(user);
        }
        
    }
}