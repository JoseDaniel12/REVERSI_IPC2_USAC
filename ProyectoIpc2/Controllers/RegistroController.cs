using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIpc2.Models;

namespace ProyectoIpc2.Controllers
{
    public class RegistroController : Controller
    {
        // GET: Registro
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(Usuario jugador)
        {
            using (ReversiContext db = new ReversiContext())
            {
                try
                {
                    db.Usuario.Add(jugador);
                    db.SaveChanges();
                    return RedirectToAction("Loging", "Loging");
                } catch
                {
                    return RedirectToAction("Registro", "Registro");
                }
                
            }
           
        }
    }          
}