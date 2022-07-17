using System;
using System.Collections.Generic;
using System.Linq;
using WetCat.Models;
using System.Threading.Tasks;

namespace WetCat.DAO
{
    public class HobbyListDAO
    {
        private static HobbyListDAO instance = null;
        private static readonly object instanceLock = new object();
        public static HobbyListDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new HobbyListDAO();
                    }
                    return instance;
                }
            }
        }

        public List<HobbyList> GetHobbyList(string usn) {
            List<HobbyList> hobbies = null;
            try {
                using var context = new WetCat_DBContext();
                hobbies = context.HobbyLists.Where(h => h.Username == usn).ToList();
                HobbyDAO hobbyDAO = new HobbyDAO();
                foreach(HobbyList h in hobbies){
                    h.Hobby = hobbyDAO.GetHobbyByID(h.HobbyId);
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return hobbies;
        }
        public void AddHobby(string usn, int id) {
            HobbyList hobby = new HobbyList();
            try {
                using var context = new WetCat_DBContext();
                hobby.HobbyId = id;
                hobby.Username = usn;
                context.HobbyLists.Add(hobby);
                context.SaveChanges();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public void RemoveHobby(string usn, int id) {
            HobbyList hobby = new HobbyList();
            try {
                using var context = new WetCat_DBContext();
                hobby = context.HobbyLists.Find(id, usn);
                context.HobbyLists.Remove(hobby);
                context.SaveChanges();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}