using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Services;

namespace OdeToFood.ViewComponents
{
    /// <summary>
    /// This acts as controller
    /// </summary>
    public class GreeterViewComponent : ViewComponent
    {
        private IGreeter _greeter;

        public GreeterViewComponent(IGreeter greeter)
        {
            _greeter = greeter;
        }

        /// <summary>
        /// Naming convention should call Invoke
        /// The method can accept parameter
        /// The method can by async
        /// </summary>
        /// <returns></returns>
        public IViewComponentResult Invoke()
        {
            var model = _greeter.GetMessageOfTheDay();
            return View("Default", model); // By convention, the first parameter which is string passed to View method should be treated as View name.
        }
    }
}
