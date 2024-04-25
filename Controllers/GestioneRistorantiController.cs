using EliteMenu.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;


namespace EliteMenu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GestioneRistorantiController : Controller
    {
        private DBContext db = new DBContext();

        // GET: GestioneRistoranti
        public ActionResult Index()
        {
            var ristoranti = db.Ristoranti.ToList(); 
            return View(ristoranti);
        }


        // GET: GestioneRistoranti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ristoranti ristorante = db.Ristoranti.Find(id);
            if (ristorante == null)
            {
                return HttpNotFound();
            }
            return View(ristorante);
        }

        // GET: GestioneRistoranti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GestioneRistoranti/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RistoranteID,Nome,Indirizzo,Telefono,Email,Descrizione")] Ristoranti ristorante, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    // Crea un nome di file univoco per evitare sovrascritture
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    fileUpload.SaveAs(path);

                    // Salva il percorso relativo nel campo Foto
                    ristorante.Foto = "~/Uploads/" + fileName;
                }

                db.Ristoranti.Add(ristorante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ristorante);
        }


        // GET: GestioneRistoranti/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ristoranti ristorante = db.Ristoranti.Find(id);
            if (ristorante == null)
            {
                return HttpNotFound();
            }
            return View(ristorante);
        }

        //// POST: GestioneRistoranti/Edit/5
        [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit([Bind(Include = "RistoranteID,Nome,Indirizzo,Telefono,Email,Descrizione")] Ristoranti ristoranteModel, HttpPostedFileBase fileUpload)
{
    if (ModelState.IsValid)
    {
        var existingRistorante = db.Ristoranti.Find(ristoranteModel.RistoranteID);
        if (existingRistorante == null)
        {
            return HttpNotFound("Ristorante non trovato.");
        }

        if (fileUpload != null && fileUpload.ContentLength > 0)
        {
            // Genera un nome di file univoco
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
            var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
            fileUpload.SaveAs(path);

            // Aggiorna il percorso dell'immagine nel database
            existingRistorante.Foto = "~/Uploads/" + fileName;
        }
        
        // Aggiorna le altre proprietà dell'entità esistente con i valori dal modello
        existingRistorante.Nome = ristoranteModel.Nome;
        existingRistorante.Indirizzo = ristoranteModel.Indirizzo;
        existingRistorante.Telefono = ristoranteModel.Telefono;
        existingRistorante.Email = ristoranteModel.Email;
        existingRistorante.Descrizione = ristoranteModel.Descrizione;

        // Se non viene caricato un nuovo file, non è necessario impostare la proprietà Foto come modificata
        db.Entry(existingRistorante).State = EntityState.Modified;
        db.SaveChanges();
        
        return RedirectToAction("Details", new { id = existingRistorante.RistoranteID });
    }

    return View(ristoranteModel);
}












        // GET: GestioneRistoranti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ristoranti ristorante = db.Ristoranti.Find(id);
            if (ristorante == null)
            {
                return HttpNotFound();
            }
            return View(ristorante);
        }

        // POST: GestioneRistoranti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ristoranti ristorante = db.Ristoranti.Find(id);
            db.Ristoranti.Remove(ristorante);
            db.SaveChanges();
            return RedirectToAction("Index");
        }





       



    }
}
