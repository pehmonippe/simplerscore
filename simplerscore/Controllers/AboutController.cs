namespace SimplerScore.Controllers
{
    using System.Reflection;
    using System.Web.Http;

    [RoutePrefix("about")]
    public class AboutController : ApiController
    {
        [HttpGet]
        [Route("")]
        public string Version ()
        {
            var assembly = Assembly.GetEntryAssembly();

            var version = assembly.GetName().Version.ToString();

            var infoAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (null != infoAttribute)
            {
                version += $" {infoAttribute.InformationalVersion}";
            }

            return version;
        }
    }
}
