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
        public IActionResult Index(User user)
        {
            if(TempData.ContainsKey("LoginFailed"))
                ViewBag.LoginFailed = TempData["LoginFailed"];
            if (TempData.ContainsKey("UserExisted"))
                ViewBag.UserExisted = TempData["UserExisted"];
            if (TempData.ContainsKey("ConfirmPasswordIncorrect"))
                ViewBag.ConfirmPasswordIncorrect = TempData["ConfirmPasswordIncorrect"];
            if (HttpContext.Session.GetString("username") != null) {
                if (HttpContext.Session.GetString("username") != "admin") return RedirectToAction("Index", "Post");
                return RedirectToAction("Index","Admin");
            }
                
            dynamic model = new ExpandoObject();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            try {
                if (ModelState.IsValid) {
                    System.Console.WriteLine("HI");
                    User user = userDAO.LoginByUsernameAndPassword(username, password);
                    if (user != null) {
                        HttpContext.Session.SetString("username", user.Username);  
                        if (user.Role == "Admin") 
                            return RedirectToAction("Index", "Admin"); 
                        return RedirectToAction("Index", "Post");
                    } else {
                        
                    }
                        
                }
            } catch (Exception) {

            }
            System.Console.WriteLine("Login failed");
            TempData["LoginFailed"] = "Username or Password incorrect!";
            return RedirectToAction("Index", "Home");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, string confirmPassword, string Gender) {
            //System.Console.WriteLine(user.Username + " " + user.Password + " " + confirmPassword + " " + user.Gender);
            if (user.Password != confirmPassword) {
                TempData["ConfirmPasswordIncorrect"] = "Confirm Password is incorrect!";
                System.Console.WriteLine("Confirm password is wrong");
                return RedirectToAction("Index", "Home", user);
            }
            User _user = userDAO.GetUserByUsername(user.Username);
            if (_user != null) {
                TempData["UserExisted"] = "Username already existed";
                System.Console.WriteLine("User already existed");
                return RedirectToAction("Index", "Home", user);
            }
            user.Gender = (Gender == "Male") ? 1 : 0;
            user.AvatarSrc = "images/user.jpg";
            userDAO.RegisterUser(user);
            System.Console.WriteLine("Login successfully");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/Wall/{usn}/{what}")]
        public IActionResult Wall(string usn, string what){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            System.Console.WriteLine("Xin chao? " + usn);
            //if (what == "") what = "timeline";
            dynamic model = new ExpandoObject();
            User user = userDAO.GetUserByUsername(usn);
            User currentSessionUser = userDAO.GetUserByUsername(HttpContext.Session.GetString("username"));
            ViewBag.AvatarSrc = currentSessionUser.AvatarSrc;
            ViewBag.Username = currentSessionUser.Username;
            return View("/Views/Home/_Wall.cshtml", user);
        }

        public IActionResult Timeline(string id){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            User currentSessionUser = userDAO.GetUserByUsername (HttpContext.Session.GetString ("username"));

            System.Console.WriteLine("timeline " + id);
            PostDAO postDAO = new PostDAO();
            IEnumerable<Post> tempPosts = null;

            IEnumerable<Follow> followings = followDAO.GetFollowings (currentSessionUser.Username);
            IEnumerable<Friend> friends = friendDAO.GetFriendList (currentSessionUser.Username);
            friends = friendDAO.SwapColumnFriend (currentSessionUser.Username, friends);
            IEnumerable<Post> posts = postDAO.GetAllPosts ().ToList ();
            IEnumerable<Post> posts_admin;
            IEnumerable<Post> posts_privacy;
            IEnumerable<Post> posts_following;
            IEnumerable<Post> posts_friend;

            posts = postDAO.GetAllPostsByDeleteStatus (posts);
            posts_admin = postDAO.GetAllAdminPosts (posts);
            tempPosts = posts_admin;
            if (posts_admin != null) {
                tempPosts = tempPosts.Union (posts_admin.ToHashSet ());
            }

            posts_privacy = postDAO.GetAllPostsByPrivacy (currentSessionUser.Username, posts);
            if (posts_privacy != null) {
                tempPosts = tempPosts.Union (posts_privacy.ToHashSet ());
            }

            foreach (Follow following in followings) {
                posts_following = postDAO.GetAllPostsByFollowings (currentSessionUser.Username, posts, following);
                if (posts_following != null)
                    tempPosts = tempPosts.Union (posts_following.ToHashSet ());
            }

            foreach (Friend friend in friends) {
                posts_friend = postDAO.GetAllPostsByFriends (currentSessionUser.Username, posts, friend);
                if (posts_friend != null) {
                    tempPosts = tempPosts.Union (posts_friend.ToHashSet ());
                }
            }
            dynamic model = new ExpandoObject();
            posts = tempPosts.ToList().FindAll(p => p.PostAuthor == id);
            model.postsList = posts;
            model.currentSessionUser = userDAO.GetUserByUsername(HttpContext.Session.GetString("username"));
            return View("/Views/Home/Timeline.cshtml", model);
        }

        public IActionResult Follow(string id){
            List<Follow> Followings = followDAO.GetFollowings(id);
            return View("/Views/Follow/Followings.cshtml",Followings);
        }

        [HttpPost]
        public IActionResult Search(string data){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
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
