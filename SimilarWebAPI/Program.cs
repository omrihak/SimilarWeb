using SimilarWebAPI.Manager;
using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace SimilarWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            var config = new HttpSelfHostConfiguration("http://localhost:8081");

            config.MapHttpAttributeRoutes();

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
