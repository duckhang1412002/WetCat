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
    public class UserManagementController: Controller
    {
        UserDAO userList = null;
        public UserManagementController() => userList = new UserDAO();

        /*
        public IActionResult Index(){
            var userLists = userList.GetUsers().ToList();
            return View(userLists);
        }*/
    }
}