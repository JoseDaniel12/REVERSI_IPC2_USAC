using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIpc2.Content.Csharp;

namespace ProyectoIpc2.Controllers
{
    public class MenuPrincipalController : Controller
    {
        // GET: MenuPrincipal
        public ActionResult MenuPrincipal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult opcionSeleccionada(FormCollection collection)
        {
            GameLogic.tipoPartida = Request.Params["opcion"];
            Debug.WriteLine(GameLogic.tipoPartida);
            return new EmptyResult(); 
        }
    }
}