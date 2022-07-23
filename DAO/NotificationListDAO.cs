using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetCat.Models;

namespace WetCat.DAO
{
    public class NotificationListDAO
    {
        private static NotificationListDAO instance = null;
        private static readonly object instanceLock = new object();
        public static NotificationListDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new NotificationListDAO();
                    }
                    return instance;
                }
            }
        }

        public void newNoti(string target, string causer, string type, int? cmtid, int? postid){
            System.Console.WriteLine("Causer " + causer + "tag" + target);
            if(target != causer){
                if(postid != null && isAlreadyHaveReactNoti(causer, postid.Value) && type == "react"){
                    deleteNoti(causer, postid.Value);
                }
                using var _db = new WetCat_DBContext();
                NotificationList noti = new NotificationList();
                noti.Target = target;
                noti.Causer = causer;
                noti.NotificationType = type;
                noti.PostId = postid;
                noti.CommentId = cmtid;
                noti.NotifyTime = DateTime.Now;
                System.Console.WriteLine(" ADĐ " + target + causer);
                _db.NotificationLists.Add(noti);
                _db.SaveChanges();
            }   
        }

        public List<NotificationList> getAllNoti(string usn){
            using var _db = new WetCat_DBContext();
            List<NotificationList> notlst = _db.NotificationLists.Where(n => n.Target == usn).ToList();
            UserDAO uD = new UserDAO();
            NotificationDAO  nD = new NotificationDAO();
            foreach(NotificationList n in notlst){
                n.CauserNavigation = uD.GetUserByUsername(n.Causer);
                n.TargetNavigation = uD.GetUserByUsername(n.Target);
                n.NotificationTypeNavigation = nD.GetNoti(n.NotificationType);
            }
            return notlst;
        }

        public void CheckAllNoti(string usn){
            using var _db = new WetCat_DBContext();
            List<NotificationList> notlst = _db.NotificationLists.Where(n => n.Target == usn).ToList();
            foreach(NotificationList n in notlst){
                n.IsDeleted = 1;
                _db.NotificationLists.Update(n);
                _db.SaveChanges();
            }
        }
        public bool isAlreadyHaveReactNoti(string usn, int postId){
            using var _db = new WetCat_DBContext();
            NotificationList notlst = _db.NotificationLists.Where(n => n.Causer == usn && n.PostId == postId).FirstOrDefault();
            if (notlst == null){
                return false;
            } else return true;
        }
        public void deleteNoti(string usn, int postId){
            using var _db = new WetCat_DBContext();
            List<NotificationList> notlst = _db.NotificationLists.Where(n => n.Causer == usn && n.PostId == postId && n.NotificationType == "react").ToList();
            if(notlst != null){
                foreach(NotificationList n in notlst){
                    _db.NotificationLists.Remove(n);
                    _db.SaveChanges();
                }
            }
        }
    }
}