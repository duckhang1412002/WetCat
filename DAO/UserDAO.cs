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
            User user = new User();
            try {
                using var _db = new WetCat_DBContext();
                user = _db.Users.Find(username);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
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

        public void RemoveUser(User user) {
            try {
                System.Console.WriteLine("Day ne " + user.Username);
                using var _db = new WetCat_DBContext();
                _db.Users.Remove(user);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public void RegisterUser(User user) {
            try {
                user.Role = "User";
                using var _db = new WetCat_DBContext();
                _db.Users.Add(user);
                _db.SaveChanges();
                System.Console.WriteLine("Register successfully!!");
            } catch (Exception e) {
                System.Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
            }
        }

        public void EditUSer(User user){
            try {
                User _user = GetUserByUsername(user.Username);
                if (_user != null) {
                    using var _db = new WetCat_DBContext();
                    _db.Users.Update(user);
                    _db.SaveChanges();
                } else {
                    throw new Exception();
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void EditUSer1(User user){ //change isDeleted to 1
            try {
                User _user = GetUserByUsername(user.Username);
                if (_user != null) {
                    using var _db = new WetCat_DBContext();
                    _user.IsDeleted = 1;
                    _db.Users.Update(_user);
                    _db.SaveChanges();
                } else {
                    throw new Exception();
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}