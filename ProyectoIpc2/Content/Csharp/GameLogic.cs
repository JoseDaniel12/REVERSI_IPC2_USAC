using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
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
        public static int player1Points = 0;
        public static int player2Points = 0;
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
            foreach (int [] tiroPosible in tirosPosibles) {
                if (tiroPosible[0] == tiroX && tiroPosible[1] == tiroY) {
                    //comidos a la derehca 
                    for (int x = tiroX; x < 8; x++) {
                        if (tablero[tiroY, x] != turno) {
                            caminoComido.Add(new int[] {tiroY, x});
                        } else if (tablero[tiroY, x] == turno) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                    }
                    caminoComido = new List<int[]>();
                    //comidos a la izquierda 
                    for (int x = tiroX; x > 0; x--) {
                        Debug.WriteLine("(" + x + ", " + tiroY + ") -> " + tablero[tiroY, x]);
                        if (tablero[tiroY, x] != turno) {
                            caminoComido.Add(new int[] { tiroY, x });
                        } else if (tablero[tiroY, x] == turno) {
                            caminosComidos.Add(caminoComido);
                            break;
                        }
                    }
                    caminoComido = new List<int[]>();
                }
            }

            foreach (List<int[]> camino in caminosComidos) {
                foreach (int[] ficha in camino) {
                    tablero[ficha[0], ficha[1]] = turno;
                }
            }

        }

        public static void actualizarCasillasValidas() {
            tirosPosibles = new List<int[]>();
            bool hayContrarias = false;
            // recorrer casilla por casilla del tablero para encotrar las fichas del turno corresponiete
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    int coordenadaX = x;
                    int coordenadaY = y;
                    if (tablero[x, y] == turno) {
                        // tiros a la derecha
                        for (coordenadaX = x; coordenadaX < 8; coordenadaX++) {
                            if (tablero[coordenadaX, y] != turno && tablero[coordenadaX, y] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaX, y] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, y };
                                tirosPosibles.Add(arr);
                            }
                        }
                        // tiros por la izquierda
                        for (coordenadaX = x; coordenadaX > 0; coordenadaX--) {
                            if (tablero[coordenadaX, y] != turno && tablero[coordenadaX, y] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaX, y] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, y };
                                tirosPosibles.Add(arr);
                            }
                        }
                        // tiros por arriba
                        for (coordenadaY = y; coordenadaY > 0; coordenadaY--) {
                            if (tablero[x, coordenadaY] != turno && tablero[x, coordenadaY] != -1) {
                                hayContrarias = true;
                            } else if (tablero[x, coordenadaY] == -1 && hayContrarias == true) {
                                int[] arr = { x, coordenadaY };
                                tirosPosibles.Add(arr);
                            }
                        }
                        // tiros por abajo
                        for (coordenadaY = y; coordenadaY < 8; coordenadaY++) {
                            if (tablero[x, coordenadaY] != turno && tablero[x, coordenadaY] != -1) {
                                hayContrarias = true;
                            } else if (tablero[x, coordenadaY] == -1 && hayContrarias == true) {
                                int[] arr = { x, coordenadaY };
                                tirosPosibles.Add(arr);
                            }
                        }
                        // tiros diagonal derecha supeiror 
                        while (x != 7 && x != 0 && y != 7 && y != 0) {
                            if (tablero[coordenadaX, coordenadaY] != turno && tablero[coordenadaX, coordenadaY] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaX, coordenadaY] == -1 && hayContrarias == true) {
                                int[] arr = {coordenadaX, coordenadaY };
                                tirosPosibles.Add(arr);
                            }
                            x++;
                            y++;
                        }
                        // tiros diagonal izquierda supeiror 
                        while (x != 7 && x != 0 && y != 7 && y != 0) {
                            if (tablero[coordenadaX, coordenadaY] != turno && tablero[coordenadaX, coordenadaY] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaX, coordenadaY] == -1 && hayContrarias == true) {
                                int[] arr = {coordenadaX, coordenadaY};
                                tirosPosibles.Add(arr);
                            }
                            x--;
                            y++;
                        }
                        // tiros diagonal derecha inferior 
                        while (x != 7 && x != 0 && y != 7 && y != 0) {
                            if (tablero[coordenadaX, coordenadaY] != turno && tablero[coordenadaX, coordenadaY] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaX, coordenadaY] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, coordenadaY };
                                tirosPosibles.Add(arr);
                            }
                            x++;
                            y--;
                        }
                        // tiros diagonal izquierda inferior 
                        while (x != 7 && x != 0 && y != 7 && y != 0) {
                            if (tablero[coordenadaX, coordenadaY] != turno && tablero[coordenadaX, coordenadaY] != -1) {
                                hayContrarias = true;
                            } else if (tablero[coordenadaX, coordenadaY] == -1 && hayContrarias == true) {
                                int[] arr = { coordenadaX, coordenadaY };
                                tirosPosibles.Add(arr);
                            }
                            x--;
                            y--;
                        }

                    }
                }
            }
        }
        public static void guardarPartida(int[,] tablero)
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
                    switch (tablero[x, y])
                    {
                        case 1:
                            colorNode.InnerText = "blanco";
                            break;
                        case 2:
                            colorNode.InnerText = "negro";
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
            colorNode.InnerText = "blanco";
            siguienteTiroNode.AppendChild(colorNode);
            tableroNode.AppendChild(siguienteTiroNode);
            xmlDoc.Save(@"C:\Users\josed\Downloads\archivo.xml");
        }

        public static void cargarPartida(String rootFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(rootFile);
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
            {
                if (xmlNode.Name == "ficha")
                {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    int y = Int32.Parse(xmlNode.ChildNodes[2].InnerText) - 1;
                    int x = 0;
                    switch (xmlNode.ChildNodes[1].InnerText)
                    {
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

                    switch (color)
                    {
                        case "blanco":
                            tablero[x, y] = 1;
                            break;
                        case "negro":
                            tablero[x, y] = 2;
                            break;
                    }
                }

            }
        }

        public static void limpiarTablero() {
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    tablero[x, y] = -1;  
                }
            }
        }

    }
}