﻿namespace ShareScreenCore;

public class Vector2
{
	public int X { get; set; }
	public int Y { get; set; }

	public Vector2(int x, int y)
	{
		X = x;
		Y = y;
	}

	public override string ToString()
	{
		return $"({X},{Y})";
	}
}