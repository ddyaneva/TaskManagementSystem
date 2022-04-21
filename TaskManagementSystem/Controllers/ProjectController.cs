using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult Index() => View();
        public ActionResult Add() => View();

        public ActionResult Edit(int id) => View();
    }
}
