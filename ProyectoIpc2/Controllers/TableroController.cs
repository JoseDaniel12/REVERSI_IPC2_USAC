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
    public class TableroController : Controller {
        // GET: Tablero
        public ActionResult Tablero() {
            return View();
        }

        [HttpPost]
        public ActionResult CasillaPresionada(FormCollection collection) {
            string coordenada = Request.Params["coordenada"];
            int editX = Int32.Parse(coordenada[0].ToString());
            int editY = Int32.Parse(coordenada[1].ToString());
            GameLogic.colocarFicha(editX, editY);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tablero));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            return Content(JsonConvert.SerializeObject(info));
        }

        [HttpPost]
        public ActionResult GuardarPartida(FormCollection collection) {
            GameLogic.guardarPartida();
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CargarPartida(FormCollection collection) {
            string fileRoot = Request.Params["fileRoot"];
            GameLogic.cargarPartida(fileRoot);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tablero));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            return Content(JsonConvert.SerializeObject(info));
        }


        [HttpPost]
        public ActionResult PcPlayerMove(FormCollection collection) {
            int[] tiro = PcPlayer.move(GameLogic.tablero, GameLogic.tirosPosibles, GameLogic.turno);
            GameLogic.colocarFicha(tiro[0], tiro[1]);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tablero));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            return Content(JsonConvert.SerializeObject(info));
        }

        [HttpPost]
        public ActionResult CambiarColor() {
            if (GameLogic.player1Points == 2 && GameLogic.player2Points == 2) {
                string temporal = GameLogic.jugador_negro;
                GameLogic.jugador_negro = GameLogic.jugador_blanco;
                GameLogic.jugador_blanco = temporal;
                GameLogic.hostColor = (GameLogic.hostColor == 1) ? 2 : 1;
                return Content(JsonConvert.SerializeObject(true));
            }
            return Content(JsonConvert.SerializeObject(false));
        }

        [HttpPost]
        public ActionResult Salir() {
            GameLogic.reiniciarDatos();
            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult CerrarSesion() {
            GameLogic.userId = -1;
            GameLogic.reiniciarDatos();
            return new EmptyResult();
        }

    }
}