using EliteMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO; 

namespace EliteMenu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GestioneMenuController : Controller
    {
        private DBContext db = new DBContext();

        // GET: GestioneMenu
        public ActionResult Index()
        {
            var menu = db.Menu.Include(m => m.Ristoranti);
            return View(menu.ToList());
        }

        public ActionResult CreateMenu()
        {
            ViewBag.RistoranteID = new SelectList(db.Ristoranti, "RistoranteID", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMenu([Bind(Include = "Nome, RistoranteID, Descrizione")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index"); // Oppure dove preferisci reindirizzare dopo la creazione
            }

            // Se il modello non è valido, ricarica il dropdown per la selezione del ristorante
            ViewBag.RistoranteID = new SelectList(db.Ristoranti, "RistoranteID", "Nome", menu.RistoranteID);
            return View(menu);
        }

        // GET: Modifica Menu
        public ActionResult EditMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            ViewBag.RistoranteID = new SelectList(db.Ristoranti, "RistoranteID", "Nome", menu.RistoranteID);
            return View(menu);
        }

        // POST: Applica le Modifiche al Menu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenu([Bind(Include = "MenuID,Nome,RistoranteID,Descrizione")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RistoranteID = new SelectList(db.Ristoranti, "RistoranteID", "Nome", menu.RistoranteID);
            return View(menu);
        }



        // GET: Conferma eliminazione Menu
        public ActionResult DeleteMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Include(m => m.Piatti).SingleOrDefault(m => m.MenuID == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Eliminazione Menu Confermata
        [HttpPost, ActionName("DeleteMenu")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMenuConfirmed(int id)
        {
            Menu menu = db.Menu.Include(m => m.Piatti).SingleOrDefault(m => m.MenuID == id);

            if (menu != null)
            {
                // Elimina tutti i piatti associati a questo menu
                foreach (var piatto in menu.Piatti.ToList())
                {
                    db.Piatti.Remove(piatto);
                }

                // Ora elimina il menu
                db.Menu.Remove(menu);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }






        // GET: Dettagli del Menu
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Include(m => m.Piatti).SingleOrDefault(m => m.MenuID == id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Crea Nuovo Piatto
        public ActionResult CreatePiatto(int menuId)
        {
            ViewBag.MenuID = menuId;
            // Prepara eventuali ViewBag per dropdown qui
            return View();
        }

        // POST: Crea Nuovo Piatto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePiatto([Bind(Include = "PiattoID,MenuID,Nome,Descrizione,Prezzo,Sezione,Categoria")] Piatti piatto, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    fileUpload.SaveAs(path);

                    
                    piatto.Foto = "~/Uploads/" + fileName;
                }

                db.Piatti.Add(piatto);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = piatto.MenuID });
            }

            ViewBag.MenuID = piatto.MenuID;
            return View(piatto);
        }


        // GET: Modifica Piatto
        public ActionResult EditPiatto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piatti piatto = db.Piatti.Find(id);
            if (piatto == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuID = piatto.MenuID; 
            return View(piatto);
        }

        // POST: Modifica Piatto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPiatto([Bind(Include = "PiattoID,MenuID,Nome,Descrizione,Prezzo,Sezione,Categoria")] Piatti piatto, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    // Gestisce il caricamento di una nuova immagine
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    fileUpload.SaveAs(path);
                    piatto.Foto = "~/Uploads/" + fileName; // Aggiorna il percorso dell'immagine
                }
                else
                {
                    // Mantiene l'immagine esistente se non viene caricata una nuova immagine
                    var existingPiatto = db.Piatti.AsNoTracking().FirstOrDefault(p => p.PiattoID == piatto.PiattoID);
                    if (existingPiatto != null)
                    {
                        piatto.Foto = existingPiatto.Foto;
                    }
                }

                db.Entry(piatto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = piatto.MenuID });
            }
            ViewBag.MenuID = piatto.MenuID;
            return View(piatto);
        }


        // GET: Conferma eliminazione Piatto
        public ActionResult DeletePiatto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piatti piatto = db.Piatti.Find(id);
            if (piatto == null)
            {
                return HttpNotFound();
            }
            return View(piatto);
        }

        // POST: Eliminazione Piatto Confermata
        [HttpPost, ActionName("DeletePiatto")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Piatti piatto = db.Piatti.Find(id);
            if (piatto != null)
            {
                db.Piatti.Remove(piatto);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }




    }
}
