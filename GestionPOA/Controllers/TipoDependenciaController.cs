using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionPOA.Models;

namespace GestionPOA.Controllers
{
    public class TipoDependenciaController : Controller
    {
        private PEDIEntities db = new PEDIEntities();

        // GET: TipoDependencia/TipoDependencia
        public ActionResult TipoDependencia()
        {

            var tiposdependencias = db.spTipoDependenciaConsultDHBD().ToList();
            return Json(new { listtiposdependencias = tiposdependencias }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}