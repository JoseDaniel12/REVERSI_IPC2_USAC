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
        public static string nombreCampeonato = "Campeonato";
        public static int numeroEquipos = 0;
        public static List<Dictionary<string, string>> equipos = new List<Dictionary<string, string>>();
        public static List<Dictionary<string, string>> controlAuxiliar = new List<Dictionary<string, string>>();
        public static List<Dictionary<string, string>> equiposRespaldo = new List<Dictionary<string, string>>();
        public static int contadorPartidas = 0;
        public static string resultado = "perdido";
        public static int earnPoints = 0;
        public static string hostUserName = "";


        public static Dictionary<string, bool> estadosCampeonato = new Dictionary<string, bool> {
            {"ganadorCorrecto",false},
            {"hayEmpate", false},
            {"haTerminado", false},
        };

        public static void iniciarCampeonato() {
            int nJugadores = 0;
            Random random = new Random();
            int numero = random.Next(0, numeroEquipos);
            for (int i = 0; i < numeroEquipos; i++) {
                Dictionary<string, string> equipo = new Dictionary<string, string>();
                if (i == numero) {
                    using (ReversiContext db = new ReversiContext()) {
                        equipo.Add("jugador1", db.Usuario.Find(GameLogic.userId).UserName);
                    }
                    equipo.Add("jugador2", "Jugador_" + (nJugadores + 2));
                    equipo.Add("jugador3", "Jugador_" + (nJugadores + 3));
                } else {
                    equipo.Add("jugador1", "Jugador_" + (nJugadores + 1));
                    equipo.Add("jugador2", "Jugador_" + (nJugadores + 2));
                    equipo.Add("jugador3", "Jugador_" + (nJugadores + 3));
                }
                equipo.Add("nombreEquipo", "Equipo_" + (i + 1));
                equipo.Add("puntos" , "0");
                equipos.Add(equipo);
                nJugadores += 3;
            }
            foreach (Dictionary<string, string> equipo in equipos) {
                equiposRespaldo.Add(equipo);
            }
            GameLogic.jugador_negro = equipos[0]["jugador1"];
            GameLogic.jugador_blanco = equipos[1]["jugador1"];
            GameLogic.iniciarJuego();

            using(ReversiContext db = new ReversiContext()) {
                hostUserName = db.Usuario.Find(GameLogic.userId).UserName.ToString();
                Campeonato campeonato = new Campeonato();
                Debug.WriteLine(nombreCampeonato);
                campeonato.ChampionName = nombreCampeonato;
                campeonato.Resultado = "perdido";
                campeonato.EarnPoints = 0;
                campeonato.UserId = GameLogic.userId;
                db.Campeonato.Add(campeonato);
                db.SaveChanges();
                GameLogic.championId = campeonato.ChampionId;

                foreach (Dictionary<string, string> team in equipos) {
                    Equipo equipo = new Equipo();
                    equipo.TeamName = team["nombreEquipo"];
                    equipo.Player1Name = team["jugador1"];
                    equipo.Player2Name = team["jugador2"];
                    equipo.Player3Name = team["jugador3"];
                    equipo.ChampionId = GameLogic.championId;
                    db.Equipo.Add(equipo);
                    db.SaveChanges();
                }
            }

        }

        public static void reiniciar() {
            nombreCampeonato = "Campeonato";
            numeroEquipos = 0;
            equipos = new List<Dictionary<string, string>>(); 
            equiposRespaldo = new List<Dictionary<string, string>>();
            controlAuxiliar = new List<Dictionary<string, string>>();
            contadorPartidas = 0;
            resultado = "perdido";
            earnPoints = 0;
            estadosCampeonato = new Dictionary<string, bool> {
                {"ganadorCorrecto",false},
                {"hayEmpate", false},
                {"haTerminado", false},

            };
        }


        public static void manage(bool imponerGanador = false, int ganadorIndex = -1, int equipoIndex = -1 ) {

            if (estadosCampeonato["hayEmpate"] == false) {
                contadorPartidas++;
            }
            estadosCampeonato = new Dictionary<string, bool> {
                {"ganadorCorrecto",false},
                {"hayEmpate", false},
                {"haTerminado", false},
            };
            if (imponerGanador == false) {
                if (GameLogic.player1Points > GameLogic.player2Points) {
                    equipos[0]["puntos"] = (Convert.ToInt32(equipos[0]["puntos"]) + 3).ToString();
                    using (ReversiContext db = new ReversiContext()) {
                        earnPoints = (GameLogic.jugador_negro == db.Usuario.Find(GameLogic.userId).UserName) ? earnPoints + 3 : earnPoints;
                    }
                } else if (GameLogic.player2Points > GameLogic.player1Points) {
                    equipos[1]["puntos"] = (Convert.ToInt32(equipos[1]["puntos"]) + 3).ToString();
                    using (ReversiContext db = new ReversiContext()) {
                        earnPoints = (GameLogic.jugador_blanco == db.Usuario.Find(GameLogic.userId).UserName) ? earnPoints + 3 : earnPoints;
                    }
                } else {
                    equipos[0]["puntos"] = (Convert.ToInt32(equipos[0]["puntos"]) + 1).ToString();
                    equipos[1]["puntos"] = (Convert.ToInt32(equipos[1]["puntos"]) + 1).ToString();
                    using (ReversiContext db = new ReversiContext()) {
                        if (equipos[0]["jugador1"] == db.Usuario.Find(GameLogic.userId).UserName) {
                           earnPoints += 1;
                        } else if (equipos[0]["jugador2"] == db.Usuario.Find(GameLogic.userId).UserName) {
                            earnPoints += 1;
                        } else if (equipos[0]["jugador3"] == db.Usuario.Find(GameLogic.userId).UserName) {
                            earnPoints += 1;
                        } else if (equipos[1]["jugador1"] == db.Usuario.Find(GameLogic.userId).UserName) {
                            earnPoints += 1;
                        } else if (equipos[1]["jugador2"] == db.Usuario.Find(GameLogic.userId).UserName) {
                            earnPoints += 1;
                        } else if (equipos[1]["jugador3"] == db.Usuario.Find(GameLogic.userId).UserName) {
                            earnPoints += 1;
                        }
                    }
                }
            } else {
                estadosCampeonato["ganadorCorrecto"] = true;
                equipos[equipoIndex]["puntos"] = (Convert.ToInt32(equipos[equipoIndex]["puntos"]) + 3).ToString();
                using (ReversiContext db = new ReversiContext()) {
                    if (equipos[equipoIndex]["jugador1"] == db.Usuario.Find(GameLogic.userId).UserName) {
                        earnPoints += 3;
                    } else if (equipos[equipoIndex]["jugador2"] == db.Usuario.Find(GameLogic.userId).UserName) {
                        earnPoints += 3;
                    } else if (equipos[equipoIndex]["jugador3"] == db.Usuario.Find(GameLogic.userId).UserName) {
                        earnPoints += 3;
                    }
                }

            }


            if (contadorPartidas >= 3) {
                if (Convert.ToInt32(equipos[0]["puntos"]) > Convert.ToInt32(equipos[1]["puntos"])) {
                    equipos[0]["puntos"] = "0";
                    controlAuxiliar.Add(equipos[0]);
                    equipos.RemoveAt(0);
                    equipos.RemoveAt(0);
                    contadorPartidas = 0;
                } else if (Convert.ToInt32(equipos[1]["puntos"]) > Convert.ToInt32(equipos[0]["puntos"])) {
                    equipos[1]["puntos"] = "0";
                    controlAuxiliar.Add(equipos[1]);
                    equipos.RemoveAt(0);
                    equipos.RemoveAt(0);
                    contadorPartidas = 0;
                } else {
                    estadosCampeonato["hayEmpate"] = true;
                }

            }
                
            if (equipos.Count() == 0 && controlAuxiliar.Count() > 0) {
                foreach (Dictionary<string, string> equipo in controlAuxiliar) {
                    equipos.Add(equipo);
                }
                controlAuxiliar = new List<Dictionary<string, string>>();
            }

            GameLogic.reiniciarDatos();
            if (equipos.Count() == 1) {
                estadosCampeonato["haTerminado"] = true;
                using (ReversiContext db = new ReversiContext()) {
                    Campeonato campeonato = db.Campeonato.Find(GameLogic.championId);
                    campeonato.ChampionName = (nombreCampeonato == "") ? "Campeonato_" + GameLogic.championId : campeonato.ChampionName;
                    if (equipos[0]["jugador1"] == hostUserName || equipos[0]["jugador2"] == hostUserName || equipos[0]["jugador3"] == hostUserName) {
                        campeonato.Resultado = "ganado";
                    }
                    campeonato.EarnPoints = earnPoints;
                    db.SaveChanges();
                } 
            } else if (estadosCampeonato["hayEmpate"] == false) {
                GameLogic.tipoPartida = "campeonato";
                GameLogic.iniciarJuego();
                GameLogic.jugador_negro = equipos[0].Values.ToList()[contadorPartidas];
                GameLogic.jugador_blanco = equipos[1].Values.ToList()[contadorPartidas];
            }

        } 
        




    }
}