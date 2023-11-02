using ShareScreen.Core;

namespace ScreenShare.Server.DI.Interfaces;

internal interface IMouseEmulator
{
	void MoveMouse(Vector2 pos);
	void LeftUp(Vector2 pos);
	void LeftDown(Vector2 pos);
	void RightUp(Vector2 pos);
	void RightDown(Vector2 pos);
	void MiddleUp(Vector2 pos);
	void MiddleDown(Vector2 pos);
	void MiddleScrollUp(Vector2 pos);
	void MiddleScrollDown(Vector2 pos);
	void ForwardUp(Vector2 pos);
	void ForwardDown(Vector2 pos);
	void BackwardUp(Vector2 pos);
	void BackwardDown(Vector2 pos);
}