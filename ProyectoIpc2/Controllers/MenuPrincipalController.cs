using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIpc2.Content.Csharp;
using ProyectoIpc2.Models;

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
            if (GameLogic.tipoPartida == "vsJugador") {
                using (ReversiContext db = new ReversiContext()) {
                    Random random = new Random();
                    int numero = random.Next(1, 3);
                    if (numero == 1) {
                        GameLogic.jugador_negro = db.Usuario.Find(GameLogic.userId).Name;
                        GameLogic.jugador_blanco = "Invitado";
                    } else {
                        GameLogic.jugador_blanco= db.Usuario.Find(GameLogic.userId).Name;
                        GameLogic.jugador_negro = "Invitado";
                    }
                }
            } else if (GameLogic.tipoPartida == "vsPc") {
                using (ReversiContext db = new ReversiContext()) {
                        GameLogic.jugador_negro = db.Usuario.Find(GameLogic.userId).Name;
                        GameLogic.jugador_blanco = "PC";       
                }
            }
            return new EmptyResult(); 
        }
    }
}