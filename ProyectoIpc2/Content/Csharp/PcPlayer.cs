using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace ProyectoIpc2.Content.Csharp {

    public static class PcPlayer { 
        public static int[,] tableroCopia = new int[8,8];
        public static List<int> puntajes = new List<int>();

        public static int[,] colocarFicha(int[,] tablero, int tiroX, int tiroY, int turno) {
            List<List<int[]>> caminosComidos = new List<List<int[]>>();
            //_____________________comidos a la derehca____________________________ 
            int x = tiroX;
            int y = tiroY;
            List<int[]> caminoComido = new List<int[]>();
            bool hayContrarias = false;
            caminoComido.Add(new int[] { tiroX, tiroY });
            for (x = (x + 1 < 8) ? x + 1 : x; x < 8; x++) {
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
            for (y = (y + 1 < 8) ? y + 1 : y; y < 8; y++) {
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
            if (x + 1 != 8 && x + 1 != -1 && y + 1 != 8 && y + 1 != -1) {
                x++;
                y++;
            }
            while (x != 8 && x != -1 && y != 8 && y != -1) {
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
            if (x - 1 != 8 && x - 1 != -1 && y - 1 != 8 && y - 1 != -1) {
                x--;
                y--;
            }
            while (x != 8 && x != -1 && y != 8 && y != -1) {
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
            if (x - 1 != 8 && x - 1 != -1 && y + 1 != 8 && y + 1 != -1) {
                x--;
                y++;
            }
            while (x != 8 && x != -1 && y != 8 && y != -1) {
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
            if (x + 1 != 8 && x + 1 != -1 && y - 1 != 8 && y - 1 != -1) {
                x++;
                y--;
            }
            while (x != 8 && x != -1 && y != 8 && y != -1) {
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
                }
            }
            return tablero;
        }


        public static int calcularPuntajePc(int[,] tablero, int turno) {
            int puntaje = 0;
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    if (tablero[y,x] == turno) {
                        puntaje += 1;
                    }
                }
            }
            return puntaje;
        }

        public static int[,] copiarTablero(int[,] tableroOriginal) {
            int[,] copia = new int[8, 8];
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    copia[y, x] = tableroOriginal[y, x];
                }
            }
            return copia;
        }

        public static void move(int[,] tablero, List<int[]> tirosPosibles, int turno) {
            int[] tiroPc = new int[2];
            if (tirosPosibles.Count > 0) {
                tableroCopia = copiarTablero(tablero);
                foreach (int[] tiro in tirosPosibles) {
                    puntajes.Add(calcularPuntajePc(colocarFicha(tableroCopia, tiro[0], tiro[1], turno), turno));
                    tableroCopia = copiarTablero(tablero);
                }
                int maxIndex = puntajes.IndexOf(puntajes.Max());
                puntajes = new List<int>();
                tiroPc = tirosPosibles[maxIndex];
                GameLogic.colocarFicha(tiroPc[0], tiroPc[1]);
            } 
            tableroCopia = new int[8, 8];
            puntajes = new List<int>();
       
        }

    }
}