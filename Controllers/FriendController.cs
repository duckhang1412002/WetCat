using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using WetCat.Models;
using WetCat.DAO;

namespace WetCat.Controllers
{
    public class FriendController: Controller
    {
        FriendDAO friendDAO = new FriendDAO();
        public FriendController(){}

        public IActionResult Index(){
            List<Friend> friendlist = friendDAO.GetFriendList().ToList();
            return View(friendlist);
        }
        /*[HttpPost]
        public IActionResult Delete(){
            var Follows = DB.Follows.ToList();
            return View(Follows);
        }*/
        public IActionResult FriendStatus(string id){
            System.Console.WriteLine("ID KB: " + id);
            Friend fr = friendDAO.GetFrienders(HttpContext.Session.GetString("username"), id);
            System.Console.WriteLine(fr.FirstUsername + "  -- " + fr.SecondUsername);
            return PartialView("/Views/Friend/_FriendStatus.cshtml", fr);
        }
    }
}