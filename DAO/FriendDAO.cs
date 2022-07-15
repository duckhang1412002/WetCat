using System;
using System.Collections.Generic;
using System.Linq;
using WetCat.Models;
using System.Threading.Tasks;

namespace WetCat.DAO
{
    public class FriendDAO
    {
         private static FriendDAO instance = null;
        private static readonly object instanceLock = new object();
        public static FriendDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new FriendDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Friend> GetFriendList() {
            var friends = new List<Friend>();
            try {
                using var context = new WetCat_DBContext();
                friends = context.Friends.ToList();
                foreach(var fr in friends){
                    UserDAO userDAO = new UserDAO();
                    fr.SecondUsernameNavigation = userDAO.GetUserByUsername(fr.SecondUsername);
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return friends;
        }

        public Friend FriendRelation(string friender, string friended) {
            Friend friend = null;
            try {
                using var context = new WetCat_DBContext();
                friend = context.Friends.SingleOrDefault(c => (c.FirstUsername.Equals(friender) && c.SecondUsername.Equals(friended)) || (c.SecondUsername.Equals(friender) && c.FirstUsername.Equals(friended)));
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return friend;
        }

        public Friend GetFrienders(string userid, string friendID) {
            Friend friend = null;
            try {
                using var context = new WetCat_DBContext();
                friend = context.Friends.SingleOrDefault(c => (c.FirstUsername == friendID && c.SecondUsername == userid) || (c.FirstUsername == userid && c.SecondUsername == friendID));
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return friend;
        }

        /*public void AddNew(Friend friend) {
            try {
                Friend _friend = GetFriendStatus(friend.FrienderUsername, friend.FriendedUsername);
                //ID not collapse
                if (_friend == null) {
                    using var context = new WetCat_DBContext();
                    context.Friends.Add(friend); 
                    context.SaveChanges();
                } else {
                    throw new Exception("The friend is already exist.");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }*/
/*
        public void SendFriendRequest(string FirstUsername, string SecondUsername) {
            try {
                int? _friend = GetFriendStatus(FirstUsername, SecondUsername).FriendStatus;
                if (_friend == 0) {
                    using var context = new WetCat_DBContext();
                   // context.Friends.Add(friend);
                    context.SaveChanges();
                } else {
                    throw new Exception("The friend does not not exist.");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void Unfriend(string friender, string friended) {
            try {
                Friend _friend = GetFriendStatus(friender, friended);
                if (_friend != null) {
                    using var context = new WetCat_DBContext();
                    context.Friends.Remove(_friend);
                    context.SaveChanges();
                } else {
                    throw new Exception("The friend does not not exist.");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }*/
    }
}