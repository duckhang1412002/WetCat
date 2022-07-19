using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetCat.Models;

namespace WetCat.DAO
{
    public class CommentDAO
    {
        private static CommentDAO instance = null;
        private static readonly object instanceLock = new object();
        public static CommentDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new CommentDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Comment> GetCommentByPostID(int postID) {
            List<Comment> comments = null;
            try {
                using var context = new WetCat_DBContext();
                comments = context.Comments.Where(c => c.PostId == postID).ToList();
                foreach(var cm in comments){
                    UserDAO userDAO = new UserDAO();
                    cm.CommentAuthorNavigation = userDAO.GetUserByUsername(cm.CommentAuthor);
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return comments;
        }
    }
}