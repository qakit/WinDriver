using System;
using System.Web.Http;
using Owin;

namespace UIAutomation.Client
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			// Configure Web API for self-host. 
			var config = new HttpConfiguration();

			config.MapHttpAttributeRoutes();

			var options = new SimpleLoggerOptions
			{
				Log = (key, value) => Console.WriteLine("{0}:{1}", key, value),
				RequestKeys = new[] { "owin.RequestPath", "owin.RequestMethod" },
				ResponseKeys = new[] { "owin.ResponseStatusCode", "owin.ResponseBody" }
			};

			appBuilder.Map("", apiBuilder =>
			{
				apiBuilder.Use((ctx, next) => next());
				apiBuilder.UseWebApi(config);
			});

			appBuilder.Use<SimpleLogger>(options);
		}
	}
}