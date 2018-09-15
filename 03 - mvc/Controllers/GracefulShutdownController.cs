using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _03___mvc.Models;
using _03___mvc.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace _03___mvc.Controllers
{
    public class GracefulShutdownController : Controller
    {
        private readonly IApplicationLifetime _lifetime;
        private readonly ITrackingServer _tracking;


        public GracefulShutdownController(
            IApplicationLifetime lifetime,
            ITrackingServer tracking)
        {
            _lifetime = lifetime;
            _tracking = tracking;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Confirm(RequestedUserViewModel model)
        {
            _lifetime.StopApplication();

            return new ContentResult()
            {
                StatusCode = 200,
                ContentType = "text/plain",
                Content = $"Server [{_tracking.ServerGuid.ToString()}] will be graceful shutdown soon, \r\nrequested by {model.UserName}({model.Id}) \r\nreason:{model.Reason}"
            };
        }
    }
}