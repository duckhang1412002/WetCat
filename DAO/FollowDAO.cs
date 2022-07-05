using System;
using System.Collections.Generic;
using System.Linq;
using WetCat.Models;
using System.Threading.Tasks;

namespace WetCat.DAO
{
    public class FollowDAO
    {
         private static FollowDAO instance = null;
        private static readonly object instanceLock = new object();
        public static FollowDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new FollowDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Follow> GetFollowList() {
            var follows = new List<Follow>();
            try {
                using var context = new WetCat_DBContext();
                follows = context.Follows.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return follows;
        }

        public Follow GetFollowStatus(string follower, string followed) {
            Follow follow = null;
            try {
                using var context = new WetCat_DBContext();
                follow = context.Follows.SingleOrDefault(c => c.FollowerUsername.Equals(follower) && c.FollowedUsername.Equals(followed));
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return follow;
        }

        public Follow GetFollowers(string followID) {
            Follow follow = null;
            try {
                using var context = new WetCat_DBContext();
                follow = context.Follows.SingleOrDefault(c => c.FollowedUsername == followID);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return follow;
        }

        /*public void AddNew(Follow follow) {
            try {
                Follow _follow = GetFollowStatus(follow.FollowerUsername, follow.FollowedUsername);
                //ID not collapse
                if (_follow == null) {
                    using var context = new WetCat_DBContext();
                    context.Follows.Add(follow); 
                    context.SaveChanges();
                } else {
                    throw new Exception("The follow is already exist.");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }*/

        public void Follow(Follow follow) {
            try {
                Follow _follow = GetFollowStatus(follow.FollowerUsername, follow.FollowedUsername);
                if (_follow != null) {
                    using var context = new WetCat_DBContext();
                    context.Follows.Add(follow);
                    context.SaveChanges();
                } else {
                    throw new Exception("The follow does not not exist.");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void Unfollow(string follower, string followed) {
            try {
                Follow _follow = GetFollowStatus(follower, followed);
                if (_follow != null) {
                    using var context = new WetCat_DBContext();
                    context.Follows.Remove(_follow);
                    context.SaveChanges();
                } else {
                    throw new Exception("The follow does not not exist.");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}