using ScreenShare.Server.DI.Interfaces;
using System.Runtime.InteropServices;

namespace ScreenShare.Server.DI.Services.InputEmulators.KeyboardEmulators;

internal class WindowsKeyboardEmulator : IKeyboardEmulator
{
	private const byte KEYEVENTF_KEYUP = 0x02;
	private const byte KEYEVENTF_KEYDOWN = 0x00;
	private const byte VK_A = 0x41;
	private const byte VK_SHIFT = 0x10;
	private const byte VK_RETURN = 0x0D;
	private const byte VK_TAB = 0x09;

	[DllImport("user32.dll")]
	static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

	public void KeyDown(VirtualKey key)
	{
		keybd_event((byte)(VK_A + (byte)key), 0, KEYEVENTF_KEYDOWN, 0);
	}

	public void KeyUp(VirtualKey key)
	{
		keybd_event((byte)(VK_A + (int)key), 0, KEYEVENTF_KEYUP, 0);
	}
}