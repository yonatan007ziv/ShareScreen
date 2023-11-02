using System.Drawing;

namespace ScreenShare.Server.DI.Interfaces;

internal interface IScreenAnalyzer
{
	Bitmap GetScreen();
}