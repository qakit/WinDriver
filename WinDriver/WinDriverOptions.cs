namespace UIAutomation.Win
{
	public class WinDriverOptions
	{
		public string App { get; set; }
		public int ImplicitWaitTimeout { get; set; } = 1;
		public int AppStartUpTimeOut { get; set; } = 5;
	}
}