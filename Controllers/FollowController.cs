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

namespace WetCat.Controllers
{
    public class FollowController: Controller
    {
        FollowDAO followDAO = new FollowDAO();
        public FollowController(){}
/*
        [HttpPost]
        public IActionResult Delete(){
            var Follows = DB.Follows.ToList();
            return View(Follows);
        }
*/
        public IActionResult Follow(string usn){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            Follow fl = new Follow();
            fl.FollowerUsername = HttpContext.Session.GetString("username");
            System.Console.WriteLine("DUoc Follow" + usn);
            fl.FollowedUsername = usn;
            followDAO.Follow(fl);
            return Redirect("/Wall/" + usn + "/timeline");
        }
        public IActionResult Followers(string usn){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            List<Follow> followers = followDAO.GetFollowers(HttpContext.Session.GetString("username")).ToList();
            System.Console.WriteLine("Tui la Followers" + followers.Count);
            return View(followers);
        }

        public IActionResult Followings(string usn){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            List<Follow> followings = followDAO.GetFollowings(HttpContext.Session.GetString("username")).ToList();
            return View(followings);
        }
        public IActionResult UnfollowAtList(string usn){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            System.Console.WriteLine("Unfollow " + usn);
            followDAO.Unfollow(HttpContext.Session.GetString("username"), usn);
            return Redirect("/Wall/" + HttpContext.Session.GetString("username") + "/followings");
        }
        public IActionResult UnfollowAtWall(string usn){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            System.Console.WriteLine("Unfollow " + usn);
            followDAO.Unfollow(HttpContext.Session.GetString("username"), usn);
            return Redirect("/Wall/" + usn + "/timeline");
        }
        public IActionResult FollowStatus(string id){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            System.Console.WriteLine("ID Follow: " + id);
            Follow fl = followDAO.GetFollowStatus(HttpContext.Session.GetString("username"), id);
            if (fl == null){
                fl = new Follow();
                fl.FollowedUsername = id;
                fl.FollowerUsername = "";
            }
            return PartialView("/Views/Follow/_FollowStatus.cshtml", fl);
        }
    }
}