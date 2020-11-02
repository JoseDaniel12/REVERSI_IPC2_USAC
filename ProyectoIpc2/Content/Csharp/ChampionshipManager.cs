using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;
using ProyectoIpc2.Controllers;
using ProyectoIpc2.Models;

namespace ProyectoIpc2.Content.Csharp {
    public static class ChampionshipManager {
        public static string nombreCampeonato = "";
        public static int numeroEquipos = 0;
        public static List<Dictionary<string, string>> equipos = new List<Dictionary<string, string>>();
        public static List<Dictionary<string, string>> controlAuxiliar = new List<Dictionary<string, string>>();
        public static int contadorPartidas = 0;
        public static Dictionary<string, bool> estadosCampeonato = new Dictionary<string, bool> {
            {"ganadorCorrecto",false},
            {"hayEmpate", false},
            {"haTerminado", false},
        };

        public static void iniciarCampeonato() {
            int nJugadores = 0;
            for (int i = 0; i < numeroEquipos; i++) {
                Dictionary<string, string> equipo = new Dictionary<string, string>();
                equipo.Add("jugador1" , "Jugador_" + (nJugadores + 1));
                equipo.Add("jugador2" , "Jugador_" + (nJugadores + 2));
                equipo.Add("jugador3" , "Jugador_" + (nJugadores + 3));
                equipo.Add("nombreEquipo", "Equipo_" + (i + 1));
                equipo.Add("puntos" , "0");
                equipos.Add(equipo);
                nJugadores += 3;
            }
            GameLogic.jugador_negro = equipos[0]["jugador1"];
            GameLogic.jugador_blanco = equipos[1]["jugador1"];
            GameLogic.iniciarJuego();

            using(ReversiContext db = new ReversiContext()) {
                Campeonato campeonato = new Campeonato();
                campeonato.XmlRouteInfo = "";
                campeonato.Resultado = "enCurso";
                campeonato.UserId = GameLogic.userId;
                db.Campeonato.Add(campeonato);
                db.SaveChanges();
                GameLogic.championId = campeonato.ChampionId;
            }
        }



        public static void manage(bool imponerGanador = false, string ganador = "") {
            contadorPartidas++;
            estadosCampeonato = new Dictionary<string, bool> {
                {"ganadorCorrecto",false},
                {"hayEmpate", false},
                {"haTerminado", false},
            };
            if (imponerGanador == false) {
                if (GameLogic.player1Points > GameLogic.player2Points) {
                    equipos[0]["puntos"] = (Convert.ToInt32(equipos[0]["puntos"]) + 3).ToString();
                } else if (GameLogic.player2Points > GameLogic.player1Points) {
                    equipos[1]["puntos"] = (Convert.ToInt32(equipos[1]["puntos"]) + 3).ToString();
                } else {
                    equipos[0]["puntos"] = (Convert.ToInt32(equipos[0]["puntos"]) + 1).ToString();
                    equipos[1]["puntos"] = (Convert.ToInt32(equipos[1]["puntos"]) + 1).ToString();
                }
            } else {
                estadosCampeonato["ganadorCorrecto"] = true;
                if (ganador == equipos[0].Values.ToList()[contadorPartidas]) {
                    equipos[0]["puntos"] = (Convert.ToInt32(equipos[0]["puntos"]) + 3).ToString();
                } else if (ganador == equipos[1].Values.ToList()[contadorPartidas]) {
                    equipos[1]["puntos"] = (Convert.ToInt32(equipos[1]["puntos"]) + 3).ToString();
                }
            }

            Debug.WriteLine(equipos.Count());
            if (contadorPartidas == 3) {
                contadorPartidas = 0;
                if (Convert.ToInt32(equipos[0]["puntos"]) > Convert.ToInt32(equipos[1]["puntos"])) {
                    controlAuxiliar.Add(equipos[0]);
                } else if (Convert.ToInt32(equipos[1]["puntos"]) > Convert.ToInt32(equipos[0]["puntos"])) {
                    controlAuxiliar.Add(equipos[1]);
                } else {
                    controlAuxiliar.Add(equipos[0]);
                }
                equipos.RemoveAt(0);
                equipos.RemoveAt(0);
            }
                
            if (equipos.Count() == 0 && controlAuxiliar.Count() > 0) {
                foreach (Dictionary<string, string> equipo in controlAuxiliar) {
                    equipos.Add(equipo);
                }
                controlAuxiliar = new List<Dictionary<string, string>>();
            }

            GameLogic.reiniciarDatos();
            if (equipos.Count() == 1) {
                Debug.WriteLine("ha terminado");
                estadosCampeonato["haTerminado"] = true;
            } else {
                GameLogic.tipoPartida = "campeonato";
                GameLogic.iniciarJuego();
                GameLogic.jugador_negro = equipos[0].Values.ToList()[contadorPartidas];
                GameLogic.jugador_blanco = equipos[1].Values.ToList()[contadorPartidas];
            }

        } 
        




    }
}