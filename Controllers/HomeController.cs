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

        [HttpGet("/Wall/{usn}/{what}")]
        public IActionResult Wall(string usn, string what){
            /*if (HttpContext.Session.GetString("username") == null) {
                return RedirectToAction("Index", "Home");
            }*/
            System.Console.WriteLine("Xin chao " + usn);
            dynamic model = new ExpandoObject();
            User user = userDAO.GetUserByUsername(usn);
            return View("/Views/Home/_Wall.cshtml", user);
        }

        public IActionResult Timeline(string id){
            System.Console.WriteLine("timeline " + id);
            PostDAO postDAO = new PostDAO();
            List<Post> list = postDAO.GetPostByUsername(id);
            foreach(Post i in list){
                System.Console.WriteLine("Troi oi" + i.PostAuthor);
            }
            return View("/Views/Home/Timeline.cshtml", list.Reverse<Post>().ToList());
        }
        
        public IActionResult Follow(string id){
            System.Console.WriteLine("Con cho " + id);
            List<Follow> Followings = followDAO.GetFollowings(id);
            return View("/Views/Follow/Followings.cshtml",Followings);
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
