using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.WebAPI.Controllers
{
    public class DevController : ApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public DevController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ASPNETCORE_ENVIRONMENT: {_webHostEnvironment.EnvironmentName}");
            sb.AppendLine($"AppContext.BaseDirectory: {AppContext.BaseDirectory}");
            sb.AppendLine($"ConnectionString FortifexDatabase: {_configuration.GetSection("ConnectionStrings")["FortifexDatabase"]}");

            return Content(sb.ToString());
        }
    }
}
