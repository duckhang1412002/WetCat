using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetCat.Models;

namespace WetCat.DAO
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();
        public static UserDAO Instance{
            get{
                lock(instanceLock){
                    if(instance == null){
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public User GetUserByUsername(string username) {
            User user = null;
            try {
                using var _db = new WetCat_DBContext();
                user = _db.Users.Find(username);
            } catch (Exception) {}
            return user;
        }

        public User LoginByUsernameAndPassword(string username, string password) {
            User user = null;
            try {
                using var _db = new WetCat_DBContext();
                user = _db.Users.Find(username);
                if (!user.Password.Equals(password)) user = null;
            } catch (Exception) {}
            return user;
            /* */
        }
    }
}