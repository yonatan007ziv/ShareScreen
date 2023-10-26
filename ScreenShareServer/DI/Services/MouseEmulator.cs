using ScreenShareServer.DI.Interfaces;
using ShareScreenCore;
using System.Runtime.InteropServices;

namespace ScreenShareServer.DI.Services;

internal class MouseEmulator : IMouseEmulator
{
	private const int MOUSEEVENTF_LEFTUP = 0x04;
	private const int MOUSEEVENTF_LEFTDOWN = 0x02;
	
	private const int MOUSEEVENTF_RIGHTUP = 0x10;
	private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
	
	private const int MOUSEEVENTF_MIDDLEUP = 0x40;
	private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
	
	[DllImport("user32.dll")]
	static extern bool SetCursorPos(int X, int Y);

	[DllImport("user32.dll")]
	static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	public void MoveMouse(Vector2 pos)
	{
		SetCursorPos(pos.X, pos.Y);
	}

	public void LeftUp(Vector2 pos)
	{
		MoveMouse(pos);
		mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
	}

	public void LeftDown(Vector2 pos)
	{
		MoveMouse(pos);
		mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
	}

	public void RightUp(Vector2 pos)
	{
		MoveMouse(pos);
		mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
	}

	public void RightDown(Vector2 pos)
	{
		MoveMouse(pos);
		mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
	}

	public void MiddleUp(Vector2 pos)
	{
		MoveMouse(pos);
		mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
	}

	public void MiddleDown(Vector2 pos)
	{
		MoveMouse(pos);
		mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
	}

	public void MiddleScrollUp(Vector2 pos)
	{
		throw new NotImplementedException();
	}

	public void MiddleScrollDown(Vector2 pos)
	{
		throw new NotImplementedException();
	}

	public void ForwardUp(Vector2 pos)
	{
		throw new NotImplementedException();
	}

	public void ForwardDown(Vector2 pos)
	{
		throw new NotImplementedException();
	}

	public void BackwardUp(Vector2 pos)
	{
		throw new NotImplementedException();
	}

	public void BackwardDown(Vector2 pos)
	{
		throw new NotImplementedException();
	}
}