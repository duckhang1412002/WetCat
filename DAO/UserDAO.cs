using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<User> GetUsers() {
            var userLists = new List<User>();
            try {
                using var _db = new WetCat_DBContext();
                userLists = _db.Users.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return userLists;
        }
    }
}