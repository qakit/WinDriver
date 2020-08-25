using System;
using UIAutomation.Core;

namespace UIAutomation.Win
{
	public interface IAutomationElement
	{
		WinElement FindElementByName(string elementName);
		WinElement FindElementByAutomationId(string elementName);
		WinElement FindElementByTypeAndName(ControlType controlType, string elementName);
		WinDriver Driver { get; }
		Guid? Guid { get; }
	}
}