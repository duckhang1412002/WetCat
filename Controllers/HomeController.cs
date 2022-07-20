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
            if (HttpContext.Session.GetString("username") != null)
                return RedirectToAction("Index", "Post");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            HttpContext.Session.SetString("username", "");            
            try {
                if (ModelState.IsValid) {
                    System.Console.WriteLine("HI");
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

        [HttpGet("/Wall/{usn}/{what?}")]
        public IActionResult Wall(string usn, string what){
            System.Console.WriteLine("Xin chao? " + usn);
            if (what == "") what = "timeline";
            dynamic model = new ExpandoObject();
            User user = userDAO.GetUserByUsername(usn);
            return View("/Views/Home/_Wall.cshtml", user);
        }

        public IActionResult Timeline(string id){
            System.Console.WriteLine("timeline " + id);
            PostDAO postDAO = new PostDAO();
            List<Post> list = postDAO.GetPostByUsername(id);
            dynamic model = new ExpandoObject();
            model.postsList = list.Reverse<Post>().ToList();
            model.currentSessionUser = userDAO.GetUserByUsername(HttpContext.Session.GetString("username"));
            return View("/Views/Home/Timeline.cshtml", model);
        }

        public IActionResult Follow(string id){
            System.Console.WriteLine("Con cho " + id);
            List<Follow> Followings = followDAO.GetFollowings(id);
            return View("/Views/Follow/Followings.cshtml",Followings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, string confirmPassword, string Gender) {
            System.Console.WriteLine(user.Username + " " + user.Password + " " + confirmPassword + " " + user.Gender);
            if (user.Password != confirmPassword) {
                ViewBag.ConfirmPassword = "Confirm password is wrong";
                //System.Console.WriteLine("Confirm password is wrong");
                return RedirectToAction("Index", "Home");
            }
            User _user = userDAO.GetUserByUsername(user.Username);
            if (_user != null) {
                ViewBag.UserExisted = "Username already existed";
                //System.Console.WriteLine("User already existed");
                return RedirectToAction("Index", "Home");
            }
            user.Gender = (Gender == "Male") ? 1 : 0;
            userDAO.RegisterUser(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Search(string data){
            if (data == "") return View(data);
            
            data = data.ToLower();
            System.Console.WriteLine("DATA " + data);
            List<User> result = userDAO.GetUsers().ToList()
            .Where(p =>    p.Nickname.ToString().ToLower().Contains(data)
                        || p.UserMail.ToString().ToLower() == data).ToList();
            return View(result);
        }

        public IActionResult Logout()
        {
            System.Console.WriteLine("Im going to logout");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        
    }
}
