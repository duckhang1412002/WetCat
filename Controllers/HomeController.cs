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
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
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
            HttpContext.Session.SetString("username", "");            
            try {
                if (ModelState.IsValid) {
                    HttpContext.Session.SetString("username", userDAO.LoginByUsernameAndPassword(_user.Username, _user.Password).Username);   
                    if (HttpContext.Session.GetString("username") != "") {
                        return RedirectToAction("Index", "User");
                        //set session here
                    }
                        
                }
            } catch (Exception) {

            }
            return Redirect("/Home/Index");
        }
        
    }
}
