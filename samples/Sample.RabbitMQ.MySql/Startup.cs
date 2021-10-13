using DotNetCore.CAP.Dashboard.NodeDiscovery;
using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sample.RabbitMQ.MySql
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();

            services.AddCap(x =>
            {
                x.UseEntityFramework<AppDbContext>();

                x.UseRabbitMQ(aa =>
                {
                    aa.HostName = "117.50.40.186";
                    aa.UserName = "guest";
                    aa.Password = "guest";
                });

                x.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "117.50.40.186";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "117.50.40.186";
                    d.CurrentNodePort = 64616;
                    d.NodeName = "CAP No.1 Consul";
                });

                x.UseDashboard();
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetService<ILogger<Startup>>();
                    logger.LogError($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                };
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
