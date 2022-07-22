using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WetCat.Models;
using System.Dynamic;
using WetCat.DAO;

namespace WetCat.Controllers
{
    public class ReactController: Controller
    {
        ReactDAO reactDAO = new ReactDAO();
        public ReactController(){}
        
        public IActionResult GetReactStatus(string id){
            ReactList rl = reactDAO.GetReactStatus(Convert.ToInt32(id), HttpContext.Session.GetString("username"));
            if(rl == null){
                rl = new ReactList();
                rl.PostId = Convert.ToInt32(id);
                rl.ReactType = "";
            }
            System.Console.WriteLine("DAY LA REACT STATUS " + id);
            return PartialView("/Views/React/_ReactStatus.cshtml", rl);
        }

        public IActionResult SetReact(string id, string type){
            if (type == "delete"){
                reactDAO.Unreact(Convert.ToInt32(id), HttpContext.Session.GetString("username"));
            } else{
                type = type.Substring(0, type.Length - "-icon".Length);
            System.Console.WriteLine("SET " + id + type);
            reactDAO.ReactPost(Convert.ToInt32(id), HttpContext.Session.GetString("username"), type);
            }
                return GetReactStatus(id);
            
        }
    }
}