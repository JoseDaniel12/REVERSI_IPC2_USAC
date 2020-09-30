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
            List<int[]> caminoComido = new List<int[]>();
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
                    caminoComido.Add(new int[] { x, tiroY });
                    hayContrarias = (tablero[tiroY, x] != -1) ? true : hayContrarias;
                } else if (hayContrarias) {
                    caminosComidos.Add(caminoComido);
                    break;
                }
            }
            hayContrarias = false;
            caminoComido = new List<int[]>();
            //comidos por arriba
            for (y = tiroY; y > -1; y--) {
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
                    caminoComido.Add(new int[] { x, y });
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

        public static int[] move(int[,] tablero, List<int[]> tirosPosibles, int turno) {
            tableroCopia = copiarTablero(tablero);
            foreach (int[] tiro in tirosPosibles) { 
                puntajes.Add(calcularPuntajePc(colocarFicha(tableroCopia, tiro[0], tiro[1], turno), turno ));
                tableroCopia = copiarTablero(tablero);
            }
            Debug.WriteLine(puntajes.Max());
            int maxIndex = puntajes.IndexOf(puntajes.Max());
            Debug.WriteLine("Indice de movimiento: " + maxIndex);
            puntajes = new List<int>();
            return tirosPosibles[maxIndex];
        }

    }
}