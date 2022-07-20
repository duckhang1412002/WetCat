using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.DAO;
using WetCat.Models;

namespace WetCat.Controllers
{
    public class UserController : Controller
    {
        UserDAO userDAO = null;
        public UserController() {
            userDAO = new UserDAO();
        }
        public IActionResult Index()
        {
            string usn = HttpContext.Session.GetString("username");
            User user = userDAO.GetUserByUsername(usn);
            return View(user);
        }
        
        public IActionResult Edit()
        {
            string username = HttpContext.Session.GetString("username");
            User user = userDAO.GetUserByUsername(username);
            //System.Console.WriteLine("Hi");
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user, string gender)
        {
            user.Gender = (gender == "Male") ? 1 : 0;
            userDAO.EditUser(user);
            return Redirect ("/Wall/" + user.Username + "/timeline");
        }

        [HttpPost]
        public IActionResult UploadAvatar(string file)
        {
            System.Console.WriteLine("Im in upload avatar!");
            string author = HttpContext.Session.GetString("username");  
            byte[] bytes = Convert.FromBase64String(file.Split(',')[1]);
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/" + author);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            System.Console.WriteLine("Created dir path!");
            string fileName = Guid.NewGuid() + ".png";
            var filePath = Path.Combine(dirPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            string imgSrc = String.Format("images/profiles/{0}/{1}", author, fileName);
            userDAO.UpdateAvatar(author,imgSrc);
            System.Console.WriteLine(userDAO.GetUserByUsername(author).AvatarSrc);
            return Redirect ("/Wall/" + author + "/timeline");
        }
        
        [HttpPost]
        public IActionResult UploadWallpaper(IFormFile file)  
        {  
            System.Console.WriteLine("Im in upload wallpaper");
            string imgSrc = null;  
            string author = HttpContext.Session.GetString("username");   
            if (file != null)  
            {  
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/" + author);

                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

                var filePath = Path.Combine(dirPath, file.FileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                imgSrc = String.Format("images/profiles/{0}/{1}", author, file.FileName);
                userDAO.UpdateWallpaper(author,imgSrc);
            }  
            return Redirect ("/Wall/" + author + "/timeline");      
        }
    }
}