using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetCat.Models;

namespace WetCat.DAO
{
    public class ReactDAO
    {
        private static ReactDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ReactDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new ReactDAO();
                    }
                    return instance;
                }
            }
        }

        public void ReactPost(int postId, string reacter, string type){
            try {
                using var _db = new WetCat_DBContext();
                ReactList rct = _db.ReactLists.Where(r => r.PostId == postId && r.Username == reacter).FirstOrDefault();
                if(rct != null){
                    _db.ReactLists.Remove(rct);
                    _db.SaveChanges();
                }
                rct = new ReactList();
                rct.ReactType = type;
                rct.PostId = postId;
                rct.Username = reacter;
                _db.ReactLists.Add(rct);
                _db.SaveChanges();
            } catch (Exception e) {
                System.Console.WriteLine(e.InnerException.Message);
            }
            
        }

        public List<ReactList> GetReactList(int postId){
            using var _db = new WetCat_DBContext();
            List<ReactList> rl = _db.ReactLists.Where(r => r.PostId == postId).ToList();
            return rl;
        }

        public ReactList GetReactStatus(int postId, string usn){
            using var _db = new WetCat_DBContext();
            ReactList rl = _db.ReactLists.Where(r => r.PostId == postId && r.Username == usn).FirstOrDefault();
            return rl;
        }
        public void Unreact(int postId, string usn){
            using var  _db = new WetCat_DBContext();
            ReactList rl = _db.ReactLists.Where(r => r.PostId == postId && r.Username == usn).FirstOrDefault();
            if(rl != null){
                _db.ReactLists.Remove(rl);
                _db.SaveChanges();
            }
            
        }
    }
}