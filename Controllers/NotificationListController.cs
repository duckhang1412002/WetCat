using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.Models;
using WetCat.DAO;
using System.Dynamic;

namespace WetCat.Controllers
{
    public class NotificationListController: Controller
    {
        static NotificationListDAO notificationListDAO = new NotificationListDAO();
        static UserDAO userDAO = new UserDAO();
        public NotificationListController(){}
        public static void sendNoti(string type, int? postid, int? cmtid, string causer, string target){
            notificationListDAO.newNoti(target, causer, type, cmtid, postid);
        }
        public IActionResult NotificationList(){
            List<NotificationList> notiList = notificationListDAO.getAllNoti(HttpContext.Session.GetString("username")).Reverse<NotificationList>().ToList();
            dynamic model = new ExpandoObject();
            model.currentSessionUser = userDAO.GetUserByUsername(HttpContext.Session.GetString("username"));
            model.notiList = notiList;
            return View("NotificationList", model);
        }
        public IActionResult NotiCount(){
            int count = notificationListDAO.getAllNoti(HttpContext.Session.GetString("username")).Where(p => p.NotifyTime.AddHours(1) > DateTime.Now).Count();
            return PartialView("/Views/NotificationList/_NotiCount.cshtml", count);
        }
    }
}