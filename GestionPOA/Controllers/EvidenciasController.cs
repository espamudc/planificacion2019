using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionPOA.Models;
using System.IO;

namespace GestionPOA.Controllers
{
    public class EvidenciasController : Controller
    {
        private PEDIEntities db = new PEDIEntities();

        // GET: Evidencias/upload
        public ActionResult upload()
        {
            bool _estado = false;
            string _mensaje = "Ocurrió un error inesperado";
            try
            {
                if (Request.Files.Count == 0)
                {
                    _mensaje = "Debe seleccionar al menos un archivo PDF.";
                }
                else
                {
                    var idmeta = Request.Form["id"];
                    var IntervaloID = Request.Form["IntervaloID"];
                    var meta = Convert.ToInt32(idmeta);
                    var intervalo = Convert.ToInt32(IntervaloID);
                    var programcion = db.Programacion.Where(p => p.MetaID == meta)
                                   .Where(p => p.IntervaloId == intervalo)
                                   .Select(p => new { id = p.id, planificado = p.planificado })
                                   .SingleOrDefault();
                    if (programcion.planificado == "0")
                    {
                        _mensaje = "La planificación registrada en esta meta no puede ser 0 en caso de que desee subir una evidencia.";
                    }
                    else
                    {

                        foreach (string file in Request.Files)
                        {
                            var fileContent = Request.Files[file];
                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                string Extension = Path.GetExtension(fileContent.FileName).ToLower();
                                if (Extension != ".pdf")
                                {
                                    _mensaje = "Solo se permiten archivos PDF.";
                                }
                                else
                                {
                                    TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
                                    double unixTime = span.TotalSeconds;
                                    string año = Convert.ToString(DateTime.Today.Year);

                                    string nameFile = Session["departamento"] + "-Evidencia" + año + "-idM" + idmeta + "-idP" + programcion.id + "-" + unixTime + Extension;
                                    var path = Path.Combine(Server.MapPath("~/App_Data/Evidencias/"), nameFile);
                                    string[] allowedExtensions = { ".pdf" };
                                    for (int count = 0; count < allowedExtensions.Length; count++)
                                    {
                                        if (Extension == allowedExtensions[count])
                                        {
                                            int idEvidencia = Convert.ToInt32(db.spEvidenciasInsert(programcion.id, nameFile).FirstOrDefault());
                                            if (idEvidencia == 0)
                                            {
                                                _mensaje = "Ocurrió un error al intentar ingresar el registro en la base de datos, intente nuevamente.";
                                            }
                                            else
                                            {
                                                fileContent.SaveAs(path);
                                                _estado = true;
                                                _mensaje = "¡Archivo ingresado de forma correcta!";
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mensaje = "El tamaño del archivo excedió el tamaño permitido. Ocurrió un error técnico. La excepción que se produjo fue: " + ex.Message;
            }
            
            return Json(new { estado = _estado, mensaje =_mensaje }, JsonRequestBehavior.AllowGet);
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
