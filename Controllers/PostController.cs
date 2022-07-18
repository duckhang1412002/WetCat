using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class PostController : Controller
    {
        WetCat_DBContext DB = new WetCat_DBContext();

        PostDAO PostDAO = new PostDAO();
        UserDAO UserDAO = new UserDAO();
        FollowDAO FollowDAO = new FollowDAO();

        FriendDAO FriendDAO = new FriendDAO();
        public PostController(){}
        private string currentSessionUser = null;

        public IActionResult Index(){
            if (HttpContext.Session.GetString("username") == null) {
                return RedirectToAction("Index", "Home");
            }   
            User currentSessionUser = UserDAO.GetUserByUsername(HttpContext.Session.GetString("username"));

            dynamic model = new ExpandoObject();

            List<Post> tempPosts = new List<Post>();

            IEnumerable<Post> posts = PostDAO.GetAllPosts().ToList();   
            //IEnumerable<Friend> friends = FriendDAO.get              
            IEnumerable<Follow> followings = FollowDAO.GetFollowings(currentSessionUser.Username);
            IEnumerable<Friend> friends = FriendDAO.GetFriendList(currentSessionUser.Username);
            friends = FriendDAO.SwapColumnFriend(currentSessionUser.Username, friends);
            IEnumerable<Post> posts_privacy;
            IEnumerable<Post> posts_following;
            IEnumerable<Post> posts_friend;

            System.Console.WriteLine("=============f===========");
            foreach(Friend friend in friends){
                System.Console.WriteLine(friend.FirstUsername + "====" + friend.SecondUsername);            
            }


            posts = PostDAO.GetAllPostsByDeleteStatus(posts);            
            posts_privacy = PostDAO.GetAllPostsByPrivacy(currentSessionUser.Username, posts); 
            if(posts_privacy != null){
                    tempPosts.AddRange(posts_privacy.ToHashSet());    
            }
            
            foreach(Follow following in followings){
                posts_following = PostDAO.GetAllPostsByFollowings(currentSessionUser.Username, posts, following);
                if(posts_following != null)
                    tempPosts.AddRange(posts_following.ToHashSet());            
            }  

            foreach(Friend friend in friends){
                posts_friend = PostDAO.GetAllPostsByFriends(currentSessionUser.Username, posts, friend);
                if(posts_friend != null){
                    tempPosts.AddRange(posts_friend.ToHashSet()); 
                }           
            }



            System.Console.WriteLine("----------Result-----------");
            foreach(Post p in tempPosts){
                System.Console.WriteLine(p.PostId);         
            }

            IEnumerable<Post> result = tempPosts.ToList();
             
            
            model.postsList = result.Reverse(); 
            model.currentSessionUser = currentSessionUser;
            return View(model);        
        }  

        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public IActionResult CreatePost(string content, IFormFile file, string privacy)  
        {  
            string author = HttpContext.Session.GetString("username");
            DateTime time = DateTime.Now;

            bool check = (author != null) && (content != null) && (privacy != null);

            if (check)  
            {                           
                string imgSrc = UploadedFile(author, file);
                System.Console.WriteLine(author + " --- " + content + " --- " + privacy + " --- " + time + " --- " + file + " --- " + imgSrc);
  
                Post post = new Post(privacy, author, time, content, imgSrc);
                
                PostDAO.CreatePost(post);
                System.Console.WriteLine("-----> Add successfully!"); 
                return RedirectToAction(nameof(Index));  
            }
            return RedirectToAction(nameof(Index));  
        }

        private string UploadedFile(string author, IFormFile file)  
        {  
            string imgSrc = null;             
  
            if (file != null)  
            {  
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/" + author);

                //create folder if not exist
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

                var filePath = Path.Combine(dirPath, file.FileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                imgSrc = String.Format("images/profiles/{0}/{1}", author, file.FileName);
            }  
            return imgSrc; 
        }

        public IActionResult EditPost(int? postId){
            if(postId == null){
                return NotFound();
            }

            System.Console.WriteLine("Post: " + postId);
            System.Console.WriteLine("Current session: " + HttpContext.Session.GetString("username"));


            Post post = PostDAO.GetPost(postId.Value);
            User currentSessionUser = UserDAO.GetUserByUsername(HttpContext.Session.GetString("username"));

            dynamic model = new ExpandoObject();                             
            
            model.post = post;
            model.currentSessionUser = currentSessionUser;
            return View(model);        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(Post post){
            PostDAO.EditPost(post);
            return RedirectToAction(nameof(Index));   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPostReal(int postId, string content, IFormFile file, string privacy){
            try{
            string author = HttpContext.Session.GetString("username");
            DateTime time = DateTime.Now;

            bool check = (postId != 0) && (author != null) && (content != null) && (time != null) && (privacy != null);

            if (check)  
            {                           
                string imgSrc = UploadedFile(author, file);
                System.Console.WriteLine(postId + "---" + author + " --- " + content + " --- " + privacy + " --- " + time + " --- " + file + " --- " + imgSrc);
  
                Post post = new Post(postId, privacy, author, time, content, imgSrc);
                
                PostDAO.EditPost(post);
                System.Console.WriteLine("-----> Edit successfully!"); 
                return RedirectToAction("Index", "Post");  
            }
            } catch(Exception e){
                System.Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));  
        }

        public ActionResult ShowComment(string id)
        {
            System.Console.WriteLine("HELOOOOO" + id);
            IEnumerable<Comment> model = null; //Temp to test
            return PartialView("_PostComment", model);
        }
    }
}