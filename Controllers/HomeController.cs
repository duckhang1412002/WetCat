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
using System.Dynamic;

namespace WetCat.Controllers
{
    public class HomeController : Controller
    {
        UserDAO userDAO = null;
        PostDAO postDAO = null;
        FollowDAO followDAO = null;
        FriendDAO friendDAO = null;
        public HomeController() {
            userDAO = new UserDAO();
            postDAO = new PostDAO();
            followDAO = new FollowDAO();
            friendDAO = new FriendDAO();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            HttpContext.Session.SetString("username", "");            
            try {
                if (ModelState.IsValid) {
                    User user = userDAO.LoginByUsernameAndPassword(username, password);
                    if (user.Username != null) {
                        HttpContext.Session.SetString("username", user.Username);  
                        if (user.Role == "Admin") 
                            return RedirectToAction("Index", "Admin"); 
                        return RedirectToAction("Index", "Post");
                    }
                        
                }
            } catch (Exception) {

            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Wall(){
            /*if (HttpContext.Session.GetString("username") == null) {
                return RedirectToAction("Index", "Home");
            }*/

            dynamic model = new ExpandoObject();
            model.following = followDAO.GetFollowings(HttpContext.Session.GetString("username"));
            model.followers = followDAO.GetFollowers(HttpContext.Session.GetString("username"));
            return View(model);   
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user) {
            System.Console.WriteLine(user.Username + " " + user.Password + " " + user.Gender);
            userDAO.RegisterUser(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
