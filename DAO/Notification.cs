using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WetCat.Models;

namespace WetCat.DAO
{
    public class NotificationDAO
    {
        private static NotificationDAO instance = null;
        private static readonly object instanceLock = new object();
        public static NotificationDAO Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new NotificationDAO();
                    }
                    return instance;
                }
            }
        }

        public Notification GetNoti(string id){
            using var context = new WetCat_DBContext();
            return context.Notifications.Find(id);
        }

    }
}