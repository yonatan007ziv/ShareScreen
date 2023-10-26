using System.Drawing;

namespace ScreenShareServer.DI.Interfaces;

internal interface IScreenExtractor
{
	Bitmap GetScreen();
}