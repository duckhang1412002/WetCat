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

namespace WetCat.Controllers {
    public class PostController : Controller {
        WetCat_DBContext DB = new WetCat_DBContext ();

        PostDAO PostDAO = new PostDAO ();
        UserDAO UserDAO = new UserDAO ();
        FollowDAO FollowDAO = new FollowDAO ();
        CommentDAO CommentDAO = new CommentDAO ();
        FriendDAO FriendDAO = new FriendDAO ();
        NotificationListDAO nld = new NotificationListDAO ();
        public PostController () { }

        public IActionResult Index () {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            User currentSessionUser = UserDAO.GetUserByUsername (HttpContext.Session.GetString ("username"));

            dynamic model = new ExpandoObject ();

            IEnumerable<Post> tempPosts = null;

            IEnumerable<Follow> followings = FollowDAO.GetFollowings (currentSessionUser.Username);
            IEnumerable<Friend> friends = FriendDAO.GetFriendList (currentSessionUser.Username);
            friends = FriendDAO.SwapColumnFriend (currentSessionUser.Username, friends);
            IEnumerable<Post> posts = PostDAO.GetAllPosts ().ToList ();
            IEnumerable<Post> posts_admin;
            IEnumerable<Post> posts_privacy;
            IEnumerable<Post> posts_following;
            IEnumerable<Post> posts_friend;

            posts = PostDAO.GetAllPostsByDeleteStatus (posts);
            posts_admin = PostDAO.GetAllAdminPosts (posts);
            tempPosts = posts_admin;
            if (posts_admin != null) {
                tempPosts = tempPosts.Union (posts_admin.ToHashSet ());
            }

            posts_privacy = PostDAO.GetAllPostsByPrivacy (currentSessionUser.Username, posts);
            if (posts_privacy != null) {
                tempPosts = tempPosts.Union (posts_privacy.ToHashSet ());
            }

            foreach (Follow following in followings) {
                posts_following = PostDAO.GetAllPostsByFollowings (currentSessionUser.Username, posts, following);
                if (posts_following != null)
                    tempPosts = tempPosts.Union (posts_following.ToHashSet ());
            }

            foreach (Friend friend in friends) {
                posts_friend = PostDAO.GetAllPostsByFriends (currentSessionUser.Username, posts, friend);
                if (posts_friend != null) {
                    tempPosts = tempPosts.Union (posts_friend.ToHashSet ());
                }
            }
            posts = tempPosts.ToList().OrderByDescending(p => p.PostId);
            
            System.Console.WriteLine("----------Result Posts-----------");
            foreach(Post post in tempPosts){
                System.Console.WriteLine("Post: " + post.PostId + "---" + post.PostAuthor + "---" + post.PrivacyMode);         
            }             
            model.countNoti = nld.getAllNoti (HttpContext.Session.GetString ("username")).Where (p => p.NotifyTime.AddHours (1) > DateTime.Now).Count ();
            posts = tempPosts.ToList ();
            model.postsList = posts.Reverse ();
            model.currentSessionUser = currentSessionUser;
            return View (model);
        }

        public IActionResult DeletePost(int? postId){
            System.Console.WriteLine(postId);
            Post post = PostDAO.FindPost(postId.Value);
            //System.Console.WriteLine("OK");

            if(post != null){
                post.IsDeleted = 1;
                PostDAO.EditPost(post);
            }
            return RedirectToAction(nameof(Index));   
        }


        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public IActionResult CreatePost(string content, IFormFile file, string privacy)  
        {  
            string author = HttpContext.Session.GetString("username");
            DateTime time = DateTime.Now;

            bool check = (author != null) && (content != null) && (privacy != null);

            if (check) {
                string imgSrc = UploadedFile (author, file);
                System.Console.WriteLine (author + " --- " + content + " --- " + privacy + " --- " + time + " --- " + file + " --- " + imgSrc);

                Post post = new Post (privacy, author, time, content, imgSrc);

                PostDAO.CreatePost (post);
                System.Console.WriteLine ("-----> Add successfully!");
                return RedirectToAction (nameof (Index));
            }
            return RedirectToAction (nameof (Index));
        }

        private string UploadedFile (string author, IFormFile file) {
            string imgSrc = null;

            if (file != null) {
                string dirPath = Path.Combine (Directory.GetCurrentDirectory (), "wwwroot/images/profiles/" + author);

                //create folder if not exist
                if (!Directory.Exists (dirPath)) Directory.CreateDirectory (dirPath);

                var filePath = Path.Combine (dirPath, file.FileName);

                imgSrc = String.Format ("images/profiles/{0}/{1}", author, file.FileName);
                System.Console.WriteLine (imgSrc);
                int cnt = 1;
                while (System.IO.File.Exists ((Directory.GetCurrentDirectory () + "/wwwroot/" + imgSrc))) {
                    System.Console.WriteLine ("EXIST");
                    imgSrc = String.Format ("images/profiles/{0}/{1}", author, Path.GetFileNameWithoutExtension (file.FileName) + (++cnt).ToString () + Path.GetExtension (file.FileName));
                }
                var a = Directory.GetCurrentDirectory () + "/wwwroot/" + imgSrc;
                using var fileStream = new FileStream (a, FileMode.Create);
                file.CopyTo (fileStream);
            }
            return imgSrc;
        }

        public IActionResult EditPost (int? postId) {
            if (postId == null) {
                return NotFound ();
            }
            Post post = PostDAO.GetPost (postId.Value);
            User currentSessionUser = UserDAO.GetUserByUsername (HttpContext.Session.GetString ("username"));
            
            
            if (post.IsDeleted == 1) return NotFound();
            dynamic model = new ExpandoObject ();
            model.countNoti = nld.getAllNoti (HttpContext.Session.GetString ("username")).Where (p => p.NotifyTime.AddHours (1) > DateTime.Now).Count ();
            model.post = post;
            model.currentSessionUser = currentSessionUser;
            return View (model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost (Post post) {
            PostDAO.EditPost (post);
            return RedirectToAction (nameof (Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPostReal (int postId, string content, IFormFile file, string privacy) {
            try {
                string author = HttpContext.Session.GetString ("username");
                DateTime time = DateTime.Now;

                bool check = (postId != 0) && (author != null) && (content != null) && (privacy != null);

                if (check) {
                    string imgSrc = UploadedFile (author, file);
                    System.Console.WriteLine (postId + "---" + author + " --- " + content + " --- " + privacy + " --- " + time + " --- " + file + " --- " + imgSrc);

                    Post post = new Post (postId, privacy, author, time, content, imgSrc);

                    PostDAO.EditPost (post);
                    System.Console.WriteLine ("-----> Edit successfully!");
                    return RedirectToAction ("Index", "Post");
                }
            } catch (Exception e) {
                System.Console.WriteLine (e.Message);
                return RedirectToAction (nameof (Index));
            }
            return RedirectToAction (nameof (Index));
        }

        [HttpGet("/Post/ViewComment/{postId}")]
        public IActionResult ViewComment(int? postId){
            if(postId == null){
                return NotFound();
            }
            Post post = PostDAO.GetPost (postId.Value);
            List<Comment> cmtList = CommentDAO.GetCommentByPostID (postId.Value);
            if (post == null) System.Console.WriteLine ("Post is null!");
            User currentSessionUser = UserDAO.GetUserByUsername (HttpContext.Session.GetString ("username"));

            dynamic model = new ExpandoObject ();
            model.post = post;
            model.currentSessionUser = currentSessionUser;
            model.CommentList = cmtList;
            model.countNoti = nld.getAllNoti (HttpContext.Session.GetString ("username")).Where (p => p.NotifyTime.AddHours (1) > DateTime.Now).Count ();
            return View (model);
        }

        [HttpPost]
        public IActionResult AddComment(int postID, int parentID, string content)
        {
            Comment comment = new Comment();
            comment.CommentAuthor = HttpContext.Session.GetString("username");
            comment.CommentTime = DateTime.Now;
            comment.PostId = postID;
            comment.CommentContent = content;
            comment.ParentId = parentID;
            if(parentID == 0){
                comment.ParentId = null;
                PostDAO pD = new PostDAO();
                NotificationListController.sendNoti("comment",postID,null,comment.CommentAuthor, pD.GetPost(postID).PostAuthor);
            } else{
                CommentDAO cD = new CommentDAO();
                NotificationListController.sendNoti("reply",postID,parentID,comment.CommentAuthor, cD.GetCommentByCommentID(parentID).CommentAuthor);
            }
            CommentDAO.AddNewComment(comment);
            return RedirectToAction("ViewComment", new {postId = postID});
        }

        public IActionResult EditComment(int? commentId)
        {
            if (commentId == null) return NotFound();
            Comment comment = CommentDAO.GetCommentByCommentID(commentId.Value);
            Post post = PostDAO.GetPost(comment.PostId);
            if (post.IsDeleted == 1 || comment.IsDeleted == 1) return NotFound();
            List<Comment> cmtList = CommentDAO.GetCommentByPostID(comment.PostId);
            //if (post == null) System.Console.WriteLine("Post is null!");
            User currentSessionUser = UserDAO.GetUserByUsername(HttpContext.Session.GetString("username"));
            
            dynamic model = new ExpandoObject();                             
            model.post = post;
            model.currentSessionUser = currentSessionUser;
            model.CommentList = cmtList;
            model.countNoti = nld.getAllNoti (HttpContext.Session.GetString ("username")).Where (p => p.NotifyTime.AddHours (1) > DateTime.Now).Count ();
            ViewBag.CommentID = commentId.Value;
            return View(model);
        }

        [HttpPost]
        public IActionResult EditComment(int? commentID, string content)
        {
            if (commentID == null || content == null) return NotFound();
            Comment comment = CommentDAO.GetCommentByCommentID(commentID.Value);
            comment.CommentAuthor = HttpContext.Session.GetString("username");
            comment.CommentTime = DateTime.Now;
            comment.CommentContent = content;
            CommentDAO.UpdateComment(comment);
            return RedirectToAction("ViewComment", new {postId = comment.PostId});
        }

        public IActionResult DeleteComment(int? commentId)
        {
            if (commentId == null) return NotFound();
            Comment comment = CommentDAO.GetCommentByCommentID(commentId.Value);
            comment.IsDeleted = 1;
            CommentDAO.UpdateComment(comment);
            return RedirectToAction("ViewComment", new {postId = comment.PostId});
        }
        
    }
}