using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojodachi.Controllers {
    public class HomeController : Controller {
        private DojodachiContext dbContext;
        public HomeController(DojodachiContext context) {
            dbContext = context;
        }
        [Route ("/")]
        [HttpGet]
        public IActionResult Index () {
            Pet newPet = new Pet();
            dbContext.Add(newPet);
            ViewBag.Status = 0;
            newPet.Fullness = 20;
            newPet.Happiness = 20;
            newPet.Energy = 50;
            newPet.Meals = 3;
            dbContext.SaveChanges();
            HttpContext.Session.SetInt32("SessionID", newPet.id);
            System.Console.WriteLine(newPet.id);
            
            ViewBag.Picture = 1;
            string newMessage = "It's Monday morning and Michael is unmotivated. Help Michael make it to the weekend!";
            ViewBag.Message = newMessage;
            
            return View (newPet);
        }

        [Route ("feed")]
        public IActionResult Feed () {
            
            ViewBag.Status = 0;
            int currentID = (int) HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(u => u.id == currentID);
            System.Console.WriteLine(currentPet);

            Pet updated = new Pet ();
            // int feedFullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            // updated.Happiness = (int) HttpContext.Session.GetInt32 ("Happiness");
            // updated.Fullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            // updated.Energy = (int) HttpContext.Session.GetInt32 ("Energy");
            // updated.Meals = (int) HttpContext.Session.GetInt32("Meals");
            // int checkMeals = (int) HttpContext.Session.GetInt32 ("Meals");
            // if(checkMeals <= 0){
            //     string mealMessage = "Michael needs to do some work before he can eat!";
            //     ViewBag.Message = mealMessage;
            //     ViewBag.Picture = 1;
            //     return View("Index", updated);
            // }
            // // Update Meals with each Feed
            // int meals = (int) HttpContext.Session.GetInt32 ("Meals");
            // HttpContext.Session.SetInt32 ("Meals", meals - 1);
            // updated.Meals = (int) HttpContext.Session.GetInt32 ("Meals");
            // //25% chance Dojodachi Wont like Feed 
            // Random rand = new Random ();
            // int randChance = rand.Next (1, 4);
            // if (randChance == 2) {
            //     updated.Fullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            //     string message = "Michael did not like that Meal! Fullness + 0, Meals - 1";
            //     ViewBag.Message = message;
            //     ViewBag.Picture = 2;
            //     if (updated.Fullness < 1 || updated.Happiness <= 0) {
            //         string deathMessage = "You let Toby talk to Michael!";
            //         ViewBag.Message = deathMessage;
            //         ViewBag.Picture = 8;
            //         ViewBag.Status = 1;
            //     }
            //     if (updated.Fullness >= 100 || updated.Happiness >= 100) {
            //         string successMessage = "Michael made it to the weekend!";
            //         ViewBag.Message = successMessage;
            //         ViewBag.Picture = 9;
            //         ViewBag.Status = 1;
            //     }
            //     return View ("Index", updated);
            // }
            // Random fullnessValue = new Random ();
            // int newFeedFullness = fullnessValue.Next (5, 10);
            // HttpContext.Session.SetInt32 ("Fullness", feedFullness + newFeedFullness);
            // string feedMessage = $"You fed Michael! Fullness +{newFeedFullness}, Meals - 1";
            // ViewBag.Picture = 3;
            // ViewBag.Message = feedMessage;
            // if (updated.Fullness <= 0 || updated.Happiness <= 0) {
            //     string deathMessage = "You let Toby talk to Michael!";
            //     ViewBag.Message = deathMessage;
            //     ViewBag.Picture = 8;
            //     ViewBag.Status = 1;
            // }
            // if (updated.Fullness >= 100 || updated.Happiness >= 100) {
            //     string successMessage = "Michael made it to the weekend!";
            //     ViewBag.Message = successMessage;
            //     ViewBag.Picture = 9;
            //     ViewBag.Status = 1;
            // }
            return View ("Index");

        }

        [Route ("play")]
        public IActionResult Play () {
            ViewBag.Status = 0;
            Pet updated = new Pet ();
            // Every play gains between 5-10 happiness
            updated.Fullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            // 25% chance Dojodachi won't like Play
            Random rand = new Random ();
            int randChance = rand.Next (1, 4);
            if (randChance == 2) {
                int newPlayEnergy = (int) HttpContext.Session.GetInt32 ("Energy");
                HttpContext.Session.SetInt32 ("Energy", newPlayEnergy - 5);
                updated.Energy = (int) HttpContext.Session.GetInt32 ("Energy");
                updated.Meals = (int) HttpContext.Session.GetInt32 ("Meals");
                updated.Happiness = (int) HttpContext.Session.GetInt32 ("Happiness");
                string BadMessage = "Michael played a prank on Angela and she called Corporate, Energy -5, Happiness + 0";
                ViewBag.Picture = 4;
                ViewBag.Message = BadMessage;
                if (updated.Fullness <= 0 || updated.Happiness <= 0) {
                    string deathMessage = "You let Toby talk to Michael!";
                    ViewBag.Message = deathMessage;
                    ViewBag.Picture = 8;
                    ViewBag.Status = 1;
                }
                if (updated.Fullness >= 100 || updated.Happiness >= 100) {
                    string successMessage = "Michael made it to the weekend!";
                    ViewBag.Message = successMessage;
                    ViewBag.Picture = 9;
                    ViewBag.Status = 1;
                }
                return View ("Index", updated);
            }
            // Every play loses 5 energy
            int playEnergy = (int) HttpContext.Session.GetInt32 ("Energy");
            HttpContext.Session.SetInt32 ("Energy", playEnergy - 5);
            updated.Energy = (int) HttpContext.Session.GetInt32 ("Energy");
            updated.Meals = (int) HttpContext.Session.GetInt32 ("Meals");
            int playHappiness = (int) HttpContext.Session.GetInt32 ("Happiness");
            Random happiness = new Random ();
            int newHappiness = happiness.Next (5, 10);
            HttpContext.Session.SetInt32 ("Happiness", playHappiness + newHappiness);
            updated.Happiness = (int) HttpContext.Session.GetInt32 ("Happiness");
            string GoodMessage = $"Michael played a prank on Dwight! Happiness +{newHappiness}, Energy - 5";
            ViewBag.Message = GoodMessage;
            ViewBag.Picture = 5;
            if (updated.Fullness <= 0 || updated.Happiness <= 0) {
                string deathMessage = "You let Toby talk to Michael!";
                ViewBag.Message = deathMessage;
                ViewBag.Picture = 8;
                ViewBag.Status = 1;
            }
            if (updated.Fullness >= 100 || updated.Happiness >= 100) {
                string successMessage = "Congrats! Michael made it to the weekend!";
                ViewBag.Message = successMessage;
                ViewBag.Picture = 9;
                ViewBag.Status = 1;
            }
            return View ("Index", updated);
        }

        [Route ("work")]
        public IActionResult Work () {
            ViewBag.Status = 0;
            Pet updated = new Pet ();
            updated.Happiness = (int) HttpContext.Session.GetInt32 ("Happiness");
            updated.Fullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            // Every Work loses 5 energy
            int workEnergy = (int) HttpContext.Session.GetInt32 ("Energy");
            HttpContext.Session.SetInt32 ("Energy", workEnergy - 5);
            updated.Energy = (int) HttpContext.Session.GetInt32 ("Energy");
            // Every Work gains between 1-3 meals
            int workMeal = (int) HttpContext.Session.GetInt32 ("Meals");
            Random meal = new Random ();
            int newWorkMeal = meal.Next (1, 3);
            HttpContext.Session.SetInt32 ("Meals", workMeal + newWorkMeal);
            updated.Meals = (int) HttpContext.Session.GetInt32 ("Meals");
            string newMessage = $"Michael had a great day at work, he earned {newWorkMeal} meals, Energy -5";
            ViewBag.Message = newMessage;
            ViewBag.Picture = 6;
            if (updated.Fullness <= 0 || updated.Happiness <= 0) {
                string deathMessage = "You let Toby talk to Michael!";
                ViewBag.Message = deathMessage;
                ViewBag.Picture = 8;
                ViewBag.Status = 1;
            }
            if (updated.Fullness >= 100 || updated.Happiness >= 100) {
                string successMessage = "Michael made it to the weekend!";
                ViewBag.Message = successMessage;
                ViewBag.Picture = 9;
                ViewBag.Status = 1;
            }
            return View ("Index", updated);
        }

        [Route ("sleep")]
        public IActionResult Sleep () {
            ViewBag.Status = 0;
            Pet updated = new Pet ();
            // Each sleep loses 5 happiness
            int sleepHappiness = (int) HttpContext.Session.GetInt32 ("Happiness");
            HttpContext.Session.SetInt32 ("Happiness", sleepHappiness - 5);
            updated.Happiness = (int) HttpContext.Session.GetInt32 ("Happiness");
            // Each sleep loses 5 Fullness
            int sleepFullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            HttpContext.Session.SetInt32 ("Fullness", sleepFullness - 5);
            updated.Fullness = (int) HttpContext.Session.GetInt32 ("Fullness");
            // Each sleep gains 15 energy
            int sleepEnergy = (int) HttpContext.Session.GetInt32 ("Energy");
            HttpContext.Session.SetInt32 ("Energy", sleepEnergy + 15);
            updated.Energy = (int) HttpContext.Session.GetInt32 ("Energy");
            updated.Meals = (int) HttpContext.Session.GetInt32 ("Meals");
            string newMessage = "Michael had a great night sleep, Energy + 15, Happiness -5, Fullness -5";
            ViewBag.Message = newMessage;
            ViewBag.Picture = 7;
            if (updated.Fullness <= 0 || updated.Happiness <= 0) {
                string deathMessage = "You let Toby talk to Michael!";
                ViewBag.Message = deathMessage;
                ViewBag.Picture = 8;
                ViewBag.Status = 1;
            }
            if (updated.Fullness >= 100 || updated.Happiness >= 100) {
                string successMessage = "Michael made it to the weekend!";
                ViewBag.Message = successMessage;
                ViewBag.Picture = 9;
                ViewBag.Status = 1;
            }
            return View ("Index", updated);
        }

    }
}