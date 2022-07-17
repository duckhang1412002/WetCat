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
    public class HobbyListController: Controller
    {
        HobbyListDAO hobbyListDAO = new HobbyListDAO();
        public HobbyListController(){}

        public IActionResult Index(string usn){
            HobbyDAO hobbyDAO = new HobbyDAO();
            List<HobbyList> hobbyLists = new List<HobbyList>();
            List<Hobby> hobbies = new List<Hobby>();
            hobbyLists = hobbyListDAO.GetHobbyList(usn);
            return View(hobbyLists);
        }
        /*[HttpPost]
        public IActionResult Delete(){
            var Follows = DB.Follows.ToList();
            return View(Follows);
        }*/

    }
}