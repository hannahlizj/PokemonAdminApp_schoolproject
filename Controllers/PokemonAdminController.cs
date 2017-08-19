using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PokemonAdminApp2.Models;
using System.IO;

namespace PokemonAdminApp2.Controllers
{
    public class PokemonAdminController : Controller
    {
        // GET: PokemonAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VisAllePokemoner()
        {
            //Koble mot database
            using (var pokemonDBKobling = new PokemonDBEntities())
            {
                //LINQ-spørring (dynamisk variabel)
                var pokemonliste = (from pokemon in pokemonDBKobling.Pokemon.Include("Trener")
                                    select pokemon).ToList();

                return View(pokemonliste);
            }
        }

        [HttpGet]
        public ActionResult OpprettPokemon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OpprettPokemon(Pokemon pokemon, HttpPostedFileBase file)
        {
            var filnavn = Path.GetFileName(file.FileName);
            var filsti = Path.Combine(Server.MapPath("~/Content"), filnavn);

            file.SaveAs(filsti);

            //Hver gang man kobler til databasen utfører man try-catch-blokk
            try { 
                using (var pokemonDBKobling = new PokemonDBEntities())
                {
                        pokemon.BildeSrc = filnavn;

                        pokemonDBKobling.Pokemon.Add(pokemon);
                        pokemonDBKobling.SaveChanges();

                        return RedirectToAction("VisAllePokemoner");
                }
            }
            catch
            {
                ViewBag.Feilmelding = "Noe gikk galt med databasen.";
                return View();
            }
        }
    }
}