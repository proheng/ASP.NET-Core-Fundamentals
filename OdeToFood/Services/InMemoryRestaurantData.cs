﻿using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Services
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        public InMemoryRestaurantData()
        {
            _restaurants = new List<Restaurant>
            {
                new Restaurant {Id = 1 , Name = "Rex Restaurant"},
                new Restaurant {Id = 2 , Name = "Rex 1 Restaurant"},
                new Restaurant {Id = 3 , Name = "Rex 2 Restaurant"},
            };
        }

        List<Restaurant> _restaurants;

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants.OrderBy(r => r.Name); 
        }
    }
}