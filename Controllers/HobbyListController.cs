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
using System.Dynamic;

namespace WetCat.Controllers
{
    public class HobbyListController: Controller
    {
        HobbyListDAO hobbyListDAO = new HobbyListDAO();
        public HobbyListController(){}

        public IActionResult HobbyList(string id){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            HobbyDAO hobbyDAO = new HobbyDAO();
            List<HobbyList> hobbyLists = new List<HobbyList>();
            List<Hobby> hobbies = new List<Hobby>();
            hobbies = hobbyDAO.GetAllHobby();
            hobbyLists = hobbyListDAO.GetHobbyList(id);
            dynamic model = new ExpandoObject();
            List<Hobby> exceptList = new List<Hobby>();
            foreach(HobbyList hl in hobbyLists){
                exceptList.Add(hobbyDAO.GetHobbyByID(hl.HobbyId));
            }            
            for(int i = 0; i < hobbies.Count; ++i){
                for(int j = 0; j < exceptList.Count; ++j){
                    if(hobbies[i].HobbyId == exceptList[j].HobbyId){
                        hobbies.Remove(hobbies[i]);
                        i -= 1;
                        break;
                    }
                }
            }    
            model.HobbyItems = hobbies.Except(exceptList);  
            model.HobbyList = hobbyLists;
            model.User = id;
            return View(model);
        }
        [HttpPost]
        public IActionResult AddHobby(string hb){
            if (HttpContext.Session.GetString ("username") == null) {
                return RedirectToAction ("Index", "Home");
            }
            System.Console.WriteLine("HObby " + Convert.ToInt32(hb));
            hobbyListDAO.AddHobby(HttpContext.Session.GetString("username"), Convert.ToInt32(hb));
            return Redirect("/Wall/" + HttpContext.Session.GetString("username") + "/hobby");
        }
        public IActionResult RemoveHobby(string id){
            hobbyListDAO.RemoveHobby(HttpContext.Session.GetString("username"), Convert.ToInt32(id));
            return Redirect("/Wall/" + HttpContext.Session.GetString("username") + "/hobby");
        }
        /*[HttpPost]
        public IActionResult Delete(){
            var Follows = DB.Follows.ToList();
            return View(Follows);
        }*/

    }
}