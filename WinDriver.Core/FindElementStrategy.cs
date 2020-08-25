namespace UIAutomation.Core
{
	public class FindElementStrategy
	{
		public FindElementStrategy(By by, string selector)
		{
			By = by;
			Selector = selector;
		}
		public By By { get; }
		public string Selector { get; }
		public ControlType ControlType { get; set; }
	}

	public enum By
	{
		Name,
		TypeAndName,
		AutomationId
	}

	public enum ControlType
	{
		Window,
		Menu,
		MenuBar,
		MenuItem,
		Tree,
		TreeItem,
		CheckBox,
		Button,
		Text
	}
}