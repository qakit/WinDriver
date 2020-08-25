using System;
using UIAutomation.Core;
using UIAutomation.Win.Internal;

namespace UIAutomation.Win
{
	/// <summary>
	/// Represent basic win automation element
	/// </summary>
	public class WinElement : IAutomationElement
	{
		internal WinElement(WinDriver driver, Guid? elementGuid)
		{
			Driver = driver;
			Guid = elementGuid;
		}

		public Guid? Guid { get; }

		public WinElement Element => this;

		/// <summary>
		/// Gets the <see cref="ToggleState"/> of current element if it's checkbox and
		/// Toggle Pattern is available
		/// </summary>
		public ToggleState ToggleState
		{
			get
			{
				var state = WebHelper.PerformAction(this, new ElementAction {ActionType = ActionType.GetToggleState});
				var value = (ToggleState) Enum.ToObject(typeof(ToggleState), Int32.Parse(state.ToString()));
				return value;
			}
		}

		/// <summary>
		/// Find element in current element using it's name
		/// </summary>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public WinElement FindElementByName(string elementName)
		{
			var strategy = new FindElementStrategy(By.Name, elementName);
			return FindElement(strategy);
		}

		/// <summary>
		/// Find element in current element using it's AutomationId
		/// </summary>
		/// <param name="automationId"></param>
		/// <returns></returns>
		public WinElement FindElementByAutomationId(string automationId)
		{
			var strategy = new FindElementStrategy(By.AutomationId, automationId);
			return FindElement(strategy);
		}

		/// <summary>
		/// Find element in current element using it's ControlType and Name
		/// </summary>
		/// <param name="controlType"></param>
		/// <param name="elementName"></param>
		/// <returns></returns>
		public WinElement FindElementByTypeAndName(ControlType controlType, string elementName)
		{
			var strategy = new FindElementStrategy(By.TypeAndName, elementName) {ControlType = controlType};
			return FindElement(strategy);
		}

		private WinElement FindElement(FindElementStrategy strategy)
		{
			var elementGuid = WebHelper.FindElement(this, strategy);
			if (elementGuid == null) return null;

			var element = new WinElement(Driver, elementGuid);
			return element;
		}

		public WinDriver Driver { get; }

		public void Click()
		{
			WebHelper.PerformAction(this, new ElementAction {ActionType = ActionType.Click });
		}

		public void Invoke()
		{
			WebHelper.PerformAction(this, new ElementAction { ActionType = ActionType.Invoke });
		}

		public void DoDefaultAction()
		{
			WebHelper.PerformAction(this, new ElementAction { ActionType = ActionType.DoDefaultAction });
		}

		public void Expand()
		{
			WebHelper.PerformAction(this, new ElementAction {ActionType = ActionType.Expand});
		}

		public void Collapse()
		{
			WebHelper.PerformAction(this, new ElementAction { ActionType = ActionType.Collapse });
		}

		public void Select()
		{
			WebHelper.PerformAction(this, new ElementAction { ActionType = ActionType.Select });
		}
	}
}