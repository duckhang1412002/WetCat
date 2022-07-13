using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

    }
}