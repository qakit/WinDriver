using System;
using System.Windows.Automation;
using System.Windows.Threading;

namespace UIAutomation.Client.Internal
{
	internal class SessionHelper
	{
		private readonly int _implicitWait;

		internal SessionHelper(int implicitWait)
		{
			_implicitWait = implicitWait;
		}

		internal Tuple<Guid, AutomationElement> FindElement(AutomationElement parentElement, Condition condition)
		{
			Console.WriteLine($"Parent element is {parentElement.Current.Name} & {parentElement.Current.ClassName}");
			var walker = new TreeWalker(condition);
			var element = GetElement(walker, parentElement);
			var elementGuid = element == null ? Guid.Empty : Guid.NewGuid();

			return new Tuple<Guid, AutomationElement>(elementGuid, element);
		}

		internal AutomationElement GetElement(
			TreeWalker walker,
			AutomationElement root)
		{
			AutomationElement uiElement = null;
			WaitUntil(() =>
				{
					uiElement = root != null ? walker.GetFirstChild(root) : walker.GetFirstChild(AutomationElement.RootElement);
					return uiElement != null;
				}, _implicitWait);
			return uiElement;
		}

		internal void WaitUntil(Func<bool> condition, int timeout)
		{
			var frame = new DispatcherFrame();
			var timer = new DispatcherTimer();
			timer.Tick += (sender, args) =>
			{
				if (condition() || timeout <= 0)
				{
					timer.Stop();
					frame.Continue = false;
				}
				else
				{
					timeout--;
				}
			};
			timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
			timer.Start();
			Dispatcher.PushFrame(frame);
		}
	}
}
