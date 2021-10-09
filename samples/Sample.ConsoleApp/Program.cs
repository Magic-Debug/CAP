using System;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sample.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new ServiceCollection();

            container.AddLogging(x => x.AddConsole());
            container.AddCap(x =>
            {
                //console app does not support dashboard

                x.UseMySql("Server=localhost;Port=3306; database=cap; UID=dev; password=1234QWERasdf; SSLMode=none;allowPublicKeyRetrieval=true");
                x.UseRabbitMQ(z =>
                {
                    z.HostName = "117.50.40.186";
                    z.UserName = "guest";
                    z.Password = "guest";
                });
            });

            container.AddSingleton<EventSubscriber>();

            var sp = container.BuildServiceProvider();

            sp.GetService<IBootstrapper>().BootstrapAsync();

            Console.ReadLine();
        }
    }
}