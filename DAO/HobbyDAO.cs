using System;
using System.Collections.Generic;
using System.Linq;
using WetCat.Models;
using System.Threading.Tasks;

namespace WetCat.DAO
{
    public class HobbyDAO
    {
        private static HobbyDAO instance = null;
        private static readonly object instanceLock = new object();
        public static HobbyDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new HobbyDAO();
                    }
                    return instance;
                }
            }
        }

        public Hobby GetHobby(int id) {
            Hobby hobby = null;
            try {
                using var context = new WetCat_DBContext();
                hobby = context.Hobbies.Find(id);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return hobby;
        }
    }
}