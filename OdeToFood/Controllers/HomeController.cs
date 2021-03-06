﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Models;
using OdeToFood.Services;
using OdeToFood.ViewModels;

namespace OdeToFood.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRestaurantData _restaurantData;
        private readonly IGreeter _greet;

        public HomeController(
            IRestaurantData restaurantData,
            IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greet = greeter;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new HomeIndexViewModel
            {
                Restaurants = _restaurantData.GetAll(),
                CurrentMessage = _greet.GetMessageOfTheDay()
            };
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RestaurantEditModel model)
        {
            if (ModelState.IsValid)
            {
                var newRestaurant = new Restaurant()
                {
                    Name = model.Name,
                    Cuisine = model.Cuisine
                };

                newRestaurant = _restaurantData.Add(newRestaurant);

                return RedirectToAction(nameof(Details), new { id = newRestaurant.Id });
            }
            else
            {
                return View();
            }
        }
    }
}
