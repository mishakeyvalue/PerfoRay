using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL;

namespace PerfoRay.Controllers
{
    public class RayController : Controller
    {
        private ScansManager _manager { get; set; }
        public RayController(ScansManager manager)
        {
            _manager = manager;
        }
        public IActionResult Index()
        {
            return View(_manager.GetAll());
        }

    }
}
