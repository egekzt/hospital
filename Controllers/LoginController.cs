using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using hospital.Models;

namespace hospital.Controllers
{
    public class LoginController : Controller
    {
        
        public LoginController()
        {

        }
        public IActionResult Index()
        {
            return View("Login");
        }
        //public IActionResult Authenticate(string username, string password)
        //{
            // Perform authentication logic here
            // For simplicity, let's assume the username and password are hardcoded
           // var user = _hospitalContext.Find(x => x.Username == login.Username && x.Password == hashedPassword);

            
       // }
    }
}

