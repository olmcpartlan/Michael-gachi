using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        private DojodachiContext dbContext;
        public HomeController(DojodachiContext context)
        {
            dbContext = context;
        }


        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            Pet newPet = new Pet();
            dbContext.Add(newPet);
            ViewBag.Status = 0;
            newPet.Fullness = 20;
            newPet.Happiness = 20;
            newPet.Energy = 50;
            newPet.Meals = 3;
            dbContext.SaveChanges();
            HttpContext.Session.SetInt32("SessionID", newPet.id);

            ViewBag.Picture = 1;
            string newMessage = "It's Monday morning and Michael is unmotivated. Help Michael make it to the weekend!";
            ViewBag.Message = newMessage;

            return View(newPet);
        }

        [Route("feed")]
        public IActionResult Feed()
        {

            ViewBag.Status = 0;
            int currentID = (int)HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(u => u.id == currentID);
            // Check that Michael has Meals
            if (currentPet.Meals <= 0)
            {
                ViewBag.Message = "Michael needs to do some work before he can eat!";
                return View("Index", currentPet);
            }
            // Feeding Michal
            currentPet.Meals -= 1;
            // Random Chance Michael won't like the food
            Random rand = new Random();
            int randChance = rand.Next(1, 4);
            if (randChance == 3)
            {
                ViewBag.Message = "Michael did not like that Meal! Fullness + 0, Meals - 1";
                return View("Index", currentPet);
            }
            // Random fullness value from feeding
            Random fullnessValue = new Random();
            int foodValue = fullnessValue.Next(5, 10);
            currentPet.Fullness += foodValue;
            dbContext.SaveChanges();
            ViewBag.Message = $"You fed Michael! Fullness +{foodValue}, Meals - 1";
            ViewBag.Picture = 3;
            if (currentPet.Fullness >= 100 || currentPet.Happiness >= 100)
            {
                return Redirect("/success");
            }

            if (currentPet.Fullness <= 0 || currentPet.Happiness <= 0)
            {
                return Redirect("/death");
            }

            return View("Index", currentPet);
        }

        [Route("play")]
        public IActionResult Play()
        {
            ViewBag.Status = 0;
            int currentID = (int)HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(p => p.id == currentID);
            // 25% chance Angela will call Corporate
            Random rand = new Random();
            int randChance = rand.Next(1, 4);
            if (randChance == 2)
            {
                currentPet.Energy -= 5;
                dbContext.SaveChanges();
                string BadMessage = "Michael played a prank on Angela and she called Corporate, Energy -5, Happiness + 0";
                ViewBag.Picture = 4;
                ViewBag.Message = BadMessage;

            }
            else
            {
                currentPet.Energy -= 5;
                Random happiness = new Random();
                int newHappiness = happiness.Next(5, 10);
                currentPet.Happiness += newHappiness;
                string GoodMessage = $"Michael played a prank on Dwight! Happiness +{newHappiness}, Energy - 5";
                dbContext.SaveChanges();
                ViewBag.Message = GoodMessage;
                ViewBag.Picture = 5;
            }
            if (currentPet.Fullness >= 100 || currentPet.Happiness >= 100)
            {
                return Redirect("/success");
            }

            if (currentPet.Fullness <= 0 || currentPet.Happiness <= 0)
            {
                return Redirect("/death");
            }

            return View("Index", currentPet);
        }

        [Route("work")]
        public IActionResult Work()
        {
            ViewBag.Status = 0;
            int currentID = (int)HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(p => p.id == currentID);
            // Working takes 5 energy from Michael
            currentPet.Energy -= 5;
            // Working gains between 1-3 meals
            Random rand = new Random();
            int mealVal = rand.Next(1, 3);
            currentPet.Meals += mealVal;
            dbContext.SaveChanges();
            string newMessage = $"Michael had a great day at work, he earned {mealVal} meals, Energy -5";
            ViewBag.Message = newMessage;
            ViewBag.Picture = 6;

            if (currentPet.Fullness >= 100 || currentPet.Happiness >= 100)
            {
                return Redirect("/success");
            }

            if (currentPet.Fullness <= 0 || currentPet.Happiness <= 0)
            {
                return Redirect("/death");
            }

            return View("Index", currentPet);
        }

        [Route("sleep")]
        public IActionResult Sleep()
        {
            ViewBag.Status = 0;
            int currentID = (int)HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(p => p.id == currentID);
            // Sleeping takes 5 Fullness
            currentPet.Fullness -= 5;
            // Sleeping gains 15 Energy
            currentPet.Energy += 15;
            // Sleeping loses 5 Happiness 
            currentPet.Happiness -= 5;
            dbContext.SaveChanges();
            string newMessage = "Michael had a great night sleep, Energy + 15, Happiness -5, Fullness -5";
            ViewBag.Message = newMessage;
            ViewBag.Picture = 7;
            if (currentPet.Fullness >= 100 || currentPet.Happiness >= 100)
            {
                return Redirect("/success");
            }

            if (currentPet.Fullness <= 0 || currentPet.Happiness <= 0)
            {
                return Redirect("/death");
            }
            return View("Index", currentPet);
        }


        [Route("/success")]
        public IActionResult Success()
        {
            int currentID = (int)HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(p => p.id == currentID);
            string successMessage = "Michael made it to the weekend!";
            ViewBag.Message = successMessage;
            ViewBag.Picture = 9;
            ViewBag.Status = 1;

            return View("Index", currentPet);
        }

        [Route("/death")]
        public IActionResult Death()
        {
            int currentID = (int)HttpContext.Session.GetInt32("SessionID");
            Pet currentPet = dbContext.Pets.FirstOrDefault(p => p.id == currentID);
            string deathMessage = "You let Toby talk to Michael!";
            ViewBag.Message = deathMessage;
            ViewBag.Picture = 8;
            ViewBag.Status = 1;
            return View("Index", currentPet);
        }

    }
}