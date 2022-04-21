using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index() =>  View();

        public IActionResult AddAccount() => View(new Account());

        public IActionResult Edit(int id) => new ViewResult();

    }
}
