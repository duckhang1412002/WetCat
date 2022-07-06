using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class HomeController : Controller
    {
        UserDAO userDAO = null;
        public HomeController() {
            userDAO = new UserDAO();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User _user)
        {
            User user = null;
            try {
                if (ModelState.IsValid) {
                    user = userDAO.LoginByUsernameAndPassword(_user.Username, _user.Password);   
                    if (user != null) {
                        return RedirectToAction("Index", "User");
                        //set session here
                    }
                        
                }
            } catch (Exception) {

            }
            return RedirectToAction("Index", user);
        }
        
    }
}
