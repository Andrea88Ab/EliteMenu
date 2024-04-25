using System;
using System.Linq;
using System.Web.Security;
using EliteMenu.Models; // Assicurati che questo namespace corrisponda al luogo dove si trova il tuo DBContext e il modello Utenti.

namespace EliteMenu.Models
{
    public class RoleManager : RoleProvider
    {
        // Assumi che DBContext sia il contesto EF che stai utilizzando. 
        // Sostituisci con il nome reale del tuo DbContext.
        private DBContext db = new DBContext();

        // ApplicationName non è strettamente necessario per questo esempio, quindi puoi semplicemente implementarlo con proprietà auto-implementate se non ti serve una logica specifica.
        public override string ApplicationName { get; set; }

        // Questo metodo non è necessario per l'esempio fornito, quindi lo lasciamo non implementato.
        public override void AddUsersToRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }

        public override void CreateRole(string roleName) { throw new NotImplementedException(); }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotImplementedException(); }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) { throw new NotImplementedException(); }

        public override string[] GetAllRoles() { throw new NotImplementedException(); }

        // Nota: Il parametro originariamente era 'string Id', ma la funzione IsUserInRole si aspetta 'string username'. 
        // Ho cambiato l'implementazione per utilizzare 'string username' come parametro, poiché è lo standard per RoleProvider.
        public override string[] GetRolesForUser(string username)
        {
            var userFromDb = db.Utenti.FirstOrDefault(u => u.Email == username);
            if (userFromDb != null)
            {
                var userRole = userFromDb.Ruolo;
                string[] userRoles = new string[] { userRole };
                return userRoles;
            }
            return new string[] { }; // Se l'utente non viene trovato, restituisci un array vuoto.
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = db.Utenti.FirstOrDefault(u => u.Email == username);
            return user != null && user.Ruolo.Equals(roleName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }

        public override bool RoleExists(string roleName)
        {
            return db.Utenti.Any(u => u.Ruolo.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
