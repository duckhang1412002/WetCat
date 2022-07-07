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

        public IEnumerable<Post> GetAllPosts() {
            var posts = new List<Post>();
            try {
                using var _db = new WetCat_DBContext();
                posts = _db.Posts.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return posts;
        }

        public void Remove(int PostId){
        try{
            Post p = FindID(PostId);
            if(p != null){
                using var _db = new WetCat_DBContext();
                _db.Posts.Remove(p);
                _db.SaveChanges();
            }
            else {
                throw new Exception ("The Post does not already exist.");
            }
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
           }
        }
     

        public Post FindID(int PostId){ 
        Post p = null;
        try{
             using var _db = new WetCat_DBContext();
            p = _db.Posts.SingleOrDefault(p => p.PostId == PostId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
            return p;
        }

    }    
}

