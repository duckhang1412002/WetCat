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

        public Comment GetCommentByCommentID(int commentID) {
            Comment comment = null;
            try {
                using var context = new WetCat_DBContext();
                foreach (var cmt in context.Comments.ToList()) {
                    if (cmt.CommentId == commentID) {
                        comment = cmt;
                        break;
                    }
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return comment;
        }

        public void AddNewComment(Comment comment) {
            try {
                using var _db = new WetCat_DBContext();
                _db.Comments.Add(comment);
                _db.SaveChanges();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateComment(Comment comment) {
            try {
                using var _db = new WetCat_DBContext();
                _db.Comments.Update(comment);
                _db.SaveChanges();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}