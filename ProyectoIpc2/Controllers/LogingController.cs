using System.Web.Mvc;
using ProyectoIpc2.Models;

namespace ProyectoIpc2.Controllers
{
    public class LogingController : Controller
    {
        public ActionResult Loging()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Loging(string nombre, string contraseña)
        {
            using (ReversiContext db = new ReversiContext())
            {
                foreach (Usuario usuario in db.Usuario)
                    if (usuario.Name == nombre && usuario.Password == contraseña)
                    {
                        return RedirectToAction("Tablero", "Tablero");
                    }
            }

            return View();
        }
    }
}