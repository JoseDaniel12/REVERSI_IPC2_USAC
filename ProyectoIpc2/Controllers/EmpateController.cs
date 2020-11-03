using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIpc2.Content.Csharp;

namespace ProyectoIpc2.Controllers
{
    public class EmpateController : Controller
    {
        public ActionResult Empate() {
            return View();
        }

        [HttpPost]
        public ActionResult Desempatar(FormCollection collection) {
            string jugadorEquipo1 = Request.Params["jugadorEquipo1"];
            string jugadorEquipo2 = Request.Params["jugadorEquipo2"];
            GameLogic.reiniciarDatos();
            GameLogic.tipoPartida = "campeonato";
            GameLogic.iniciarJuego();
            GameLogic.jugador_negro = jugadorEquipo1;
            GameLogic.jugador_blanco = jugadorEquipo2;
            return new EmptyResult();
        }
    }
}