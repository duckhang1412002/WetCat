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
        public IActionResult Login(User _user)
        {
            HttpContext.Session.SetString("username", "");            
            try {
                if (ModelState.IsValid) {
                    HttpContext.Session.SetString("username", userDAO.LoginByUsernameAndPassword(_user.Username, _user.Password).Username);   
                    if (HttpContext.Session.GetString("username") != null) {
                        return RedirectToAction("Index", "Post");
                        //set session here
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

        public IActionResult Wall(){
            /*if (HttpContext.Session.GetString("username") == null) {
                return RedirectToAction("Index", "Home");
            }*/

            dynamic model = new ExpandoObject();
            model.following = followDAO.GetFollowings(HttpContext.Session.GetString("username"));
            model.followers = followDAO.GetFollowers(HttpContext.Session.GetString("username"));
            return View(model);   
        }
        
        public IActionResult Register() {
            return View();
            //return RedirectToAction("Index", "Home");
        }
    }
}
