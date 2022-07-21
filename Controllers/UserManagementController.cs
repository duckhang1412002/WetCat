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

namespace WetCat.Controllers
{
    public class UserManagementController: Controller
    {
        WetCat_DBContext DB = new WetCat_DBContext();

        UserDAO UserDAO = new UserDAO();
        public UserManagementController(){}

        public IActionResult Index(){
            if (HttpContext.Session.GetString("username") == null) {
                return RedirectToAction("Index", "Home");
            }  
            var users = UserDAO.GetUsers().ToList();
            System.Console.WriteLine(users.Count);
            return View(users);
        }
        
        public ActionResult Delete(string username){
            if (username == null){
                return NotFound();
            }
            var user = UserDAO.GetUserByUsername(username);
            if (user == null){
                return NotFound();
            }
            return View(user);
        }

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(User user){
            try {
                using var _db = new WetCat_DBContext();
                _db.Users.Update(user);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex){
                ViewBag.Message = ex.Message;
            }
            return View();
        }
        */

        [HttpPost]
        public ActionResult Delete1(string username){
            try {
                UserDAO.EditUSer1(UserDAO.GetUserByUsername(username)); 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex){
                ViewBag.Message = ex.Message;
            }
            return View();
        }

        public ActionResult ShowUser(string username){
            if (username == null){
                return NotFound();
            }
            var user = UserDAO.GetUserByUsername(username);
            if (user == null){
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult ShowUser(User user){
            try {
                UserDAO.GetUsers();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex){
                ViewBag.Message = ex.Message;
            }
            return View();
        }
    }
}