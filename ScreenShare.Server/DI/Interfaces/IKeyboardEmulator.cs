namespace ScreenShare.Server.DI.Interfaces;

internal interface IKeyboardEmulator
{
	void KeyDown(VirtualKey key);
	void KeyUp(VirtualKey key);
}

internal enum VirtualKey
{
	A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
}