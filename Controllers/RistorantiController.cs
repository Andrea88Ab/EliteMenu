using EliteMenu.Models;
using EliteMenu.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Mail;


namespace EliteMenu.Controllers
{
    public class RistorantiController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Ristoranti

        public ActionResult Index(string searchTerm)
        {
            // Query per trovare ristoranti basati sul termine di ricerca
            var ristorantiViewModel = db.Ristoranti
                .Where(r => string.IsNullOrEmpty(searchTerm) || r.Nome.Contains(searchTerm) || r.Descrizione.Contains(searchTerm))
                .Select(r => new RistoranteViewModel
                {
                    Ristorante = r,
                    Piatti = db.Menu
                        .Where(m => m.RistoranteID == r.RistoranteID)
                        .SelectMany(m => m.Piatti)
                        .Select(p => new PiattoViewModel { Nome = p.Nome, Foto = p.Foto }) 
                        .ToList()
                }).ToList();

            if (Request.IsAjaxRequest())
            {
                // Ritorna una PartialView per richieste AJAX.
                return PartialView("_RistorantiList", ristorantiViewModel);
            }

            ViewBag.ActiveLink = "Ristoranti";
            // Passa il ViewModel alla vista principale.
            return View(ristorantiViewModel);
        }





        // Dettagli di un singolo ristorante
        // GET: Ristoranti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var ristorante = db.Ristoranti
                               
                               .Include(r => r.CommentiRistoranti.Select(c => c.Utenti))
                               .SingleOrDefault(r => r.RistoranteID == id);


            if (ristorante == null)
            {
                return HttpNotFound();
            }

            ristorante.CommentiRistoranti = ristorante.CommentiRistoranti.OrderByDescending(c => c.DataOra).ToList();

            return View(ristorante);
        }




       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggiungiCommento(int RistoranteID, string Testo, int Valutazione, string Titolo)
        {
            // Verifica se l'utente è autenticato
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Reindirizza alla pagina di login se l'utente non è autenticato
            }

            // Ottieni l'ID dell'utente corrente
            var userId = (int)Session["UserId"]; 

            // Crea l'oggetto Commento con i dati ricevuti
            var commento = new CommentiRistoranti
            {
                RistoranteID = RistoranteID,
                UtenteID = userId, // Imposta l'ID dell'utente autenticato
                Testo = Testo,
                Valutazione = Valutazione,
                DataOra = DateTime.Now,
                Titolo = Titolo
            };

            

            
            db.CommentiRistoranti.Add(commento);
            db.SaveChanges();

            // Reindirizza l'utente alla pagina di dettaglio del ristorante con un messaggio di successo
            return RedirectToAction("Details", new { id = RistoranteID });
        }



        
        public ActionResult Prenota()
        {
            return View();
        }


        private void InviaEmailFittizia(string toEmail, string subject, string body)
        {
            // Simula l'invio di un'email stampando i dettagli nella console di debug
            Console.WriteLine("Simulazione invio email:");
            Console.WriteLine($"A: {toEmail}");
            Console.WriteLine($"Oggetto: {subject}");
            Console.WriteLine($"Corpo del messaggio: {body}");
            Console.WriteLine("Email inviata con successo (simulato).");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviaPrenotazione(DateTime data, DateTime ora, int persone, string areaFumatori, string allergie)
        {
            var emailBody = $"Richiesta di prenotazione per il {data.ToShortDateString()} alle ore {ora.ToShortTimeString()} per {persone} persone.";
            emailBody += $"\nArea fumatori: {areaFumatori}";
            emailBody += $"\nAllergie: {allergie}";

            // Simula l'invio della email
            InviaEmailFittizia("email@ristorante.com", "Nuova Prenotazione", emailBody);

            // Utilizza TempData per passare il messaggio alla vista di conferma
            TempData["Message"] = "La tua richiesta di prenotazione è stata inviata. Controlla la tua email per i dettagli della conferma.";

            // Redirect alla vista di conferma
            return RedirectToAction("ConfermaPrenotazione");
        }

        // Metodo per la pagina di conferma
        public ActionResult ConfermaPrenotazione()
        {
            return View();
        }



    }
}


    

