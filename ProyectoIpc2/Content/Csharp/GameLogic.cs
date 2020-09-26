using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;

namespace ProyectoIpc2.Content.Csharp
{
    public static class GameLogic
    {
        public static int userId = -1;
        public static string tipoPartida = "";
        public static int[,] tablero = new int[8, 8] {
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
            {-1, -1, -1, -1, -1, -1, -1, -1},
        };

        static void Main(String[] args)
        {

        }

        public static void colocarFicha(int x, int y)
        {
            switch(tablero[x,y])
            {
                case -1:
                    tablero[x,y] = 1;
                    break;
                case 1:
                    tablero[x,y] = 2;
                    break;
                case 2:
                    tablero[x,y] = -1;
                    break;
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
            Debug.WriteLine(rootFile);
            xmlDoc.Load(rootFile);
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
            {
                if (xmlNode.Name == "ficha")
                {
                    string color = xmlNode.ChildNodes[0].InnerText;
                    Debug.WriteLine(xmlNode.ChildNodes[1].InnerText);
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

        public static void limpiarTablero()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    tablero[x, y] = -1;  
                }
            }
        }

    }
}