using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;
using ProyectoIpc2.Models;

namespace ProyectoIpc2.Content.Csharp {
    public static class SaveGame {

        public static XmlDocument guardarPartidaNormal(int gameId) {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode tableroNode = xmlDoc.CreateElement("tablero");

            XmlNode fichaNode;
            XmlNode colorNode;
            XmlNode columnaNode;
            XmlNode filaNode;
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    fichaNode = xmlDoc.CreateElement("ficha");
                    colorNode = xmlDoc.CreateElement("color");
                    columnaNode = xmlDoc.CreateElement("columna");
                    filaNode = xmlDoc.CreateElement("fila");

                    // establecer la fila de la ficha
                    filaNode.InnerText = (y + 1).ToString();

                    // establecer el color de la ficha
                    switch (GameLogic.tablero[y, x]) {
                        case 1:
                            colorNode.InnerText = "negro"; break;
                        case 2:
                            colorNode.InnerText = "blanco"; break;
                    }

                    // establecer la columna de la ficha
                    switch (x) {
                        case 0:
                            columnaNode.InnerText = "A"; break;
                        case 1:
                            columnaNode.InnerText = "B"; break;
                        case 2:
                            columnaNode.InnerText = "C"; break;
                        case 3:
                            columnaNode.InnerText = "D"; break;
                        case 4:
                            columnaNode.InnerText = "E"; break;
                        case 5:
                            columnaNode.InnerText = "F"; break;
                        case 6:
                            columnaNode.InnerText = "G"; break;
                        case 7:
                            columnaNode.InnerText = "H"; break;
                    }

                    if (colorNode.InnerText != "") {
                        fichaNode.AppendChild(colorNode);
                        fichaNode.AppendChild(columnaNode);
                        fichaNode.AppendChild(filaNode);
                        tableroNode.AppendChild(fichaNode);
                    }
                }
                
            }
            XmlNode siguienteTiroNode = xmlDoc.CreateElement("siguienteTiro");
            colorNode = xmlDoc.CreateElement("color");
            colorNode.InnerText = (GameLogic.turno == 1) ? "negro" : "blanco";
            siguienteTiroNode.AppendChild(colorNode);
            tableroNode.AppendChild(siguienteTiroNode);
            xmlDoc.AppendChild(tableroNode);
            return xmlDoc;
        }

        public static XmlDocument guardarPartidaXtreme(int gameId) {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode partidaNode = xmlDoc.CreateElement("partida");
            XmlNode filasNode = xmlDoc.CreateElement("filas");
            XmlNode columnasNode = xmlDoc.CreateElement("columnas");
            XmlNode jugador1Node = xmlDoc.CreateElement("Jugador1");
            XmlNode jugador2Node = xmlDoc.CreateElement("Jugador2");
            XmlNode modalidadNode = xmlDoc.CreateElement("Modalidad");
            XmlNode colorNode;
            filasNode.InnerText = GameLogic.altoTablero.ToString();
            columnasNode.InnerText = GameLogic.anchoTablero.ToString();
            foreach (String color in GameLogic.coloresElegidos[0]) {
                colorNode = xmlDoc.CreateElement("color");
                colorNode.InnerText = color.ToLower();
                jugador1Node.AppendChild(colorNode);
            }
            foreach (String color in GameLogic.coloresElegidos[1]) {
                colorNode = xmlDoc.CreateElement("color");
                colorNode.InnerText = color.ToLower();
                jugador2Node.AppendChild(colorNode);
            }
            modalidadNode.InnerText = (GameLogic.esModoInverso) ? "Inversa" : "Normal";
            partidaNode.AppendChild(filasNode);
            partidaNode.AppendChild(columnasNode);
            partidaNode.AppendChild(jugador1Node);
            partidaNode.AppendChild(jugador2Node);
            partidaNode.AppendChild(modalidadNode);

            XmlNode tableroNode = xmlDoc.CreateElement("tablero");
            string[] abcdario = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "V"};
            string[] colores = new string[] {"gris", "negro", "blanco", "rojo", "amarillo", "azul", "anaranjado", "verde", "violeta", "celeste" };
            for (int y = 0; y < GameLogic.altoTablero; y++) {
                for (int x = 0; x < GameLogic.anchoTablero; x++) {
                    XmlNode fichaNode = xmlDoc.CreateElement("ficha");
                    colorNode = xmlDoc.CreateElement("color");
                    XmlNode columnaNode = xmlDoc.CreateElement("columna");
                    XmlNode filaNode = xmlDoc.CreateElement("fila");
                    columnaNode.InnerText = abcdario[x];
                    filaNode.InnerText = (y+1).ToString();
                    colorNode.InnerText = (GameLogic.tableroDeColores[y, x] != -1) ? colores[GameLogic.tableroDeColores[y, x]] : "";
                    fichaNode.AppendChild(colorNode);
                    fichaNode.AppendChild(columnaNode);
                    fichaNode.AppendChild(filaNode);
                    if (colorNode.InnerText != "") {
                        tableroNode.AppendChild(fichaNode);
                    }
                }
            }
            XmlNode siguienteTiroNode = xmlDoc.CreateElement("siguienteTiro");
            colorNode = xmlDoc.CreateElement("color");
            colorNode.InnerText = GameLogic.coloresActuales[GameLogic.turno -1].ToLower();
            siguienteTiroNode.AppendChild(colorNode);
            partidaNode.AppendChild(filasNode);
            partidaNode.AppendChild(columnasNode);
            partidaNode.AppendChild(jugador1Node);
            partidaNode.AppendChild(jugador2Node);
            partidaNode.AppendChild(modalidadNode);
            tableroNode.AppendChild(siguienteTiroNode);
            partidaNode.AppendChild(tableroNode);
            xmlDoc.AppendChild(partidaNode);
            return xmlDoc;
        }



        public static void guardar(int gameId) {
            using (ReversiContext db = new ReversiContext()) {
                Partida partida = (db.Partida.Find(gameId) != null)? db.Partida.Find(gameId) : new Partida();
                partida.GameType = GameLogic.tipoPartida;
                partida.GameMode = (GameLogic.esModoInverso) ? "Inversa" : "Normal";
                partida.XmlRouteBoard = GameLogic.xmlRouteBoard;
                partida.Player1MovesNumber = GameLogic.player1MovesNumber;
                partida.Player2MovesNumber = GameLogic.player2MovesNumber;
                partida.Player1Points = GameLogic.player1Points;
                partida.Player2Points = GameLogic.player2Points;
                partida.Player1Time = GameLogic.tiempoSegP1;
                partida.Player2Time = GameLogic.tiempoSegP2;
                partida.Resultado = GameLogic.resultado;
                partida.HostColor = (GameLogic.tipoPartida == "vsPc" || GameLogic.tipoPartida == "vsJugador") ? GameLogic.hostColor : partida.HostColor;
                partida.ChampionId = (GameLogic.tipoPartida == "campeonato") ? GameLogic.championId : partida.ChampionId;
                partida.UserId = GameLogic.userId;
                if (db.Partida.Find(gameId) == null) {
                    db.Partida.Add(partida);
                }
                db.SaveChanges();
                GameLogic.gameId = partida.GameId;
                XmlDocument xmlDoc;
                if (GameLogic.tipoPartida == "vsJugador" || GameLogic.tipoPartida == "vsPc") {
                    GameLogic.xmlRouteBoard = @"C:\Users\josed\Downloads\Reversi_" + partida.GameId + ".xml";
                    xmlDoc = guardarPartidaNormal(gameId);
                    xmlDoc.Save(GameLogic.xmlRouteBoard);
                } else if (GameLogic.tipoPartida == "vsJugadorXtreme" || GameLogic.tipoPartida == "vsPcXtreme") {
                    GameLogic.xmlRouteBoard = @"C:\Users\josed\Downloads\Reversi_Xtreme_" + partida.GameId + ".xml";
                    xmlDoc = guardarPartidaXtreme(gameId);
                    xmlDoc.Save(GameLogic.xmlRouteBoard);
                } else if (GameLogic.tipoPartida == "campeonato") {
                    GameLogic.xmlRouteBoard = @"C:\Users\josed\Downloads\Reversi_" + partida.GameId + ".xml";
                    xmlDoc = guardarPartidaNormal(gameId);
                    xmlDoc.Save(GameLogic.xmlRouteBoard);
                }
                partida.XmlRouteBoard = GameLogic.xmlRouteBoard;
                db.SaveChanges();
            }
        }



    }
}