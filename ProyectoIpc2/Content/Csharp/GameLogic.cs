using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml;
using ProyectoIpc2.Controllers;

namespace ProyectoIpc2.Content.Csharp
{
    public static class GameLogic {
        public static int userId = -1;
        public static string xmlRouteBoard = "";
        public static string tipoPartida = "";
        public static string jugador_negro = "Nombre";
        public static string jugador_blanco = "Nombre";
        public static int player1MovesNumber = 0;
        public static int player2MovesNumber = 0;
        public static int player1Points = 2;
        public static int player2Points = 2;
        public static int turno = 1;
        public static List<int[]> tirosPosibles = new List<int[]>() { 
            new int[] {3,2},
            new int[] {2,3},
            new int[] {5,4},
            new int[] {4,5},
        };

         public static int[,] tableroInicial = new int[8, 8] {
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1,  2,  1, -1, -1, -1},
            {-1, -1, -1,  1,  2, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
        };

        public static int[,] tablero = new int[8, 8] {
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1,  2,  1, -1, -1, -1},
            {-1, -1, -1,  1,  2, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
        };

        static void Main(String[] args) {
        }

        public static void colocarFicha(int tiroX, int tiroY) {
            List<List<int[]>> caminosComidos = new List<List<int[]>>();
            List<int[]> caminoComido = new List<int[]>();
            foreach (int[] tiroPosible in tirosPosibles) {
                if (tiroPosible[0] == tiroX && tiroPosible[1] == tiroY) {
                    int x = tiroX;
                    int y = tiroY;
                    bool hayContrarias = false;
                    //comidos a la derehca 
                    for (x = tiroX; x < 8; x++) {
                        if (tablero[tiroY, x] != turno) {
                            caminoComido.Add(new int[] { x, tiroY });
                            hayContrarias = (tablero[tiroY, x] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos a la izquierda 
                    for (x = tiroX; x > -1; x--) {
                        if (tablero[tiroY, x] != turno) {
                            caminoComido.Add(new int[] { x,tiroY });
                            hayContrarias = (tablero[tiroY, x] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos por arriba
                    for (y = tiroY ; y > -1; y--) {
                        if (tablero[y, tiroX] != turno) {
                            caminoComido.Add(new int[] { tiroX, y });
                            hayContrarias = (tablero[y, tiroX] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos por abajo
                    for (y = tiroY; y < 8; y++) {
                        if (tablero[y, tiroX] != turno) {
                            caminoComido.Add(new int[] { tiroX, y });
                            hayContrarias = (tablero[y, tiroX] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos diagonal derecha superior
                    x = tiroX;
                    y = tiroY;
                    while (x != 8 && x != -1 && y != 8 && y != -1) {
                        if (tablero[y, x] != turno) {
                            caminoComido.Add(new int[] { x, y });
                            hayContrarias = (tablero[y, x] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                        x++;
                        y++;
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos diagonal izquierda superior
                    x = tiroX;
                    y = tiroY;
                    while (x != 8 && x != -1 && y != 8 && y != -1) {
                        if (tablero[y, x] != turno) {
                            caminoComido.Add(new int[] {x, y});
                            hayContrarias = (tablero[y, x] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                        x--;
                        y++;
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos diagonal izquierda inferior
                    x = tiroX;
                    y = tiroY;
                    while (x != 8 && x != -1 && y != 8 && y != -1) {
                        if (tablero[y, x] != turno) {
                            caminoComido.Add(new int[] { x, y });
                            hayContrarias = (tablero[y, x] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            Debug.WriteLine("entro");
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                        x--;
                        y--;
                    }
                    hayContrarias = false;
                    caminoComido = new List<int[]>();
                    //comidos diagonal derecha inferior
                    x = tiroX;
                    y = tiroY;
                    while (x != 8 && x != -1 && y != 8 && y != -1) {
                        if (tablero[y, x] != turno) {
                            caminoComido.Add(new int[] { x, y });
                            hayContrarias = (tablero[y, x] != -1) ? true : hayContrarias;
                        } else if (hayContrarias) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                        x++;
                        y--;
                    }

                    Debug.WriteLine(turno);
                    foreach (List<int[]> camino in caminosComidos) {
                        foreach (int[] ficha in camino) {
                            tablero[ficha[1], ficha[0]] = turno;
                        }
                    }

                    if (turno == 1) {
                        player1MovesNumber++;
                    } else {
                        player2MovesNumber++;
                    }
                    calcularPutnos();

                    turno = (turno == 1) ? 2 : 1;
                    actualizarTirosPosibles();

                    string texto = (turno == 1) ? "Negro" : "Blanco";
                    Debug.WriteLine("Turno siguiente: " + texto);
                    foreach (int[] tiro in tirosPosibles) {
                        Debug.WriteLine(tiro[0] + ", " + tiro[1]);
                    }
                    Debug.WriteLine("________________________");
                    break;
                }
            }

        }

        public static void actualizarTirosPosibles() {
            tirosPosibles = new List<int[]>();
            bool hayContrarias = false;
            // recorrer casilla por casilla del tablero para encotrar las fichas del turno corresponiete
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    if (tablero[y, x] == turno) {
                        // tiros a la derecha
                        int coordenadaX = x;
                        int coordenadaY = y;
                        for (coordenadaX = x; coordenadaX < 8; coordenadaX++) {
                            if (tablero[y, coordenadaX] != turno && tablero[y, coordenadaX] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, coordenadaX] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, y };
                                tirosPosibles.Add(arr);
                                break;
                            }
                        }
                        hayContrarias = false;
                        // tiros por la izquierda
                        coordenadaX = x;
                        coordenadaY = y;
                        for (coordenadaX = x; coordenadaX > -1; coordenadaX--) {
                            if (tablero[y, coordenadaX] != turno && tablero[y, coordenadaX] != -1) {
                                hayContrarias = true;
                            } else if (tablero[y, coordenadaX] == -1 && hayContrarias == true) {
                                int[] arr = {coordenadaX, y};
                                tirosPosibles.Add(arr);
                                break;
                            }
                        }
                        hayContrarias = false;
                        // tiros por arriba
                        coordenadaX = x;
                        coordenadaY = y;
                        for (coordenadaY = y; coordenadaY > -1; coordenadaY--) {
                            if (tablero[coordenadaY, x] != turno && tablero[coordenadaY, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaY, x] == -1 && hayContrarias == true) {
                                int[] arr = {x, coordenadaY};
                                tirosPosibles.Add(arr);
                                break;
                            }
                        }
                        hayContrarias = false;
                        // tiros por abajo
                        coordenadaX = x;
                        coordenadaY = y;
                        for (coordenadaY = y; coordenadaY < 8; coordenadaY++) {
                            if (tablero[coordenadaY, x] != turno && tablero[coordenadaY, x] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaY, x] == -1 && hayContrarias == true) {
                                int[] arr = {x, coordenadaY };
                                tirosPosibles.Add(arr);
                                break;
                            }
                        }
                        hayContrarias = false;
                        // tiros diagonal derecha supeiror 
                        coordenadaX = x;
                        coordenadaY = y;
                        while (coordenadaX != 8 && coordenadaX != -1 && coordenadaY != 8 && coordenadaY != -1) {
                            if (tablero[coordenadaY, coordenadaX] != turno && tablero[coordenadaY, coordenadaX] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaY, coordenadaX] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, coordenadaY };
                                tirosPosibles.Add(arr);
                                break;
                            }
                            coordenadaX++;
                            coordenadaY++;
                        }
                        hayContrarias = false;
                        // tiros diagonal izquierda superior 
                        coordenadaX = x;
                        coordenadaY = y;
                        while (coordenadaX != 8 && coordenadaX != -1 && coordenadaY != 8 && coordenadaY != -1) {
                            if (tablero[coordenadaY, coordenadaX] != turno && tablero[coordenadaY, coordenadaX] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaY, coordenadaX] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, coordenadaY};
                                tirosPosibles.Add(arr);
                                break;
                            }
                            coordenadaX--;
                            coordenadaY++;
                        }
                        hayContrarias = false;
                        // tiros diagonal derecha inferior 
                        coordenadaX = x;
                        coordenadaY = y;
                        while (coordenadaX != 8 && coordenadaX != -1 && coordenadaY != 8 && coordenadaY != -1) {
                            if (tablero[coordenadaY, coordenadaX] != turno && tablero[coordenadaY, coordenadaX] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaY, coordenadaX] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, coordenadaY};
                                tirosPosibles.Add(arr);
                                break;
                            }
                            coordenadaX++;
                            coordenadaY--;
                        }
                        hayContrarias = false;
                        // tiros diagonal izquierda inferior 
                        coordenadaX = x;
                        coordenadaY = y;
                        while (coordenadaX != 8 && coordenadaX != -1 && coordenadaY != 8 && coordenadaY != -1) {
                            if (tablero[coordenadaY, coordenadaX] != turno && tablero[coordenadaY, coordenadaX] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaY, coordenadaX] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, coordenadaY};
                                tirosPosibles.Add(arr);
                                break;
                            }
                            coordenadaX--;
                            coordenadaY--;
                        }
                        hayContrarias = false;

                    }
                }
            }
        }
        public static void guardarPartida()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode tableroNode = xmlDoc.CreateElement("tablero");
            xmlDoc.AppendChild(tableroNode);

            XmlNode fichaNode;
            XmlNode colorNode;
            XmlNode columnaNode;
            XmlNode filaNode;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    fichaNode = xmlDoc.CreateElement("ficha");
                    colorNode = xmlDoc.CreateElement("color");
                    columnaNode = xmlDoc.CreateElement("columna");
                    filaNode = xmlDoc.CreateElement("fila");

                    // establecer la fila de la ficha
                    filaNode.InnerText = (y + 1).ToString();

                    // establecer el color de la ficha
                    switch (tablero[y,x])
                    {
                        case 1:
                            colorNode.InnerText = "negro";
                            break;
                        case 2:
                            colorNode.InnerText = "blanco";
                            break;
                    }

                    // establecer la columna de la ficha
                    switch (x)
                    {
                        case 0:
                            columnaNode.InnerText = "A";
                            break;
                        case 1:
                            columnaNode.InnerText = "B";
                            break;
                        case 2:
                            columnaNode.InnerText = "C";
                            break;
                        case 3:
                            columnaNode.InnerText = "D";
                            break;
                        case 4:
                            columnaNode.InnerText = "E";
                            break;
                        case 5:
                            columnaNode.InnerText = "F";
                            break;
                        case 6:
                            columnaNode.InnerText = "G";
                            break;
                        case 7:
                            columnaNode.InnerText = "H";
                            break;
                    }

                    if (colorNode.InnerText != "")
                    {
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
            xmlDoc.Save(@"C:\Users\josed\Downloads\archivo.xml");
        }

        public static void cargarPartida(String rootFile) {
            limpiarTablero();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(rootFile);
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes) {
                if (xmlNode.Name == "ficha") {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    int y = Int32.Parse(xmlNode.ChildNodes[2].InnerText) - 1;
                    int x = 0;
                    switch (xmlNode.ChildNodes[1].InnerText) {
                        case "A":
                            x = 0;
                            break;
                        case "B":
                            x = 1;
                            break;
                        case "C":
                            x = 2;
                            break;
                        case "D":
                            x = 3;
                            break;
                        case "E":
                            x = 4;
                            break;
                        case "F":
                            x = 5;
                            break;
                        case "G":
                            x = 6;
                            break;
                        case "H":
                            x = 7;
                            break;

                    }

                    switch (color) {
                        case "negro":
                            tablero[y, x] = 1;
                            break;
                        case "blanco":
                            tablero[y, x] = 2;
                            break;
                    }

                } else if (xmlNode.Name == "siguienteTiro") {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    GameLogic.turno = (color == "negro") ? 1 : 2;

                }
            }
            calcularPutnos();
            actualizarTirosPosibles();
        }

        public static void limpiarTablero() {
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    tablero[y, x] = -1;  
                }
            }
        }

        public static void calcularPutnos() {
            player1Points = 0;
            player2Points = 0;
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    if (tablero[y,x] == 1) {
                        player1Points++;
                    } else if (tablero[y, x] == 2) {
                        player2Points++;
                    }
                }
            }
        }

    }
}