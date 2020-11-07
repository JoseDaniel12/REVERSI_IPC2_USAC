using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml;
using ProyectoIpc2.Models;

namespace ProyectoIpc2.Content.Csharp {
    public class LoadGame {
        public static bool cargarPartidaNormal(String rootFile) {
            XmlDocument xmlDoc = new XmlDocument();
            try {
                xmlDoc.Load(rootFile);
            } catch {
                return false;
            }
            GameLogic.xmlRouteBoard = rootFile;
            GameLogic.gameId = -1;
            GameLogic.limpiarTablero();
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes) {
                if (xmlNode.Name == "ficha") {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    int y = Int32.Parse(xmlNode.ChildNodes[2].InnerText) - 1;
                    int x = 0;
                    switch (xmlNode.ChildNodes[1].InnerText) {
                        case "A":
                            x = 0; break;
                        case "B":
                            x = 1; break;
                        case "C":
                            x = 2; break;
                        case "D":
                            x = 3; break;
                        case "E":
                            x = 4; break;
                        case "F":
                            x = 5; break;
                        case "G":
                            x = 6; break;
                        case "H":
                            x = 7; break;

                    }

                    switch (color) {
                        case "negro":
                            GameLogic.tablero[y, x] = 1;
                            GameLogic.tableroDeColores[y, x] = 1;
                                break;
                        case "blanco":
                            GameLogic.tablero[y, x] = 2;
                            GameLogic.tableroDeColores[y, x] = 2;
                            break;
                    }

                } else if (xmlNode.Name == "siguienteTiro") {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    GameLogic.turno = (color == "negro") ? 1 : 2;
                    GameLogic.tirosPosibles = GameLogic.actualizarTirosPosibles(GameLogic.turno);
                }


            }
            GameLogic.calcularPutnos();
            GameLogic.haTerminado = GameLogic.isFinished();
            using (ReversiContext db = new ReversiContext()) {
                foreach (Partida partida in db.Partida) {
                    if (partida.XmlRouteBoard == rootFile) {
                        if (partida.UserId == GameLogic.userId) {
                            GameLogic.gameId = partida.GameId;
                            GameLogic.esModoInverso = false;
                            GameLogic.player1MovesNumber = partida.Player1MovesNumber;
                            GameLogic.player2MovesNumber = partida.Player2MovesNumber;
                            GameLogic.tiempoSegP1 = partida.Player1Time;
                            GameLogic.tiempoSegP2 = partida.Player1Time;
                            if (GameLogic.tipoPartida == "vsJugador") {
                                GameLogic.hostColor = partida.HostColor;
                                GameLogic.jugador_negro = (partida.HostColor == 1) ? db.Usuario.Find(partida.UserId).UserName.ToString() : "Invitado";
                                GameLogic.jugador_blanco = (partida.HostColor == 2) ? db.Usuario.Find(partida.UserId).UserName.ToString() : "Invitado";
                            }
                        }
                    }
                }
            }
            return true;
        }


        public static bool cargarPartidaXtreme(String rootFile) {
            XmlDocument xmlDoc = new XmlDocument();
            try {
                xmlDoc.Load(rootFile);
            }
            catch {
                return false;
            }
            GameLogic.xmlRouteBoard = rootFile;
            GameLogic.gameId = -1;
            GameLogic.limpiarTablero();
            string[] abcdario = new string[] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t","v"};
            string[] colores = new string[] { "gris", "negro", "blanco", "rojo", "amarillo", "azul", "anaranjado", "verde", "violeta", "celeste" };
            List<String> colores_p1 = new List<String>();
            List<String> colores_p2 = new List<String>();
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes) { 
                if (xmlNode.Name.ToLower() == "filas") {
                    GameLogic.altoTablero = Int32.Parse(xmlNode.InnerText);
                } else if (xmlNode.Name.ToLower() == "columnas") {
                    GameLogic.anchoTablero = Int32.Parse(xmlNode.InnerText);
                    GameLogic.iniciarJuego();
                } else if (xmlNode.Name.ToLower() == "jugador1") {
                    foreach (XmlNode subXmlNode in xmlNode.ChildNodes) {
                        colores_p1.Add(subXmlNode.InnerText.ToLower());
                    }
                } else if (xmlNode.Name.ToLower() == "jugador2") {
                    foreach (XmlNode subXmlNode in xmlNode.ChildNodes) {
                        Debug.WriteLine(subXmlNode.InnerText.ToLower());
                        colores_p2.Add(subXmlNode.InnerText.ToLower());
                    }
                    GameLogic.coloresElegidos = new List<List<string>> { colores_p1, colores_p2 };
                    GameLogic.coloresActuales = new List<string> { colores_p1[0], colores_p2[0] };
                } else if (xmlNode.Name.ToLower() == "modalidad") {
                    GameLogic.esModoInverso = (xmlNode.InnerText.ToLower() == "normal") ? false : true;
                } else if (xmlNode.Name.ToLower() == "tablero") {
                    foreach (XmlNode subXmlNode in xmlNode.ChildNodes) {
                        if (subXmlNode.Name.ToLower() == "ficha") {
                            string color = subXmlNode.ChildNodes[0].InnerText.ToLower();
                            int y = Int32.Parse(subXmlNode.ChildNodes[2].InnerText) - 1;
                            int x = Array.IndexOf(abcdario, subXmlNode.ChildNodes[1].InnerText.ToLower());
                            if (x >= 0 && x <= GameLogic.anchoTablero-1 && y >= 0 && y <= GameLogic.altoTablero-1) {
                                GameLogic.tableroDeColores[y, x] = (colores.Contains(color)) ? Array.IndexOf(colores, color) : -1;
                                GameLogic.tablero[y, x] = (colores_p1.Contains(color)) ? 1 : 2;
                            } 
                        } else if (subXmlNode.Name.ToLower() == "siguientetiro") {
                            string color = subXmlNode.ChildNodes[0].InnerText.ToLower();
                            GameLogic.turno = (colores_p1.Contains(color)) ? 1 : 2;
                            GameLogic.coloresActuales[GameLogic.turno - 1] = color;
                            GameLogic.tirosPosibles = GameLogic.actualizarTirosPosibles(GameLogic.turno);
                        }
                    }
                    if (GameLogic.contarFichas() < 4) {
                        int anchoTablero = GameLogic.anchoTablero;
                        int altoTablero = GameLogic.altoTablero;
                        if (GameLogic.tablero[(anchoTablero / 2) - 1, (altoTablero / 2) - 1] == -1) {
                            GameLogic.tirosPosibles.Add(new int[] { (anchoTablero / 2) - 1, (altoTablero / 2) - 1 });
                        } 
                        if (GameLogic.tablero[(anchoTablero / 2), (altoTablero / 2) - 1] == -1) {
                            GameLogic.tirosPosibles.Add(new int[] { (anchoTablero / 2), (altoTablero / 2) - 1 });
                        }
                        if (GameLogic.tablero[(anchoTablero / 2) - 1, (altoTablero / 2)] == -1) {
                            GameLogic.tirosPosibles.Add(new int[] { (anchoTablero / 2) - 1, (altoTablero / 2) });
                        }
                        if (GameLogic.tablero[(anchoTablero / 2), (altoTablero / 2)] == -1) {
                            GameLogic.tirosPosibles.Add(new int[] { (anchoTablero / 2), (altoTablero / 2) });
                        }
                    } else {
                        GameLogic.actualizarTirosPosibles(GameLogic.turno);
                    }
                } 
            }
            GameLogic.calcularPutnos();
            GameLogic.haTerminado = GameLogic.isFinished();
            using (ReversiContext db = new ReversiContext()) {
                foreach (Partida partida in db.Partida) {
                    if (partida.XmlRouteBoard == rootFile) {
                        if (partida.UserId == GameLogic.userId) {
                            GameLogic.gameId = partida.GameId;
                            GameLogic.player1MovesNumber = partida.Player1MovesNumber;
                            GameLogic.player2MovesNumber = partida.Player2MovesNumber;
                            GameLogic.tiempoSegP1 = partida.Player1Time;
                            GameLogic.tiempoSegP2 = partida.Player1Time;
                            GameLogic.hostColor = partida.HostColor;
                            if (GameLogic.tipoPartida == "vsJugadorXtreme") {
                                GameLogic.jugador_negro = (partida.HostColor == 1) ? db.Usuario.Find(partida.UserId).UserName.ToString() : "Invitado";
                                GameLogic.jugador_blanco = (partida.HostColor == 2) ? db.Usuario.Find(partida.UserId).UserName.ToString() : "Invitado";
                            } else if (GameLogic.tipoPartida == "vsPc") {
                                GameLogic.jugador_negro = (partida.HostColor == 1) ? db.Usuario.Find(partida.UserId).UserName.ToString() : "PC";
                                GameLogic.jugador_blanco = (partida.HostColor == 2) ? db.Usuario.Find(partida.UserId).UserName.ToString() : "PC";
                            }

                            if (GameLogic.isFinished()) {
                                if (partida.Player1Points > partida.Player2Points) {
                                    GameLogic.ganador = GameLogic.jugador_negro;
                                } else if (partida.Player2Points > partida.Player1Points) {
                                    GameLogic.ganador = GameLogic.jugador_blanco;
                                } else {
                                    GameLogic.ganador = "EMPATE";
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static bool cargarCampeonato(String rootFile) {
            XmlDocument xmlDoc = new XmlDocument();
            try {
                xmlDoc.Load(rootFile);
            }
            catch {
                return false;
            }
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes) {
                if (xmlNode.Name.ToLower() == "nombre") {
                    ChampionshipManager.nombreCampeonato = xmlNode.InnerText;
                } else if (xmlNode.Name.ToLower() == "equipo") {
                    Dictionary<string, string> equipo = new Dictionary<string, string>();
                    equipo.Add("jugador1", xmlNode.ChildNodes[1].InnerText);
                    equipo.Add("jugador2", xmlNode.ChildNodes[2].InnerText);
                    equipo.Add("jugador3", xmlNode.ChildNodes[3].InnerText);
                    equipo.Add("nombreEquipo", xmlNode.ChildNodes[0].InnerText);
                    equipo.Add("puntos", "0");
                    ChampionshipManager.equipos.Add(equipo);
                    ChampionshipManager.equiposRespaldo.Add(equipo);
                }
            }
            GameLogic.jugador_negro = ChampionshipManager.equipos[0]["jugador1"];
            GameLogic.jugador_blanco = ChampionshipManager.equipos[1]["jugador1"];
            GameLogic.iniciarJuego();
            using (ReversiContext db = new ReversiContext()) {
                ChampionshipManager.hostUserName = db.Usuario.Find(GameLogic.userId).UserName.ToString();
                Campeonato campeonato = new Campeonato();
                campeonato.ChampionName = ChampionshipManager.nombreCampeonato;
                campeonato.Resultado = "enCurso";
                campeonato.EarnPoints = 0;
                campeonato.UserId = GameLogic.userId;
                db.Campeonato.Add(campeonato);
                db.SaveChanges();
                GameLogic.championId = campeonato.ChampionId;

                foreach (Dictionary<string, string> team in ChampionshipManager.equipos) {
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
            return true;
        }


        public static void cargar(String rootFile) {
            if (GameLogic.tipoPartida == "vsJugador" || GameLogic.tipoPartida == "vsPc") {
                cargarPartidaNormal(rootFile);
            } else if (GameLogic.tipoPartida == "vsJugadorXtreme" || GameLogic.tipoPartida == "vsPcXtreme") {
                cargarPartidaXtreme(rootFile);
            } else if (GameLogic.tipoPartida == "campeonato") {
                cargarCampeonato(rootFile);
            }
        }


    }
}