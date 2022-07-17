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

        public List<Friend> GetFriendList(string id) {
            List<Friend> friends = null;
            try {
                using var context = new WetCat_DBContext();
                friends = context.Friends.Where(c => ((c.FirstUsername == id || c.SecondUsername == id) && (c.FriendStatus == 3))).ToList();
                 foreach(var fr in friends){
                    UserDAO userDAO = new UserDAO();
                    fr.FirstUsernameNavigation = userDAO.GetUserByUsername(fr.FirstUsername);
                    fr.SecondUsernameNavigation = userDAO.GetUserByUsername(fr.SecondUsername);
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return friends;
        }

        public List<Friend> GetRequestList(string id) {
            List<Friend> friends = null;
            try {
                using var context = new WetCat_DBContext();
                friends = context.Friends.Where(c => (c.FirstUsername == id && c.FriendStatus == 2) || (c.SecondUsername == id && c.FriendStatus == 1)).ToList();
                 foreach(var fr in friends){
                    UserDAO userDAO = new UserDAO();
                    fr.FirstUsernameNavigation = userDAO.GetUserByUsername(fr.FirstUsername);
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

        public void AddFriend(string userid, string friendID) {
            try {
                using var context = new WetCat_DBContext();
                Friend _friend = GetFrienders(userid, friendID);
                //ID not collapse
                if (_friend == null) {
                    Friend fr = new Friend();
                    fr.FirstUsername = userid;
                    fr.SecondUsername = friendID;
                    fr.FriendStatus = 1;
                    fr.StatusTime = DateTime.Now;
                    context.Friends.Add(fr);
                    context.SaveChanges();
                } else if(_friend.FriendStatus == 0){
                    if (_friend.FirstUsername == userid){
                        _friend.FriendStatus = 1;
                    } else {
                        _friend.FriendStatus = 2;
                    }
                    context.Update(_friend);
                    context.SaveChanges();
                } else {
                    throw new Exception("The friend is already exist!");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void AcceptRequest(string userid, string friendID) {
            try {
                System.Console.WriteLine("ACCEPT" + userid + friendID);
                Friend _friend = GetFrienders(userid, friendID);
                //ID not collapse
                System.Console.WriteLine("Accept 1");
                if (_friend != null) {
                    System.Console.WriteLine("Accept 2");
                    using var context = new WetCat_DBContext();
                    if(_friend.FriendStatus == 1 || _friend.FriendStatus == 2){
                        _friend.FriendStatus = 3;
                        _friend.StatusTime = DateTime.Now;
                        context.Update(_friend);
                        context.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void UnfriendOrRefuse(string userid, string friendID) {
            try {
                Friend _friend = GetFrienders(userid, friendID);
                //ID not collapse
                if (_friend != null) {
                    using var context = new WetCat_DBContext();
                    if(_friend.FriendStatus == 3 || _friend.FriendStatus == 2 || _friend.FriendStatus == 1){
                        _friend.FriendStatus = 0;
                        _friend.StatusTime = DateTime.Now;
                        context.Update(_friend);
                        context.SaveChanges();
                    }
                } else {
                    throw new Exception("The friend is not exist!");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
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