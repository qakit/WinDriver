namespace UIAutomation.Core
{
	/// <summary>
	/// Stub for describing start app and other possible args
	/// </summary>
	public class LaunchSession
	{
		public string App { get; set; }
		public int ImplicitWaitTimeout { get; set; }
		public int AppStartUpTimeOut { get; set; }
	}
}
