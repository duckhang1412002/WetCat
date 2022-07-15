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
        public IActionResult Followers(){
            List<Follow> followers = followDAO.GetFollowers(HttpContext.Session.GetString("username")).ToList();
            return View(followers);
        }

        public IActionResult Followings(){
            List<Follow> followings = followDAO.GetFollowings(HttpContext.Session.GetString("username")).ToList();
            return View(followings);
        }
        public IActionResult UnfollowAtList(string usn){
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            System.Console.WriteLine("Unfollow " + usn);
            followDAO.Unfollow(HttpContext.Session.GetString("username"), usn);
            return View();
        }
        public IActionResult UnfollowAtWall(string usn){
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            System.Console.WriteLine("Unfollow " + usn);
            followDAO.Unfollow(HttpContext.Session.GetString("username"), usn);
            return View();
        }
    }
}