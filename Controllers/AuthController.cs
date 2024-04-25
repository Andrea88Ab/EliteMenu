using EliteMenu.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace EliteMenu.Controllers
{
    public class AuthController : Controller
    {

        DBContext db = new DBContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Utenti.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // Autenticazione riuscita, procedi con il login
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    Session["UserId"] = user.Id;
                    Session["Username"] = user.Nome;
                    Session["Avatar"] = user.Avatar;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Autenticazione fallita, mostra un messaggio di errore
                    ModelState.AddModelError("", "L'email o la password non sono corretti.");
                }
            }
            return View(model);
        }



        public ActionResult Logout()
        {
            
            FormsAuthentication.SignOut();

            
            Session.Clear();

            
            Session.Abandon();

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            
            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                return RedirectToAction("Index", "Home");
            }

            
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (db.Utenti.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Un utente con questa email esiste già.");
                    return View(model);
                }

                var newUser = new Utenti
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password), 
                    DataCreazione = DateTime.Now,
                    Ruolo = model.Ruolo,
                    Avatar = Url.Content("~/Content/Assets/Icon/Avatar-icon.svg")
                   
                };

               

                db.Utenti.Add(newUser);
                db.SaveChanges();

                
                FormsAuthentication.SetAuthCookie(model.Email, createPersistentCookie: false);
                Session["Username"] = newUser.Nome;
                Session["Avatar"] = newUser.Avatar;
                return RedirectToAction("Index", "Home"); 
            }

            
            return View(model);
        }


        [HttpGet]
        public  new ActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            var user = db.Utenti.Find(userId);

            if (user == null)
            {
                
                return HttpNotFound();
            }

            
            var model = new UserProfileViewModel
            {
                Nome = user.Nome,
                Email = user.Email,
                Avatar = user.Avatar, 
                                            
            };

            return View(model);
        }












        [HttpGet]
        public ActionResult EditProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            var user = db.Utenti.Find(userId);

            var model = new ProfileEditViewModel
            {
                Nome = user.Nome,
                Email = user.Email,
                Avatar = user.Avatar

                // Non impostare il campo AvatarFile perché è per l'upload del file
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ProfileEditViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var user = db.Utenti.Find(userId);

                if (user == null)
                {
                    // Gestisci l'errore, l'utente non esiste
                    return HttpNotFound("Utente non trovato");
                }
                else
                {
                    user.Nome = model.Nome;
                    user.Email = model.Email;

                    // Gestisci l'upload dell'avatar se presente
                    if (model.AvatarFile != null && model.AvatarFile.ContentLength > 0)
                    {
                        // Utilizza un GUID per creare un nome file univoco
                        string fileExtension = Path.GetExtension(model.AvatarFile.FileName);
                        string fileName = Guid.NewGuid().ToString() + fileExtension;  // Esempio: 123e4567-e89b-12d3-a456-426614174000.jpg
                        string path = Path.Combine(Server.MapPath("~/uploads/avatars"), fileName);
                        model.AvatarFile.SaveAs(path);

                        // Aggiorna il percorso dell'avatar nell'oggetto utente
                        user.Avatar = Url.Content("~/uploads/avatars/" + fileName);
                    }

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    // Reindirizza alla vista del profilo
                    return RedirectToAction("Profile");
                }
            }

            // Se siamo qui, qualcosa è andato storto
            return View(model);
        }






    }








}
