using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProyectoIpc2.Content.Csharp;
using ProyectoIpc2.Models;

namespace ProyectoIpc2.Controllers
{
    public class PerfilController : Controller
    {
        public ActionResult Perfil()
        {
            using (ReversiContext db = new ReversiContext()) {
                Usuario usuario = db.Usuario.Find(GameLogic.userId);
                ViewBag.userId = usuario.UserId;
                ViewBag.name = usuario.Name;
                ViewBag.userName = usuario.UserName;
                ViewBag.lastName = usuario.LastName;
                ViewBag.mail = usuario.Mail;
                ViewBag.bornDate = usuario.BornDate.ToShortDateString();
                ViewBag.country = usuario.Country;

                List<Dictionary<string, string>> partidasGanadas = new List<Dictionary<string, string>>();
                foreach (Partida row in db.Partida) {
                    Dictionary<string, string> partidaGanadas = new Dictionary<string, string>();
                    if (row.UserId == usuario.UserId) {
                        partidaGanadas.Add("resultado", row.Resultado);
                        partidaGanadas.Add("gameId", row.GameId.ToString());
                        partidaGanadas.Add("gameType", row.GameType.ToString());
                        if (row.HostColor == 1) {
                            partidaGanadas.Add("miTiempo", row.Player1Time.ToString() + " seg");
                            partidaGanadas.Add("misPuntos", row.Player1Points.ToString());
                            partidaGanadas.Add("tiempoAdversario", row.Player2Time.ToString() + " seg");
                            partidaGanadas.Add("puntosAdversario", row.Player2Points.ToString());
                        } else {
                            partidaGanadas.Add("miTiempo", row.Player2Time.ToString() + " seg");
                            partidaGanadas.Add("misPuntos", row.Player2Points.ToString());
                            partidaGanadas.Add("tiempoAdversario", row.Player1Time.ToString() + " seg");
                            partidaGanadas.Add("puntosAdversario", row.Player1Points.ToString());
                        }
                    }
                    partidasGanadas.Add(partidaGanadas);
                }
                ViewBag.partidasGanadas = partidasGanadas;



            }
            return View();
        }


    }
    
}