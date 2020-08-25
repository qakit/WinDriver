using System;
using System.Configuration;
using Microsoft.Owin.Hosting;

namespace UIAutomation.Client
{
	class Program
	{
		private static readonly string BaseAddress = ConfigurationManager.AppSettings["rootUrl"];

		[MTAThread]
		static void Main(string[] args)
		{
			WebApp.Start<Startup>(BaseAddress);
			Console.WriteLine($"Server started and waiting on {BaseAddress}");
			Console.ReadLine();
		}
	}
}
