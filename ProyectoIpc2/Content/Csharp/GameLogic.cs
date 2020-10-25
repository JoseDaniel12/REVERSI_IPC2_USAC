using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;
using ProyectoIpc2.Controllers;
using ProyectoIpc2.Models;
using WebGrease.Css.Ast.Selectors;

namespace ProyectoIpc2.Content.Csharp
{
    public static class GameLogic {
        public static int gameId = -1;
        public static int userId = -1;
        public static int hostColor = 1;
        public static string xmlRouteBoard = "";
        public static string tipoPartida = "";
        public static string jugador_negro = "Nombre";
        public static string jugador_blanco = "Nombre";
        public static int player1MovesNumber = 0;
        public static int player2MovesNumber = 0;
        public static int player1Points = 2;
        public static int player2Points = 2;
        public static int turno = 1;
        public static int tiempoSegP1 = 0;
        public static int tiempoSegP2 = 0;
        public static bool haTerminado = false;
        public static bool esModoInverso = false;
        public static string ganador = "";
        public static int anchoTablero = 8;
        public static int altoTablero = 8;
        public static List<List<String>> coloresElegidos = new List<List<String>>{
            new List<String> {"Negro"},
            new List<String> {"Blanco"},
        };
        public static List<String> coloresActuales = new List<String> {"Negro", "Blanco"};
        public static Dictionary<string, int> dicColores = new Dictionary<string, int>() {
            { "Negro", 1 },
            { "Blanco", 2 },
            { "Rojo", 3 },
            { "Amarillo", 4 },
            { "Azul", 5 },
            { "Anaranjado", 6 },
            { "Verde", 7 },
            { "Violeta", 8 },
            { "Celeste", 9 },
            { "Gris", 0 },

        };

        public static List<int[]> tirosPosibles = new List<int[]>();
        public static int[,] tableroInicial;
        public static int[,] tablero;
        public static int[,] tableroDeColores;

        public static void iniciarJuego() {
            if (tipoPartida == "vsPc" || tipoPartida == "vsJugador") {
                anchoTablero = 8;
                altoTablero = 8;
                tablero = new int[altoTablero, anchoTablero];
                limpiarTablero();
                tablero[(altoTablero / 2) - 1, (anchoTablero / 2) - 1] = 2;
                tablero[(altoTablero / 2) - 1, (anchoTablero / 2)] = 1;
                tablero[(altoTablero / 2), (anchoTablero / 2) - 1] = 1;
                tablero[(altoTablero / 2), (anchoTablero / 2)] = 2;
                tirosPosibles = actualizarTirosPosibles(1);
            } else if (tipoPartida == "vsPcXtreme" || tipoPartida == "vsJugadorXtreme") {
                tablero = new int[altoTablero, anchoTablero];
                limpiarTablero();

                tirosPosibles = new List<int[]>() {
                    new int[] { (anchoTablero / 2) - 1, (altoTablero / 2) - 1},
                    new int[] { (anchoTablero / 2), (altoTablero / 2) - 1},
                    new int[] { (anchoTablero / 2) - 1, (altoTablero / 2)},
                    new int[] {(anchoTablero / 2), (altoTablero / 2)},
                };
                esModoInverso = true;
            }
            tableroDeColores = (int[,])tablero.Clone();
            tableroInicial = (int[,])tablero.Clone();
        }

        public static void colocarFicha(int tiroX, int tiroY) {
            List<List<int[]>> caminosComidos = new List<List<int[]>>();
            foreach (int[] tiroPosible in tirosPosibles) {
                if (tiroPosible[0] == tiroX && tiroPosible[1] == tiroY && !isFinished()) {
                    //_____________________comidos a la derehca____________________________ 
                    int x = tiroX;
                    int y = tiroY;
                    List<int[]> caminoComido = new List<int[]>();
                    bool hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    for (x = (x + 1 < anchoTablero) ? x + 1 : x; x < anchoTablero; x++) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                    }

                    //_____________________comidos a la izquierda______________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    for (x = (x - 1 > -1) ? x - 1 : x; x > -1; x--) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                    }

                    //_____________________comidos hacia arriba______________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    for (y = (y - 1 > -1) ? y - 1 : y; y > -1; y--) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                    }

                    //_____________________comidos hacia abajo____________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    for (y = (y + 1 < altoTablero) ? y + 1 : y; y < altoTablero; y++) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                    }

                    //_____________________comidos diagonal derecha superior____________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    if (x + 1 != anchoTablero && x + 1 != -1 && y + 1 != altoTablero && y + 1 != -1) {
                        x++;
                        y++;
                    }
                    while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                        x++;
                        y++;
                    }


                    //_____________________comidos diagonal izquierda inferior____________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    if (x - 1 != anchoTablero && x - 1 != -1 && y - 1 != altoTablero && y - 1 != -1) {
                        x--;
                        y--;
                    }
                    while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                        x--;
                        y--;
                    }

                    //_____________________comidos diagonal izquierda superior____________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    if (x - 1 != anchoTablero && x - 1 != -1 && y + 1 != altoTablero && y + 1 != -1) {
                        x--;
                        y++;
                    }
                    while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                        x--;
                        y++;
                    }

                    //_____________________comidos diagonal derecha inferior____________________________ 
                    x = tiroX;
                    y = tiroY;
                    caminoComido = new List<int[]>();
                    hayContrarias = false;
                    caminoComido.Add(new int[] { tiroX, tiroY });
                    if (x + 1 != anchoTablero && x + 1 != -1 && y - 1 != altoTablero && y - 1 != -1) {
                        x++;
                        y--;
                    }
                    while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                        if (tablero[y, x] != turno && tablero[y, x] != -1) {
                            hayContrarias = true;
                            caminoComido.Add(new int[] { x, y });
                        } else if (tablero[y, x] == turno && hayContrarias == true) {
                            caminosComidos.Add(caminoComido);
                            break;
                        } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                            break;
                        }
                        x++;
                        y--;
                    }

                    //_______________________ fin de comidos___________________________________

                    foreach (List<int[]> camino in caminosComidos) {
                        foreach (int[] ficha in camino) {
                            tablero[ficha[1], ficha[0]] = turno;
                            tableroDeColores[ficha[1], ficha[0]] = dicColores[coloresActuales[turno - 1]];
                        }
                    }
                    calcularPutnos();

                    if (turno == 1) {
                        player1MovesNumber++;
                    } else {
                        player2MovesNumber++;
                    }


                    // Verifica si es aprtura personalizada de lo contrario continua normal
                    if ((tipoPartida == "vsPcXtreme" || tipoPartida == "vsJugadorXtreme") && (player1MovesNumber + player2MovesNumber) <= 4) {
                        foreach (int[] tiro in tirosPosibles) {
                            if (tiroX == tiro[0] && tiroY == tiro[1]) {
                                tablero[tiroY, tiroX] = turno;
                                tableroDeColores[tiroY, tiroX] = dicColores[coloresActuales[turno - 1]];
                                tirosPosibles.Remove(tiro);
                                turno = (turno == 1) ? 2 : 1;
                                tirosPosibles = (player1MovesNumber + player2MovesNumber == 4) ? actualizarTirosPosibles(turno):tirosPosibles;
                                break;
                            }
                        }
                    } else {
                        turno = (turno == 1) ? 2 : 1;
                        tirosPosibles = actualizarTirosPosibles(turno);
                        if (isFinished() == true) {
                            haTerminado = isFinished();
                            definirGanador();
                            guardarPartida();
                            break;
                        } else if (tirosPosibles.Count == 0 && actualizarTirosPosibles((turno == 1) ? 2 : 1).Count > 0) {
                            turno = (turno == 1) ? 2 : 1;
                            tirosPosibles = actualizarTirosPosibles(turno);
                        }
                    }

                    /*
                    Debug.WriteLine("Turno siguiente: " + ((turno == 1) ? "negro" : "blanco"));
                    foreach (int[] tiro in tirosPosibles) {
                        Debug.WriteLine(tiro[0] + ", " + tiro[1]);
                    }
                    Debug.WriteLine("___________________________");
                    */
                    cambiarColor(turno);
                    break;
                }
            }
        }

        public static List<int[]> actualizarTirosPosibles(int turno) {
            List<int[]> tirosPosibles = new List<int[]>();
            // recorrer casilla por casilla del tablero para encotrar las fichas del turno corresponiete
            for (int coordenadaY = 0; coordenadaY < altoTablero; coordenadaY++) {
                for (int coordenadaX = 0; coordenadaX < anchoTablero; coordenadaX++) {
                    if (tablero[coordenadaY, coordenadaX] == turno) {

                        // _________________________tiros a la derecha_____________________________________________
                        int x = coordenadaX;
                        int y = coordenadaY;
                        bool hayContrarias = false;
                        for (x= (x + 1 < anchoTablero)? x + 1: x; x < anchoTablero; x++) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                int[] arr = { x, y };
                                tirosPosibles.Add(arr);
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            } 
                        }

                        //___________________________ tiros por la izquierda__________________________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        for (x = (x - 1 > -1) ? x - 1 : x; x > -1; x--) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                int[] arr = { x, y };
                                tirosPosibles.Add(arr);
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                        }

                        // _____________________________tiros por arriba_________________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        for (y = (y - 1 > -1) ? y - 1 : y; y > -1; y--) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                int[] arr = { x, y };
                                tirosPosibles.Add(arr);
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                        }


                        //________________________tiros por abajo________________________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        for (y = (y + 1 < altoTablero) ? y + 1 : y; y < altoTablero; y++) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                int[] arr = { x, y };
                                tirosPosibles.Add(arr);
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                        }

                        //_____________________ tiros diagonal derecha supeiror__________________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        if (x + 1 != anchoTablero && x + 1 != -1 && y + 1 != altoTablero && y + 1 != -1) {
                            x++;
                            y++;
                        }
                        while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                tirosPosibles.Add(new int[] {x, y});
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                            x++;
                            y++;
                        }

                        // _____________________tiros diagonal izquierda superior _______________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        if (x - 1 != anchoTablero && x - 1 != -1 && y - 1 != altoTablero && y - 1 != -1) {
                            x--;
                            y--;
                        }
                        while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                tirosPosibles.Add(new int[] { x, y });
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                            x--;
                            y--;
                        }

                        //__________________ tiros diagonal derecha inferior ___________________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        if (x + 1 != anchoTablero && x + 1 != -1 && y - 1 != altoTablero && y - 1 != -1) {
                            x++;
                            y--;
                        }
                        while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                tirosPosibles.Add(new int[] { x, y });
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                            x++;
                            y--;
                        }

                        //________________tiros diagonal izquierda inferior________________________________
                        x = coordenadaX;
                        y = coordenadaY;
                        hayContrarias = false;
                        if (x - 1 != anchoTablero && x - 1 != -1 && y + 1 != altoTablero && y + 1 != -1) {
                            x--;
                            y++;
                        }
                        while (x != anchoTablero && x != -1 && y != altoTablero && y != -1) {
                            if (tablero[y, x] != turno && tablero[y, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, x] == -1 && hayContrarias == true) {
                                tirosPosibles.Add(new int[] { x, y });
                                break;
                            } else if (tablero[y, x] == turno || tablero[y, x] == -1) {
                                break;
                            }
                            x--;
                            y++;
                        }

                    }
                }
            }
            return tirosPosibles;
        }


        public static void guardarPartida() {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode tableroNode = xmlDoc.CreateElement("tablero");
            xmlDoc.AppendChild(tableroNode);

            XmlNode fichaNode;
            XmlNode colorNode;
            XmlNode columnaNode;
            XmlNode filaNode;
            
            for (int y = 0; y < altoTablero; y++) {
                for (int x = 0; x < anchoTablero; x++) {
                    fichaNode = xmlDoc.CreateElement("ficha");
                    colorNode = xmlDoc.CreateElement("color");
                    columnaNode = xmlDoc.CreateElement("columna");
                    filaNode = xmlDoc.CreateElement("fila");

                    // establecer la fila de la ficha
                    filaNode.InnerText = (y + 1).ToString();

                    // establecer el color de la ficha
                    switch (tablero[y,x]) {
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
            colorNode.InnerText = (GameLogic.turno == 1)? "negro": "blanco";
            siguienteTiroNode.AppendChild(colorNode);
            tableroNode.AppendChild(siguienteTiroNode);

            using (ReversiContext db = new ReversiContext()) {
                if (db.Partida.Find(gameId) == null) {
                    Partida partida = new Partida();
                    partida.GameType = tipoPartida;
                    partida.Player1MovesNumber = player1MovesNumber;
                    partida.Player2MovesNumber = player2MovesNumber;
                    partida.Player1Points = player1Points;
                    partida.Player2Points = player2Points;
                    partida.Player1 = jugador_negro;
                    partida.Player2 = jugador_blanco;
                    db.Partida.Add(partida);
                    db.SaveChanges();
                    partida.XmlRouteBoard = @"C:\Users\josed\Downloads\reversi_" + partida.GameId;
                    GameLogic.gameId = partida.GameId;
                    Encuentro encuentro = new Encuentro();
                    encuentro.GameId = partida.GameId;
                    encuentro.UserId = userId;
                    db.Encuentro.Add(encuentro);
                    db.SaveChanges();
                    xmlDoc.Save(@"C:\Users\josed\Downloads\reversi_" + partida.GameId);

                } else {
                    Partida partida = db.Partida.Find(gameId);
                    partida.GameType = tipoPartida;
                    partida.Player1MovesNumber = player1MovesNumber;
                    partida.Player2MovesNumber = player2MovesNumber;
                    partida.Player1Points = player1Points;
                    partida.Player2Points = player2Points;
                    partida.Player1 = jugador_negro;
                    partida.Player2 = jugador_blanco;
                    db.SaveChanges();
                    partida.XmlRouteBoard = @"C:\Users\josed\Downloads\reversi_" + partida.GameId;
                    xmlDoc.Save(@"C:\Users\josed\Downloads\reversi_" + partida.GameId);
                }
            }
        }


        public static bool cargarPartida(String rootFile) {
            XmlDocument xmlDoc = new XmlDocument();
            try {
                xmlDoc.Load(rootFile);
            } catch {
                return false;
            }
            xmlRouteBoard = rootFile;
            gameId = -1;
            limpiarTablero();
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
                            x = 7;  break;

                    }

                    switch (color) {
                        case "negro":
                            tablero[y, x] = 1; break;
                        case "blanco":
                            tablero[y, x] = 2; break;
                    }

                } else if (xmlNode.Name == "siguienteTiro") {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    GameLogic.turno = (color == "negro") ? 1 : 2;
                    tirosPosibles = actualizarTirosPosibles(GameLogic.turno);
                }


            }
            calcularPutnos();
            haTerminado = isFinished();
            using (ReversiContext db = new ReversiContext()) {
                foreach (Partida partida in db.Partida) {
                    if (partida.XmlRouteBoard == xmlRouteBoard) {
                        Encuentro encuentro = db.Encuentro.Find(partida.GameId);
                        if (userId == encuentro.UserId) {
                            gameId = partida.GameId;
                            player1MovesNumber = partida.Player1MovesNumber;
                            player2MovesNumber = partida.Player2MovesNumber;
                            if (tipoPartida == "vsJugador") {
                                jugador_negro = partida.Player1;
                                jugador_blanco = partida.Player2;
                                if (partida.Player1 == db.Usuario.Find(userId).UserName) {
                                    hostColor = 1;
                                } else {
                                    hostColor = 2;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static void limpiarTablero() {
            for (int y = 0; y < altoTablero; y++) {
                for (int x = 0; x < anchoTablero; x++) {
                    tablero[y, x] = -1;  
                }
            }
        }

        public static void calcularPutnos() {
            player1Points = 0;
            player2Points = 0;
            for (int y = 0; y < altoTablero; y++) {
                for (int x = 0; x < anchoTablero; x++) {
                    if (tablero[y,x] == 1) {
                        player1Points++;
                    } else if (tablero[y, x] == 2) {
                        player2Points++;
                    }
                }
            }
        }

        public static void reiniciarDatos() {
            gameId = -1;
            hostColor = 1;
            xmlRouteBoard = "";
            tipoPartida = "";
            jugador_negro = "Nombre";
            jugador_blanco = "Nombre";
            player1MovesNumber = 0;
            player2MovesNumber = 0;
            player1Points = 2;
            player2Points = 2;
            tiempoSegP1 = 0;
            tiempoSegP2 = 0;
            tirosPosibles = new List<int[]>();
            turno = 1;
            haTerminado = false;
            esModoInverso = false;
            for (int y = 0; y < altoTablero; y++) {
                for (int x = 0; x < anchoTablero; x++) {
                    tablero[y, x] = tableroInicial[y, x];
                }
            }

            coloresElegidos = new List<List<String>>{
                new List<String> {"Negro"},
                new List<String> {"Blanco"},
            };
            coloresActuales = new List<String> { "Negro", "Blanco" };

    }

        public static bool isFinished() {
            bool isfull = true;
            for (int y = 0; y < altoTablero; y++) {
                for (int x = 0; x < anchoTablero; x++) {
                    if (tablero[y,x] == -1) {
                        isfull = false;
                    }
                }
            }
            if (( (actualizarTirosPosibles(1).Count == 0 && actualizarTirosPosibles(2).Count == 0) && (player1MovesNumber + player2MovesNumber) > 4) || isfull) {
                return true;
            }
            return false;
        }

        public static void definirGanador() {
            if (!esModoInverso) {
                if (player1Points > player2Points) {
                    ganador = jugador_negro;
                } else if  (player2Points > player1Points) {
                    ganador = jugador_blanco;
                } else {
                    ganador = "EMPATE";
                }
            } else {
                if (player1Points < player2Points) {
                    ganador = jugador_negro;
                } else if (player2Points < player1Points) {
                    ganador = jugador_blanco;
                } else {
                    ganador = "EMPATE";
                }
            }
        }


        public static void cambiarColor(int turno) {
            string colorActual = coloresActuales[turno - 1];
            int indexColorActual = coloresElegidos[turno - 1].IndexOf(colorActual);
            if (indexColorActual != coloresElegidos[turno - 1].Count() - 1) {
                coloresActuales[turno - 1] = coloresElegidos[turno - 1][indexColorActual + 1];
            } else {
                coloresActuales[turno - 1] = coloresElegidos[turno - 1][0];
            }

        }




    }
}