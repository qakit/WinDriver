namespace UIAutomation.Core
{
	public class ElementAction
	{
		public ActionType ActionType { get; set; }
	}

	public enum ActionType
	{
		Invoke,
		Click,
		Select,
		DoDefaultAction,
		GetToggleState,
		Expand,
		Collapse
	}
}