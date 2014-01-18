using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmosMusic2.Controllers
{
    public class AlbumsController : Controller
    {
        //
        // GET: /Album/

        public ActionResult Index()
        {
            return View();
        }

    }
}
