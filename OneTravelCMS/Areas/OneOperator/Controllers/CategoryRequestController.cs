﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OneTravelCMS.Areas.OneOperator.Controllers
{
    public class CategoryRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}