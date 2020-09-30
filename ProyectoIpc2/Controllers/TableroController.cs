using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProyectoIpc2.Content.Csharp;

namespace ProyectoIpc2.Controllers
{
    public class TableroController : Controller
    {
        // GET: Tablero
        public ActionResult Tablero()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CasillaPresionada(FormCollection collection) {
            string coordenada = Request.Params["coordenada"];
            int editX = Int32.Parse(coordenada[0].ToString());
            int editY = Int32.Parse(coordenada[1].ToString());
            GameLogic.colocarFicha(editX, editY);
            var tableroJs = JsonConvert.SerializeObject(GameLogic.tablero);
            var turnoJs = JsonConvert.SerializeObject(GameLogic.turno);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", tableroJs);
            info.Add("turno", turnoJs);
            return Content(JsonConvert.SerializeObject(info));
        }

        [HttpPost]
        public ActionResult GuardarPartida(FormCollection collection)
        {
            int[,] tablero = new int[8, 8];
            string position;
            string valorCasilla;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    position = x + "" + y;
                    valorCasilla = Request.Params[position];
                    tablero[x,y] = Int32.Parse(valorCasilla);
                }
            }
            GameLogic.guardarPartida(tablero);
            return Content("{\"Procedimiento\": \"exitoso\"}");
        }

        [HttpPost]
        public ActionResult CargarPartida(FormCollection collection)
        {
            string fileRoot = Request.Params["fileRoot"];
            GameLogic.cargarPartida(fileRoot);
            var tableroJson = JsonConvert.SerializeObject(GameLogic.tablero);
            return Content(tableroJson);
        }
    }
}