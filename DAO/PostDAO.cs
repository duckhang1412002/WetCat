using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetCat.Models;

namespace WetCat.DAO
{
    public class PostDAO
    {
        private static PostDAO instance = null;
        private static readonly object instanceLock = new object();
        public static PostDAO Instance{
            get{
                lock(instanceLock){
                    if(instance == null){
                        instance = new PostDAO();
                    }
                    return instance;
                }
            }
        }

        public User FindByUsername(string username) {
            var users = new List<User>();
            try {
                using var _db = new WetCat_DBContext();
                users = _db.Users.ToList();
                foreach(var user in users){
                    if (user.Username.Equals(username)) return user;
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public Post GetPost(int postId) {
            Post post = null;
            try {
                using var _db = new WetCat_DBContext();
                post = _db.Posts.Find(postId);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return post;
        }

        public IEnumerable<Post> GetAllPosts() {
            var posts = new List<Post>();
            try {
                using var _db = new WetCat_DBContext();
                posts = _db.Posts.ToList();
                foreach(var post in posts){
                    var user = FindByUsername(post.PostAuthor);
                    if (user != null) {
                        post.PostAuthorNavigation = user;
                    }
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return posts;
        }

        public void CreatePost(Post post){
            try{
                using var _db = new WetCat_DBContext();
                _db.Posts.Add(post);
                _db.SaveChanges();
            } catch (Exception ex) {
                throw new Exception(ex.Message);           
            }
        }

        public void DeletePost(Post post){
            try{
                using var _db = new WetCat_DBContext();
                _db.Remove(post);
                _db.SaveChanges();
            } catch (Exception ex) {
                throw new Exception(ex.Message);           
            }
        }

        public Post FindPost(int postid) {
            Post post = new Post();
            try {
                using var _db = new WetCat_DBContext();
                post = _db.Posts.Find(postid);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return post;
        }
        public void EditPost(Post post){
            System.Console.WriteLine(post.PostId);
            try {
                Post _post = GetPost(post.PostId);
                if (_post != null) {
                    using var _db = new WetCat_DBContext();
                    _db.Posts.Update(post);
                    _db.SaveChanges();
                } else {
                    throw new Exception();
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

         public void EditPost1(Post post){ //change isDeleted to 1
            try {
                Post _post = FindPost(post.PostId);
                if (_post != null) {
                    using var _db = new WetCat_DBContext();
                    _post.IsDeleted = 1;
                    _db.Posts.Update(_post);
                    _db.SaveChanges();
                } else {
                    throw new Exception();
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Post> GetPosts() {
            var postLists = new List<Post>();
            try {
                using var _db = new WetCat_DBContext();
                postLists = _db.Posts.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return postLists;
        }
    }
}

