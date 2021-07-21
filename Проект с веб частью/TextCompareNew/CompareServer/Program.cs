using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace CompareServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
     WebHost.CreateDefaultBuilder(args)
         .UseStartup<Startup>()
         .UseKestrel(options =>
         {
             options.Limits.MaxConcurrentConnections = 100;
             options.Limits.MaxRequestBodySize = 30 * 1024 * 1024;    // Максимальный размер запроса (30Mb)
             options.Limits.MinRequestBodyDataRate =
                 new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));   // задает минимальную скорость передачи данных в запросе в байтах в секунду.
             options.Limits.MinResponseDataRate =
                 new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));   // задает минимальную скорость передачи данных в исходящем потоке в байтах в секунду.
             options.Listen(IPAddress.Loopback, 5001);
         })
         .Build();
    }
}
