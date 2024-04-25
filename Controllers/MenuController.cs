using EliteMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EliteMenu.Helpers;

namespace EliteMenu.Controllers
{
    public class MenuController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Menu

        public ActionResult Index(string searchTerm)
        {
            var Menu = string.IsNullOrEmpty(searchTerm) ?
                db.Menu.ToList() :
                db.Menu.Where(r => r.Nome.Contains(searchTerm) || r.Descrizione.Contains(searchTerm)).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MenuList", Menu);
            }
            ViewBag.ActiveLink = "Menu";
            return View(Menu);
        }

        public ActionResult Piatti(int? ristoranteId, string sezione = "", string[] categorie = null, string termineRicerca ="")
        {
            if (!ristoranteId.HasValue)
            {
                return RedirectToAction("Index"); 
            }

            var ristorante = db.Ristoranti
                       .Where(r => r.RistoranteID == ristoranteId.Value)
                       .Select(r => new { r.Nome, r.RistoranteID })
                       .FirstOrDefault();

            if (ristorante == null)
            {
                return HttpNotFound("Ristorante non trovato.");
            }

            
            var menuId = db.Menu
                           .Where(m => m.RistoranteID == ristoranteId.Value)
                           .Select(m => m.MenuID)
                           .FirstOrDefault();

            if (menuId == 0) // Controlla se esiste un menu per il ristorante
            {
                return HttpNotFound("Nessun menu trovato per il ristorante selezionato.");
            }

            ViewBag.RistoranteId = ristoranteId.Value;
            ViewBag.MenuId = menuId;
            ViewBag.Sezione = sezione;
            ViewBag.NomeRistorante = ristorante.Nome;

            
            var query = db.Piatti
                          
                          .Include(p => p.Allergeni)
                          .Include(p => p.Ingredienti)
                          .Where(p => p.MenuID == menuId); 

            
            if (categorie != null && categorie.Any())
            {
                query = query.Where(p => categorie.Contains(p.Categoria));
            }

            // Applica il filtraggio per sezione, se fornita
            if (!string.IsNullOrEmpty(sezione))
            {
                query = query.Where(p => p.Sezione.Equals(sezione, StringComparison.OrdinalIgnoreCase));

            }

            if (!string.IsNullOrWhiteSpace(termineRicerca))
            {
                query = query
        .Where(p => !p.Ingredienti.Any(i => i.Nome.Contains(termineRicerca)) &&
                    !p.Allergeni.Any(a => a.Nome.Contains(termineRicerca)));
            }

            // Ordina i risultati basandosi sull'ordine delle sezioni
            var ordineSezioni = new List<string> { "Antipasti", "Primi", "Secondi", "Dolci" };
            var piattiFiltrati = query.ToList().OrderBy(p => ordineSezioni.IndexOf(p.Sezione)).ToList();

            // Raggruppamento dei piatti per sezione per la visualizzazione
            var risultato = piattiFiltrati.GroupBy(p => p.Sezione)
                                          .OrderBy(g => ordineSezioni.IndexOf(g.Key))
                                          .ToList();

            // Gestione della risposta AJAX o standard
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListaPiatti", risultato);
            }
            return View(risultato);
        }



        public ActionResult TuttiIPiatti(string searchTerm = "", string[] categorie = null)
        {
            var query = db.Piatti
                          .Include(p => p.Menu.Ristoranti) 
                          .Include(p => p.Ingredienti);   

            // Filtraggio basato sul termine di ricerca.
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Nome.Contains(searchTerm)
                                         || p.Descrizione.Contains(searchTerm)
                                         || p.Menu.Ristoranti.Nome.Contains(searchTerm));
            }

            // Filtraggio basato sulle categorie selezionate.
            if (categorie != null && categorie.Any())
            {
                query = query.Where(p => categorie.Contains(p.Categoria));
            }

            var piattiFiltrati = query.ToList(); // Ottenimento dei piatti filtrati.

            // Restituzione della vista parziale se la richiesta è di tipo AJAX.
            if (Request.IsAjaxRequest())
            {
                return PartialView("_TuttiIPiatti", piattiFiltrati); 
            }

            return View(piattiFiltrati); // Restituisce la vista completa con i piatti filtrati.
        }












    }
}