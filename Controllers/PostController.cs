using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class PostController : Controller
    {
        WetCat_DBContext DB = new WetCat_DBContext();

        PostDAO DAO = new PostDAO();
        public PostController(){}

        public IActionResult Index(){
            dynamic model = new ExpandoObject();

            var posts = DAO.GetAllPosts();
            PostViewModel post = new PostViewModel();
            System.Console.WriteLine(post.PostAuthor);
            
            model.postsList = posts; 
            model.createPost = post;
            return View(model);
        }  

        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public IActionResult CreatePost(PostViewModel postModel)  
        {  
            System.Console.WriteLine("Helllloooooo: " + postModel.PostAuthor); 
            System.Console.WriteLine("Post model:" + postModel.PostImgFile);

            if (ModelState.IsValid)  
            {  
                string uniqueFileName = UploadedFile(postModel);

                System.Console.WriteLine("Content: " + postModel.PostContent);  
  
                Post post = new Post(postModel.PrivacyMode, postModel.PostAuthor, postModel.PostTime, postModel.PostContent, uniqueFileName);
 
                
                /*dbContext.Add(employee);  
                await dbContext.SaveChangesAsync();*/
                return RedirectToAction(nameof(Index));  
            }  
            return RedirectToAction(nameof(Index));  
        }  

        private string UploadedFile(PostViewModel postModel)  
        {  
            string uniqueFileName = null;             
  
            if (postModel.PostImgFile != null)  
            {  
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/" + postModel.PostAuthor);

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(postModel.PostImgFile.FileName);

                string fileNameWithPath = Path.Combine(path, fileInfo.Extension);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    postModel.PostImgFile.CopyTo(stream);
                }  
            }  
            return uniqueFileName;  
        } 
    }
}