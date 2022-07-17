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
            List<Friend> friendlist = friendDAO.GetFriendList (HttpContext.Session.GetString ("username"));
            return View (friendlist);
        }
        /*[HttpPost]
        public IActionResult Delete(){
            var Follows = DB.Follows.ToList();
            return View(Follows);
        }*/
        public IActionResult FriendStatus (string id) {
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
            friendDAO.AddFriend (HttpContext.Session.GetString("username"), usn);
            return Redirect ("/Wall/" + usn + "/timeline");
        }
        public IActionResult Accept (string usn) {
            friendDAO.AcceptRequest(HttpContext.Session.GetString("username"), usn);
            return Redirect ("/Wall/" + usn + "/timeline");
        }
        public IActionResult UnfriendAtWall(string usn) {
            friendDAO.UnfriendOrRefuse(HttpContext.Session.GetString ("username"), usn);
            System.Console.WriteLine("UNFRIEND " + usn);
            return Redirect ("/Wall/" + usn + "/timeline");
        }
        public IActionResult UnfriendAtList(string usn) {
            friendDAO.UnfriendOrRefuse(HttpContext.Session.GetString ("username"), usn);
            System.Console.WriteLine("UNFRIEND " + usn);
            return Redirect ("/Wall/" + HttpContext.Session.GetString ("username") + "/friends");
        }
    }
}