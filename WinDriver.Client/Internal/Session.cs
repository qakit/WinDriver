using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using UIAutomation.Client.Internal.Input;
using UIAutomation.Core;
using ControlType = UIAutomation.Core.ControlType;

namespace UIAutomation.Client.Internal
{
	internal class Session : SessionHelper
	{
		private Process ApplicationProcess;

		private Dictionary<Guid, AutomationElement> Elements;

		public Session(LaunchSession launchSession) : base (launchSession.ImplicitWaitTimeout)
		{
			LaunchSession = launchSession;
			SessionId = Guid.NewGuid();
			Elements = new Dictionary<Guid, AutomationElement>();
			Console.WriteLine($"Session with id {SessionId} created!");
		}

		internal Guid SessionId { get; }

		private LaunchSession LaunchSession { get; }

		internal Guid? FindElement(FindElementStrategy strategy)
		{
			switch (strategy.By)
			{
				case By.Name:
					return FindElementByName(strategy);
				case By.AutomationId:
					return FindElementByAutomationId(strategy);
				case By.TypeAndName:
					return FindElementByTypeAndName(strategy);
				default:
					throw new Exception($"Unable to find specified method for strategy: {strategy.By}");
			}
		}

		internal Guid? FindElement(Guid parentElementId, FindElementStrategy strategy)
		{
			var parentElement = Elements[parentElementId];
			switch (strategy.By)
			{
				case By.Name:
					return FindElementByNameInElement(parentElement, strategy);
				case By.AutomationId:
					return FindElementByAutomationId(parentElement, strategy);
				case By.TypeAndName:
					return FindElementByTypeAndName(parentElement, strategy);
				default:
					throw new Exception($"Unable to find specified method for strategy: {strategy.By}");
			}
		}

		internal object PerformAction(Guid elementId, ElementAction action)
		{
			var element = Elements[elementId];

			switch (action.ActionType)
			{
				case ActionType.Invoke:
					Invoke(element);
					break;
				case ActionType.Click:
					Click(element);
					break;
				case ActionType.Select:
					Select(element);
					break;
				case ActionType.DoDefaultAction:
					DoDefaultAction (element);
					break;
				case ActionType.GetToggleState:
					return GetToggleState(element);
				case ActionType.Expand:
					Expand(element);
					break;
				case ActionType.Collapse:
					Collapse(element);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(action), action, null);
			}

			return null;
		}

		#region Element Methods

		internal void Invoke(AutomationElement element)
		{
			var invokePattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			invokePattern.Invoke();
		}

		internal object GetToggleState(AutomationElement element)
		{
			var togglePattern = element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
			if (togglePattern != null)
			{
				return togglePattern.Current.ToggleState;
			}

			throw new NotSupportedException("Toggle pattern is not supported for this element");
		}

		internal void Click(AutomationElement element)
		{
			element.SetFocus();
			Mouse.Click(element);
		}

		internal void Select(AutomationElement element)
		{
			var selectPattern = element.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
			selectPattern?.Select();
		}

		internal void Expand(AutomationElement element)
		{
			var expandCollapsePattern =
				element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
			expandCollapsePattern?.Expand();
		}

		internal void Collapse(AutomationElement element)
		{
			var expandCollapsePattern =
				element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
			expandCollapsePattern?.Collapse();
		}

		internal void DoDefaultAction(AutomationElement element)
		{
			var legacyPattern = element.GetCurrentPattern(LegacyIAccessiblePattern.Pattern) as LegacyIAccessiblePattern;
			legacyPattern?.DoDefaultAction();
		}

		internal Guid? FindElementByName(FindElementStrategy strategy)
		{
			var session = Elements[SessionId];
			return FindElementByNameInElement(session, strategy);
		}

		internal Guid? FindElementByNameInElement(AutomationElement parentElement, FindElementStrategy strategy)
		{
			try
			{
				Console.WriteLine($"Searching for {strategy.Selector} element in parent element");
				var element = FindElement(parentElement,
					new PropertyCondition(AutomationElement.NameProperty, strategy.Selector));
				if (element.Item1 == Guid.Empty) return null;

				Elements.Add(element.Item1, element.Item2);
				Console.WriteLine($"Element found");
				return element.Item1;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return Guid.Empty;
		}

		internal Guid? FindElementByAutomationId(FindElementStrategy strategy)
		{
			var session = Elements[SessionId];
			return FindElementByAutomationId(session, strategy);
		}

		internal Guid? FindElementByAutomationId(AutomationElement parentElement, FindElementStrategy strategy)
		{
			var element = FindElement(parentElement,
				new PropertyCondition(AutomationElement.AutomationIdProperty, strategy.Selector));
			if (element.Item1 == Guid.Empty) return null;
			Elements.Add(element.Item1, element.Item2);
			return element.Item1;
		}

		internal Guid? FindElementByTypeAndName(FindElementStrategy strategy)
		{
			var session = Elements[SessionId];
			return FindElementByTypeAndName(session, strategy);
		}

		internal Guid? FindElementByTypeAndName(AutomationElement parentElement, FindElementStrategy strategy)
		{
			Console.WriteLine("Searching for type and property name");
			var automationType = GetType(strategy.ControlType);
			var condition = new AndCondition(
				new PropertyCondition(AutomationElement.ControlTypeProperty, automationType),
				new PropertyCondition(AutomationElement.NameProperty, strategy.Selector));

			var element = FindElement(parentElement, condition);
			if (element.Item1 == Guid.Empty) return null;

			Elements.Add(element.Item1, element.Item2);
			return element.Item1;
		}

		//TODO find better way
		private System.Windows.Automation.ControlType GetType(ControlType type)
		{
			switch (type)
			{
				case ControlType.Button:
					return System.Windows.Automation.ControlType.Button;
				case ControlType.CheckBox:
					return System.Windows.Automation.ControlType.CheckBox;
				case ControlType.Menu:
					return System.Windows.Automation.ControlType.Menu;
				case ControlType.MenuBar:
					return System.Windows.Automation.ControlType.MenuBar;
				case ControlType.MenuItem:
					return System.Windows.Automation.ControlType.MenuItem;
				case ControlType.Text:
					return System.Windows.Automation.ControlType.Text;
				case ControlType.Tree:
					return System.Windows.Automation.ControlType.Tree;
				case ControlType.TreeItem:
					return System.Windows.Automation.ControlType.TreeItem;
				case ControlType.Window:
					return System.Windows.Automation.ControlType.Window;
				default:
					throw new ArgumentException();
			}
		}

		#endregion

		#region App Methods

		internal void StartApplication()
		{
			ApplicationProcess = new Process();
			var processStartInfo = new ProcessStartInfo
			{
				FileName = LaunchSession.App
			};
			ApplicationProcess.StartInfo = processStartInfo;
			ApplicationProcess.Start();

			int runningTime = 0;
			//assume that handle not null and title not empty (handle splash screen)
			while (ApplicationProcess.MainWindowHandle.Equals(IntPtr.Zero) || ApplicationProcess.Handle.Equals(IntPtr.Zero) || string.IsNullOrEmpty(ApplicationProcess.MainWindowTitle))
			{
				if (runningTime > LaunchSession.AppStartUpTimeOut)
				{
					KillApplication();
				}

				runningTime++;
				Thread.Sleep(1000);
				ApplicationProcess.Refresh();
				Console.WriteLine($"Waiting for process {LaunchSession.App} to start. RunningTime: {runningTime} sec.");
			}

			ApplicationProcess.Refresh();
			Thread.Sleep(2000);
			Console.WriteLine(ApplicationProcess.MainWindowTitle);
			if (!ApplicationProcess.MainWindowHandle.Equals(IntPtr.Zero))
			{
				var element = AutomationElement.FromHandle(ApplicationProcess.MainWindowHandle);
				element.SetFocus();

				Elements.Add(SessionId, element);
			}
			else
			{
				Console.WriteLine("Failed to start application");
			}
		}

		internal void KillApplication()
		{
			try
			{
				Console.WriteLine("Killing application");
				ApplicationProcess.Kill();
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception {e.Message}");
			}
		}

		#endregion
	}
}
