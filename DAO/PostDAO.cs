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
                _db.Add(post);
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
    }
}

