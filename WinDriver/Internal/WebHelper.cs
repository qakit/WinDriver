using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UIAutomation.Core;

namespace UIAutomation.Win.Internal
{
	internal static class WebHelper
	{
		/// <summary>
		/// Send request which initialize new session
		/// </summary>
		/// <param name="driver"></param>
		/// <returns></returns>
		internal static string StartSession(WinDriver driver)
		{
			using (var httpClient = new HttpClient())
			{
				var launchSession = new LaunchSession
				{
					App = driver.Options.App,
					AppStartUpTimeOut = driver.Options.AppStartUpTimeOut,
					ImplicitWaitTimeout = driver.Options.ImplicitWaitTimeout
				};

				var json = JsonConvert.SerializeObject(launchSession);
				var data = new StringContent(json, Encoding.UTF8, "application/json");

				var responseMessage = httpClient.PostAsync($"{driver.Uri}/session", data).Result;
				var result = responseMessage.Content.ReadAsStringAsync().Result;
				return result.Replace("\"", string.Empty);
			}
		}

		/// <summary>
		/// Quit current session. Remove app/remove session
		/// </summary>
		/// <param name="driver"></param>
		internal static void QuitSession(WinDriver driver)
		{
			using (var httpClient = new HttpClient())
			{
				var responseMessage = httpClient.DeleteAsync($"{driver.Uri}/session/{ driver.Guid }").Result;
				var result = responseMessage.Content.ReadAsStringAsync().Result;
				Console.WriteLine(result);
			}
		}

		/// <summary>
		/// Send request to find element in current session (window)
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="strategy"></param>
		/// <returns></returns>
		internal static Guid? FindElement(WinDriver driver, FindElementStrategy strategy)
		{
			using (var httpClient = new HttpClient())
			{
				var json = JsonConvert.SerializeObject(strategy);
				var data = new StringContent(json, Encoding.UTF8, "application/json");

				var responseMessage = httpClient.PostAsync($"{driver.Uri}/session/{driver.Guid}/element", data).Result;
				var result = JsonConvert.DeserializeObject<Element>(responseMessage.Content.ReadAsStringAsync().Result);

				return result.Id;
			}
		}

		/// <summary>
		/// Send request to find element in current element
		/// </summary>
		/// <param name="root"></param>
		/// <param name="strategy"></param>
		/// <returns></returns>
		internal static Guid? FindElement(WinElement root, FindElementStrategy strategy)
		{
			using (var httpClient = new HttpClient())
			{
				var json = JsonConvert.SerializeObject(strategy);
				var data = new StringContent(json, Encoding.UTF8, "application/json");

				var responseMessage = httpClient.PostAsync($"{root.Driver.Uri}/session/{root.Driver.Guid}/element/{root.Guid}/element", data).Result;
				var result = JsonConvert.DeserializeObject<Element>(responseMessage.Content.ReadAsStringAsync().Result);

				return result.Id;
			}
		}

		/// <summary>
		/// Send request to perform action under element
		/// </summary>
		/// <param name="element"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		internal static object PerformAction(WinElement element, ElementAction action)
		{
			using (var httpClient = new HttpClient())
			{
				var json = JsonConvert.SerializeObject(action);
				var data = new StringContent(json, Encoding.UTF8, "application/json");

				var responseMessage = httpClient.PostAsync($"{element.Driver.Uri}/session/{element.Driver.Guid}/element/{element.Guid}/action", data).Result;
				var result = responseMessage.Content.ReadAsStringAsync().Result;
				return result;
			}
		}
	}
}