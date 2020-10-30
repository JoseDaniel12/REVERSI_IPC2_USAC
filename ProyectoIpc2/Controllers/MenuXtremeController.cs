using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProyectoIpc2.Content.Csharp;

namespace ProyectoIpc2.Controllers
{
    public class MenuXtremeController : Controller
    {
        public ActionResult MenuXtreme() {
            return View();
        }

        [HttpPost]
        public ActionResult AjustarConfsXtreme(FormCollection collection) {
            bool validado = true;
            int anchoTablero = Int32.Parse(Request.Params["anchoTablero"].ToString());
            int altoTablero = Int32.Parse(Request.Params["altoTablero"].ToString());
            string modalidad = Request.Params["modalidad"].ToString();

            string color1_p1 = Request.Params["color1_p1"].ToString().ToLower();
            string color2_p1 = Request.Params["color2_p1"].ToString().ToLower();
            string color3_p1 = Request.Params["color3_p1"].ToString().ToLower();
            string color4_p1 = Request.Params["color4_p1"].ToString().ToLower();
            string color5_p1 = Request.Params["color5_p1"].ToString().ToLower();
            string color1_p2 = Request.Params["color1_p2"].ToString().ToLower();
            string color2_p2 = Request.Params["color2_p2"].ToString().ToLower();
            string color3_p2 = Request.Params["color3_p2"].ToString().ToLower();
            string color4_p2 = Request.Params["color4_p2"].ToString().ToLower();
            string color5_p2 = Request.Params["color5_p2"].ToString().ToLower();
            List<String> colores_p1 = new List<String> { color1_p1, color2_p1, color3_p1, color4_p1, color5_p1 };
            List<String> colores_p2 = new List<String> { color1_p2, color2_p2, color3_p2, color4_p2, color5_p2 };

            while (colores_p1.Contains("ninguno")) {
                colores_p1.Remove("ninguno");
            }

            while (colores_p2.Contains("ninguno")) {
                colores_p2.Remove("ninguno");
            }

            if (colores_p1.Intersect(colores_p2).ToList().Count() > 0) {
                validado = false;
            } else if (colores_p1.Intersect(colores_p1).ToList().Count() != colores_p1.Count()) {
                validado = false;
            } else if (colores_p2.Intersect(colores_p2).ToList().Count() != colores_p1.Count()) {
                validado = false;
            } else if (colores_p1.Count() == 0 || colores_p2.Count() == 0) {
                validado = false;
            }


            if (validado) {
                GameLogic.anchoTablero = anchoTablero;
                GameLogic.altoTablero = altoTablero;
                GameLogic.coloresElegidos = new List<List<string>> {colores_p1, colores_p2 };
                GameLogic.coloresActuales = new List<string> {colores_p1[0], colores_p2[0]};
                if (modalidad == "Inverso") {
                    GameLogic.esModoInverso = true;
                }
                GameLogic.iniciarJuego();
                return Content(JsonConvert.SerializeObject(true));
            }

            return Content(JsonConvert.SerializeObject(false));
        }
    }
}