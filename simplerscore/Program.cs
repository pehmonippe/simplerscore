namespace SimplerScore
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Owin.Hosting;

    internal static class ProgramExtensions
    {
        public static bool IsHelp (this string[] args)
        {
            var isHelp = args
                .Any(a => a.StartsWith("--help", StringComparison.InvariantCultureIgnoreCase)
                          || a.StartsWith("-?", StringComparison.InvariantCultureIgnoreCase));

            return isHelp;
        }

        public static string BaseUrl (this string[] args)
        {
            var baseUrl = args
                .FirstOrDefault(a => a.StartsWith("--url", System.StringComparison.InvariantCultureIgnoreCase));

            if (null == baseUrl)
                return null;

            var elements = baseUrl.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            baseUrl = elements.Length > 1 ?
                elements[1] :
                null;

            return baseUrl;
        }
    }

    internal class Program
    {
        #region Private variables
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private static void Main (string[] args)
        {
            if (args.IsHelp())
            {
                ShowStartupOptions();
                WaitForUserInteractionForExit();
                return;
            }

            // command line overrides everything
            // then app settings
            // finally default
            var baseUrl = args.BaseUrl();
            baseUrl = baseUrl ?? ConfigurationManager.AppSettings["web.baseurl"];
            baseUrl = baseUrl ?? "http://*:8888";

            Log.Info($"Api hosted at {baseUrl}");

            using (WebApp.Start<Startup>(baseUrl))
            {
                WaitForUserInteractionForExit();
            }
        }

        private static void ShowStartupOptions ()
        {
            var message = new[]
            {
                @"Options are: ",
                @"-------------------+-------------------",
                @"--help |-?          Show this help",
                @"--url=<startup url> Service address"
            };

            Console.Write(message.Aggregate("", (seed, next) => seed + Environment.NewLine + next));
        }

        private static void WaitForUserInteractionForExit ()
        {
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
