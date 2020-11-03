﻿using System;
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
            int editX = Int32.Parse(coordenada.Split('_')[0]);
            int editY = Int32.Parse(coordenada.Split('_')[1]);
            GameLogic.colocarFicha(editX, editY);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tableroDeColores));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            info.Add("tirosPosibles", JsonConvert.SerializeObject(GameLogic.tirosPosibles));
            info.Add("hostColor", JsonConvert.SerializeObject(GameLogic.hostColor));
            info.Add("ganador", JsonConvert.SerializeObject(GameLogic.ganador));
            info.Add("anchoTablero", JsonConvert.SerializeObject(GameLogic.anchoTablero));
            info.Add("altoTablero", JsonConvert.SerializeObject(GameLogic.altoTablero));
            return Content(JsonConvert.SerializeObject(info));
        }

        [HttpPost]
        public ActionResult GuardarPartida(FormCollection collection) {
            SaveGame.guardar(GameLogic.gameId);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult CargarPartida(FormCollection collection) {
            string fileRoot = Request.Params["fileRoot"];
            LoadGame.cargar(fileRoot);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tableroDeColores));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            info.Add("tirosPosibles", JsonConvert.SerializeObject(GameLogic.tirosPosibles));
            info.Add("hostColor", JsonConvert.SerializeObject(GameLogic.hostColor));
            info.Add("ganador", JsonConvert.SerializeObject(GameLogic.ganador));
            info.Add("anchoTablero", JsonConvert.SerializeObject(GameLogic.anchoTablero));
            info.Add("altoTablero", JsonConvert.SerializeObject(GameLogic.altoTablero));
            return Content(JsonConvert.SerializeObject(info));
        }


        [HttpPost]
        public ActionResult PcPlayerMove(FormCollection collection) {
            PcPlayer.move(GameLogic.tablero, GameLogic.tirosPosibles, GameLogic.turno);
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tableroDeColores));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            info.Add("tirosPosibles", JsonConvert.SerializeObject(GameLogic.tirosPosibles));
            info.Add("hostColor", JsonConvert.SerializeObject(GameLogic.hostColor));
            info.Add("ganador", JsonConvert.SerializeObject(GameLogic.ganador));
            info.Add("anchoTablero", JsonConvert.SerializeObject(GameLogic.anchoTablero));
            info.Add("altoTablero", JsonConvert.SerializeObject(GameLogic.altoTablero));
            return Content(JsonConvert.SerializeObject(info));
        }

        [HttpPost]
        public ActionResult CambiarColor() {
            Dictionary<string, string> colorInfo = new Dictionary<string, string>();
            if (GameLogic.player1Points == 2 && GameLogic.player2Points == 2) {
                string temporal = GameLogic.jugador_negro;
                GameLogic.jugador_negro = GameLogic.jugador_blanco;
                GameLogic.jugador_blanco = temporal;
                GameLogic.hostColor = (GameLogic.hostColor == 1) ? 2 : 1;
                colorInfo.Add("isChanged", JsonConvert.SerializeObject(true));
            } else {
                colorInfo.Add("isChanged", JsonConvert.SerializeObject(false));
            }
            colorInfo.Add("hostColor", JsonConvert.SerializeObject(GameLogic.hostColor));
            return Content(JsonConvert.SerializeObject(colorInfo));
        }


        [HttpPost]
        public ActionResult CambiarNombre(FormCollection collection) {
            if (GameLogic.hostColor == 1) {
                GameLogic.jugador_blanco = Request.Params["nombre"];
            } else {
                GameLogic.jugador_negro = Request.Params["nombre"];
            }
            return Content(JsonConvert.SerializeObject(GameLogic.hostColor));
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

        [HttpPost]
        public ActionResult RegistrarTiempo(FormCollection collection) {
            if (GameLogic.turno == 1) {
                GameLogic.tiempoSegP1 += Int32.Parse(Request.Params["segundos"].ToString());
                GameLogic.tiempoSegP1 += (Int32.Parse(Request.Params["minutos"].ToString())) * 60;
            } else {
                GameLogic.tiempoSegP2 += Int32.Parse(Request.Params["segundos"].ToString());
                GameLogic.tiempoSegP2 += (Int32.Parse(Request.Params["minutos"].ToString())) * 60;
            }
            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult PrepararTablero() {
            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("tablero", JsonConvert.SerializeObject(GameLogic.tableroDeColores));
            info.Add("turno", JsonConvert.SerializeObject(GameLogic.turno));
            info.Add("player1MovesNumber", JsonConvert.SerializeObject(GameLogic.player1MovesNumber));
            info.Add("player2MovesNumber", JsonConvert.SerializeObject(GameLogic.player2MovesNumber));
            info.Add("player1Points", JsonConvert.SerializeObject(GameLogic.player1Points));
            info.Add("player2Points", JsonConvert.SerializeObject(GameLogic.player2Points));
            info.Add("tipoPartida", JsonConvert.SerializeObject(GameLogic.tipoPartida));
            info.Add("jugador_negro", JsonConvert.SerializeObject(GameLogic.jugador_negro));
            info.Add("jugador_blanco", JsonConvert.SerializeObject(GameLogic.jugador_blanco));
            info.Add("haTerminado", JsonConvert.SerializeObject(GameLogic.haTerminado));
            info.Add("tirosPosibles", JsonConvert.SerializeObject(GameLogic.tirosPosibles));
            info.Add("hostColor", JsonConvert.SerializeObject(GameLogic.hostColor));
            info.Add("ganador", JsonConvert.SerializeObject(GameLogic.ganador));
            info.Add("anchoTablero", JsonConvert.SerializeObject(GameLogic.anchoTablero));
            info.Add("altoTablero", JsonConvert.SerializeObject(GameLogic.altoTablero));
            return Content(JsonConvert.SerializeObject(info));
        }

        [HttpPost]
        public ActionResult ValidarTiro(FormCollection collection) {
            string coordenada = Request.Params["coordenada"].ToString();
            int x = Int32.Parse(coordenada.Split('_')[0]);
            int y = Int32.Parse(coordenada.Split('_')[1]);
            foreach (int[] tiro in GameLogic.tirosPosibles) {
                if (tiro[0] == x && tiro[1] == y) {
                    return Content(JsonConvert.SerializeObject(true));
                }
            }
            return Content(JsonConvert.SerializeObject(false));
        }

        [HttpPost]
        public ActionResult Continuar() {
            ChampionshipManager.manage();
            return Content(JsonConvert.SerializeObject(ChampionshipManager.estadosCampeonato));
        }

        [HttpPost]
        public ActionResult ImponerGanador(FormCollection collection) {
            string jugadorGandor = Request.Params["jugadorGanador"].ToString();
            if (ChampionshipManager.equipos[0]["jugador1"] == jugadorGandor) {
                ChampionshipManager.manage(true, 0, 0);
            } else if (ChampionshipManager.equipos[0]["jugador2"] == jugadorGandor) {
                ChampionshipManager.manage(true, 1, 0);
            } else if (ChampionshipManager.equipos[0]["jugador3"] == jugadorGandor) {
                ChampionshipManager.manage(true, 2, 0);
            } else if (ChampionshipManager.equipos[1]["jugador1"] == jugadorGandor) {
                ChampionshipManager.manage(true, 0, 1);
            } else if (ChampionshipManager.equipos[1]["jugador2"] == jugadorGandor) {
                ChampionshipManager.manage(true, 1, 1);
            } else if (ChampionshipManager.equipos[1]["jugador3"] == jugadorGandor) {
                ChampionshipManager.manage(true, 2, 1);
            } else {
                ChampionshipManager.estadosCampeonato["ganadorCorrecto"] = false;
            }
            return Content(JsonConvert.SerializeObject(ChampionshipManager.estadosCampeonato));
        }

        [HttpPost]
        public ActionResult RefrescarFinal () {
            int team1Points = Convert.ToInt32(ChampionshipManager.equipos[0]["puntos"]);
            int team2oints = Convert.ToInt32(ChampionshipManager.equipos[1]["puntos"]);
            if (GameLogic.player1Points > GameLogic.player2Points) {
                team1Points += 3;
            } else if (GameLogic.player2Points > GameLogic.player1Points) {
                team2oints += 3;
            } else {
                team1Points += 1;
                team2oints += 1;
            }
            Dictionary<string, string> infoToUpdate = new Dictionary<string, string>();
            infoToUpdate.Add("team1Points", team1Points.ToString());
            infoToUpdate.Add("team2Points", team2oints.ToString());
            infoToUpdate.Add("team1Name", ChampionshipManager.equipos[0]["nombreEquipo"]);
            infoToUpdate.Add("team2Name", ChampionshipManager.equipos[1]["nombreEquipo"]);
            return Content(JsonConvert.SerializeObject(infoToUpdate));
        }

    }
}