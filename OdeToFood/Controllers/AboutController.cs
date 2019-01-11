using Microsoft.AspNetCore.Mvc;

namespace OdeToFood.Controllers
{
    [Route("company/[controller]")] // Use token here.
    public class AboutController
    {
        [Route("")]
        public string Phone()
        {
            return "8888 1111";
        }

        [Route("[action]")]
        public string Address()
        {
            return "AU";
        }
    }
}
