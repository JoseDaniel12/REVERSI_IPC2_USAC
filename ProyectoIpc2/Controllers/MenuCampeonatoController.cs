using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIpc2.Content.Csharp;

namespace ProyectoIpc2.Controllers
{
    public class MenuCampeonatoController : Controller
    {
        public ActionResult MenuCampeonato()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarCampeonato(FormCollection collection) {
            ChampionshipManager.nombreCampeonato = Request.Params["coordenada"];
            ChampionshipManager.numeroEquipos = Int32.Parse(Request.Params["numeroEquipos"]);
            ChampionshipManager.iniciarCampeonato();
            return new EmptyResult();
        }
    }
}