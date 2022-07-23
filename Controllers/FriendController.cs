using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers {
    public class FriendController : Controller {
        FriendDAO friendDAO = new FriendDAO ();
        public FriendController () { }

        public IActionResult Index () {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            List<Friend> friendlist = friendDAO.GetFriendList (HttpContext.Session.GetString ("username"));
            return View (friendlist);
        }
        public IActionResult RequestList () {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            List<Friend> requestList = friendDAO.GetRequestList(HttpContext.Session.GetString ("username"));
            requestList = requestList.OrderBy(p => p.StatusTime).Reverse<Friend>().ToList();
            return View (requestList);
        }
        public IActionResult FriendStatus (string id) {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            System.Console.WriteLine ("ID KB: " + id);
            Friend fr = friendDAO.GetFrienders (HttpContext.Session.GetString ("username"), id);
            if (fr == null) {
                fr = new Friend();  
                fr.FirstUsername = "";
                fr.SecondUsername = id;
            }
            return PartialView ("/Views/Friend/_FriendStatus.cshtml", fr);
        }

        public IActionResult AddFriend (string usn) {
            try {
                if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
                friendDAO.AddFriend (HttpContext.Session.GetString("username"), usn);
                NotificationListController.sendNoti("request", null, null, HttpContext.Session.GetString("username"),usn);
            } catch (Exception e) {
                System.Console.WriteLine(e.InnerException.Message);
            }
            
            return Redirect ("/Wall/" + usn + "/timeline");
        }
        public IActionResult AcceptAtWall(string usn) {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            friendDAO.AcceptRequest(HttpContext.Session.GetString("username"), usn);
            NotificationListController.sendNoti("accept", null, null, HttpContext.Session.GetString("username"),usn);
            return Redirect ("/Wall/" + usn + "/timeline");
        }
        public IActionResult AcceptAtList(string usn) {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            friendDAO.AcceptRequest(HttpContext.Session.GetString("username"), usn);
            NotificationListController.sendNoti("accept", null, null, HttpContext.Session.GetString("username"),usn);
            return Redirect ("/Wall/" + HttpContext.Session.GetString("username") + "/requests");
        }
        public IActionResult UnfriendAtWall(string usn) {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            friendDAO.UnfriendOrRefuse(HttpContext.Session.GetString ("username"), usn);
            return Redirect ("/Wall/" + usn + "/timeline");
        }
        public IActionResult UnfriendAtList(string usn) {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            friendDAO.UnfriendOrRefuse(HttpContext.Session.GetString ("username"), usn);
            return Redirect ("/Wall/" + HttpContext.Session.GetString ("username") + "/friends");
        }
        public IActionResult RefuseAtList(string usn) {
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            friendDAO.UnfriendOrRefuse(HttpContext.Session.GetString ("username"), usn);
            return Redirect ("/Wall/" + HttpContext.Session.GetString ("username") + "/requests");
        }
    }
}