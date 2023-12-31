﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using QueueSystem.Utilities;
using QueueSystem.Models;

namespace QueueSystem.Controllers
{

    public class AccountController : Controller
    {
        private const string _tableName = "Users";
        private IConfiguration _config;
        CommonHelper _helper;
        public AccountController(IConfiguration config)
        {
            _config = config;
           _helper = new CommonHelper(_config);

        }
        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginM objLoginModel = new LoginM();
            objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginM vm)
        {
            
            if (string.IsNullOrEmpty(vm.Name) && string.IsNullOrEmpty(vm.Password))
            {
                return View();
            }
            else
            {
                bool Isfind = SignInMethod(vm.Name, vm.Password);
               // var getUserRole = HttpContext.Session.GetString("Role");
                //var getUserServingPointId = HttpContext.Session.GetInt32("ServicePointId");
                if (Isfind == true)
                {
                    var claims = new List<Claim>
                    {
                        //new Claim(ClaimTypes.NameIdentifier,Convert.ToString(getUserServingPointId)),
                        new Claim(ClaimTypes.Name, vm.Name),
                        //new Claim(ClaimTypes.Role, getUserRole)

                    };

                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties() { IsPersistent = vm.RememberLogin });

                    if (vm.ReturnUrl == "/Admin/Dashboard")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if ((vm.ReturnUrl == "/Queue/ServicePoint"))
                    {
                        return RedirectToAction("ServicePoint", "Queue");
                    }
                }
            }
            // var role = HttpContext.Session.GetString("Role");
            //Add if condition according to role
            var  role = "Role";
            if (role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return RedirectToAction("ServicePoint", "Queue");
            }
        }
        private bool SignInMethod(string name, string password)
        {
            bool flag = false;
            string query = $"select * from {_tableName} where Name='{name}' and Password='{password}'  ";
            var userDetails = _helper.GetUserByUserName(query);

            if (userDetails.Name != null)
            {
                flag = true;

                HttpContext.Session.SetString("Name", userDetails.Name);
                HttpContext.Session.SetString("Role", userDetails.Role);
                HttpContext.Session.SetInt32("ServicePointId", userDetails.ServicePointId);
            }
            return flag;
        }
        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page
            return LocalRedirect("/");
        }
    }
}
