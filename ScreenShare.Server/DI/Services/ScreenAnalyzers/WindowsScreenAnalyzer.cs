using ScreenShare.Server.DI.Interfaces;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScreenShare.Server.DI.Services.ScreenAnalyzers;

internal class WindowsScreenAnalyzer : IScreenAnalyzer
{
	private const int SRCCOPY = 0xCC0020;
	private const int SM_CXSCREEN = 0;
	private const int SM_CYSCREEN = 1;

	[DllImport("user32.dll")]
	static extern int GetSystemMetrics(int nIndex);

	[DllImport("user32.dll")]
	static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("user32.dll")]
	static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("gdi32.dll")]
	static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

	public Bitmap GetScreen()
	{
		IntPtr desktopPtr = GetDC(IntPtr.Zero);
		Graphics g = Graphics.FromHdc(desktopPtr);
		IntPtr hdc = g.GetHdc();

		// Get the screen dimensions
		int width = GetSystemMetrics(SM_CXSCREEN);
		int height = GetSystemMetrics(SM_CYSCREEN);

		// Create a bitmap to store the screen capture
		Bitmap bitmap = new Bitmap(width, height);

		using (Graphics memoryGraphics = Graphics.FromImage(bitmap))
		{
			IntPtr dc = memoryGraphics.GetHdc();
			BitBlt(dc, 0, 0, width, height, hdc, 0, 0, SRCCOPY);
			memoryGraphics.ReleaseHdc(dc);
		}

		// Release resources
		g.ReleaseHdc(hdc);
		ReleaseDC(IntPtr.Zero, desktopPtr);

		return bitmap;
	}
}