using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;
using Point = System.Windows.Point;

namespace UIAutomation.Client.Internal.Input
{
	internal static class Mouse
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

		[DllImport("user32")]
		public static extern int SetCursorPos(int x, int y);

		private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
		private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */

		#region Public Methods

		public static Point CountPointsByRect(AutomationElement element)
		{
			var rect = element.Current.BoundingRectangle;
			return new Point((int)((rect.BottomLeft.X + rect.TopRight.X) / 2), (int)((rect.BottomLeft.Y + rect.TopRight.Y) / 2));
		}

		/// <summary>
		/// Performs click in center of <see cref="AutomationElement"/>
		/// </summary>
		/// <param name="element"></param>
		public static void Click(AutomationElement element)
		{
			var point = CountPointsByRect(element);
			MouseLeftClick(point);
		}

		public static void MouseDown(Point p)
		{
			mouse_event(MOUSEEVENTF_LEFTDOWN, (int)p.X, (int)p.Y, 0, 0);
		}
		
		public static void MouseUp(Point p)
		{
			mouse_event(MOUSEEVENTF_LEFTUP, (int)p.X, (int)p.Y, 0, 0);
		}

		public static void MouseLeftClick(Point p)
		{
			SetCursorPos((int)p.X, (int)p.Y);
			Thread.Sleep(100);
			MouseDown(p);
			Thread.Sleep(100);
			MouseUp(p);
		}

		#endregion Public Methods
	}
}
