using log4net.Config;
using log4net;

namespace LinkAggregatorMVC.Helpers
{
    public static class HelperMethods
    {
        public static void AddLog4net(this IServiceCollection services)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        }
    }
}
