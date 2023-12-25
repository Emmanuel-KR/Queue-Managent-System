using Microsoft.AspNetCore.Mvc;
using Npgsql;
using QueueSystem.Helper;
using QueueSystem.Models;

namespace QueueSystem.Controllers
{
    public class AccountsController : Controller
    {
        private IConfiguration _config;
        CommonHelper _helper;

        public AccountsController(IConfiguration config)
        {
            _helper = new CommonHelper(_config);
            _config = config;
            
        }
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM vm)
        {
            string UserExistsQuery = $"SELECT * FROM [RegistrationTb] WHERE Email = '{vm.Email}' " +
                $"OR MobileNumber = '{vm.MobileNumber}' ";
            bool UserExists = _helper.UserAlreadyExists(UserExistsQuery);
            if (UserExists == true) 
            {
                ViewBag.Error = "User Already Exists!";
                return View("Register", "Accounts");
            }
            string Query = "Insert into [RegistrationTb] (FirstName, LastName, Password, Email," +
                " MobileNumber, Address) values ('{vm.FirstName}','{vm.LastName}','{vm.Password}'," +
                "'{vm.Email}','{vm.MobileNumber}','{vm.Address}') ";
            int result = _helper.DMLTransaction(Query);
            if (result > 0 )
            {
                EntryIntoSession(vm.Email);
                return RedirectToAction("Index", "Accounts");
            }

            return View();
        }

        private void EntryIntoSession(string Email)
        {
            HttpContext.Session.SetString("Email", Email);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
