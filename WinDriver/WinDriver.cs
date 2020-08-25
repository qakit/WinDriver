using System;
using UIAutomation.Core;
using UIAutomation.Win.Internal;

namespace UIAutomation.Win
{
	public class WinDriver : IAutomationElement
	{
		public WinDriver(Uri uri, WinDriverOptions options)
		{
			Uri = uri;
			Options = options;
			Guid = new Guid(WebHelper.StartSession(this));
			Driver = this;
		}

		public WinDriver Driver { get; }

		internal WinDriverOptions Options { get; }
		internal Uri Uri { get; }
		
		public Guid? Guid { get; }

		public void Quit()
		{
			WebHelper.QuitSession(this);
		}

		public WinElement FindElementByName(string elementName)
		{
			var strategy = new FindElementStrategy(By.Name, elementName);
			return FindElement(strategy);
		}

		public WinElement FindElementByAutomationId(string elementName)
		{
			var strategy = new FindElementStrategy(By.AutomationId, elementName);
			return FindElement(strategy);
		}

		public WinElement FindElementByTypeAndName(ControlType controlType, string elementName)
		{
			var strategy = new FindElementStrategy(By.TypeAndName, elementName) {ControlType = controlType};
			return FindElement(strategy);
		}

		private WinElement FindElement(FindElementStrategy strategy)
		{
			var elementGuid = WebHelper.FindElement(this, strategy);
			if (elementGuid == null) return null;

			var element = new WinElement(this, elementGuid);
			return element;
		}
	}
}