using GestionPOA.Models;
using GestionPOA.MyClass;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GestionPOA.Controllers
{
    public class AdminController : Controller
    {
        private PEDIEntities db = new PEDIEntities();
        private F_Encriptacion encrypt = new F_Encriptacion();

        public ActionResult Index()
        {
            var page = Session["Page"];
            if ( Convert.ToString(page) == "verify")
            {
                return View("Index");
            }
            else {
                return RedirectToAction("Login","Admin");
            }
        }

        public ActionResult Login()
        {
            var page = Session["Page"];
            if (Convert.ToString(page) != "verify")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
            
        }
        public ActionResult POAorPEDI(string singIN)
        {
            if ((singIN == "POA") || (singIN == "PEDI"))
            {
                var status = db.POAorPEDI(singIN, Convert.ToInt32(Session["tipodepartament"]),  Convert.ToInt32(Session["department"])).FirstOrDefault();
                if (status== "No Existe") {
                    return Json(new { status }, JsonRequestBehavior.AllowGet);
                }
                Session["POAorPEDI"] = singIN;
                Session["Page"] = "verify";
                return Json(new { msj = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { msj = "No se pudo ingresar al sistema" }, JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return Json(new { msj = "Sesión Cerrada" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Logearse(string usuario, string password)
        {
            var claveEncrypt = encrypt.Encriptar(password);
            var username =  db.spLoginIngreso2(usuario, claveEncrypt).FirstOrDefault();
            if (username != null)
            {
                Session["user"] = usuario;
                Session["departamento"] = username.nombreDependencia;
                Session["department"] = username.idDependencia;
                Session["tipodepartamento"] = username.tipoDependencia;
                Session["tipodepartament"] = username.idTipoDependencia;
                Session["rol"] = username.TipoRol;
                Session["nombres"] = username.Nombres;
                string rutaFoto = "/Content/img/default-avatar.png";
                if (username.foto.Trim()!="")
                {
                    rutaFoto = "http://gestionacademica.espam.edu.ec/img/fotos/"+username.foto;
                }

                Session["foto"] = rutaFoto;
                if (username.TipoRol != "Administrador")
                {
                    var rol = db.POAorPEDI("POA", username.idTipoDependencia, username.idDependencia).FirstOrDefault();
                    if (rol=="Existe") {
                        Session["Page"] = "verify";
                        Session["POAorPEDI"] = "POA";
                    }
                    return Json(new { username, rol, tipo = "Usuario" }, JsonRequestBehavior.AllowGet);
                }
                else 
                {
                    var rol = db.POAorPEDI("PEDI", username.idTipoDependencia, username.idDependencia).FirstOrDefault();
                    
                    if (rol== "No Existe") {
                        var tipoPlanificacion = db.TipoPlanificacion.Where(p => p.eliminado == false && p.Descripcion=="PEDI")
                                        .Select(p => new { idPlanificacion =p.TipoPlanificacionId, descripcion = p.Descripcion})
                                        .ToList();

                        var periocidad = db.Periocidad.Where(p => p.eliminado == false && p.Periodo=="Anual")
                                       .Select(p => new { idPeriocidad = p.id})
                                       .ToList();

                        if (tipoPlanificacion.Count > 0 && periocidad.Count>0)
                        {

                            Planificacion planificacion = new Planificacion();
                            planificacion.DepartamentoID = username.idDependencia;
                            planificacion.eliminado = false;
                            planificacion.fecha = DateTime.Now;
                            planificacion.TipoDependenciaID = username.idTipoDependencia;
                            planificacion.TipoPlanificacionId = tipoPlanificacion.FirstOrDefault().idPlanificacion;
                            planificacion.PeriocidadID = periocidad.FirstOrDefault().idPeriocidad;
                            db.Planificacion.Add(planificacion);
                            db.SaveChanges();                        
                            Session["Page"] = "verify";
                            Session["POAorPEDI"] = "PEDI";
                        }
                    }
                    return Json(new { username, rol, tipo="Administrador" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "usuario y/o contraseña incorrecta");
            }
        }
    }
}