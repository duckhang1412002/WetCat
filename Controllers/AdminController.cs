using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class AdminController: Controller
    {
        WetCat_DBContext DB = new WetCat_DBContext();

        PostDAO PostDAO = new PostDAO();
        public AdminController(){}

        public IActionResult Index(){
            var postLists = PostDAO.GetAllPosts().ToList();
            return View(postLists);
        }

        public ActionResult Delete(int postid){
            if (postid == 0){
                return NotFound();
            }
            var post = PostDAO.FindPost(postid);
            if (post == null){
                return NotFound();
            }
            return View(post);
        }

        /*
        [HttpPost]
        public ActionResult Delete(Post post){
            try {
                using var _db = new WetCat_DBContext();
                _db.Posts.Remove(post);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex){
                ViewBag.Message = ex.Message;
            }
            return RedirectToAction("Delete");
        }
        */

        [HttpPost]
        public ActionResult Delete1(int postid){
            try {
                PostDAO.EditPost1(PostDAO.FindPost(postid)); 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex){
                ViewBag.Message = ex.Message;
            }
            return View();
        }

        public ActionResult ShowPost(int postid){
            if (postid.Equals("")){
                return NotFound();
            }
            var post = PostDAO.GetPost(postid);
            if (post == null){
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult ShowPost(Post post){
            try {
                PostDAO.GetPosts();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex){
                ViewBag.Message = ex.Message;
            }
            return View();
        }
    }
}