using Godot;
using System;
using System.Collections.Generic;

public partial class KeyboardManager : Node
{
	[Export] public Node FKeyboard;

	private Dictionary<Key, GameKeyboardData> FKeyData = new();

	public override void _Ready()
	{
		FKeyData.Add(Key.Key1, new GameKeyboardData(Key.Key1, 0, 0, 0));
		FKeyData.Add(Key.Key2, new GameKeyboardData(Key.Key2, 1, 0, 1));
		FKeyData.Add(Key.Key3, new GameKeyboardData(Key.Key3, 2, 0, 2));
		FKeyData.Add(Key.Key4, new GameKeyboardData(Key.Key4, 3, 0, 3));
		FKeyData.Add(Key.Key5, new GameKeyboardData(Key.Key5, 4, 0, 4));
		FKeyData.Add(Key.Key6, new GameKeyboardData(Key.Key6, 5, 0, 5));
		FKeyData.Add(Key.Key7, new GameKeyboardData(Key.Key7, 6, 0, 6));
		FKeyData.Add(Key.Key8, new GameKeyboardData(Key.Key8, 7, 0, 7));
		FKeyData.Add(Key.Key9, new GameKeyboardData(Key.Key9, 8, 0, 8));
		FKeyData.Add(Key.Key0, new GameKeyboardData(Key.Key0, 9, 0, 9));

		FKeyData.Add(Key.Q, new GameKeyboardData(Key.Q, 0, 1, 10));
		FKeyData.Add(Key.W, new GameKeyboardData(Key.W, 1, 1, 11));
		FKeyData.Add(Key.E, new GameKeyboardData(Key.E, 2, 1, 12));
		FKeyData.Add(Key.R, new GameKeyboardData(Key.R, 3, 1, 13));
		FKeyData.Add(Key.T, new GameKeyboardData(Key.T, 4, 1, 14));
		FKeyData.Add(Key.Y, new GameKeyboardData(Key.Y, 5, 1, 15));
		FKeyData.Add(Key.U, new GameKeyboardData(Key.U, 6, 1, 16));
		FKeyData.Add(Key.I, new GameKeyboardData(Key.I, 7, 1, 17));
		FKeyData.Add(Key.O, new GameKeyboardData(Key.O, 8, 1, 18));
		FKeyData.Add(Key.P, new GameKeyboardData(Key.P, 9, 1, 19));

		FKeyData.Add(Key.A, new GameKeyboardData(Key.A, 0, 2, 20));
		FKeyData.Add(Key.S, new GameKeyboardData(Key.S, 1, 2, 21));
		FKeyData.Add(Key.D, new GameKeyboardData(Key.D, 2, 2, 22));
		FKeyData.Add(Key.F, new GameKeyboardData(Key.F, 3, 2, 23));
		FKeyData.Add(Key.G, new GameKeyboardData(Key.G, 4, 2, 24));
		FKeyData.Add(Key.H, new GameKeyboardData(Key.H, 5, 2, 25));
		FKeyData.Add(Key.J, new GameKeyboardData(Key.J, 6, 2, 26));
		FKeyData.Add(Key.K, new GameKeyboardData(Key.K, 7, 2, 27));
		FKeyData.Add(Key.L, new GameKeyboardData(Key.L, 8, 2, 28));
		FKeyData.Add(Key.Semicolon, new GameKeyboardData(Key.Semicolon, 9, 2, 29));

		FKeyData.Add(Key.Z, new GameKeyboardData(Key.Z, 0, 3, 30));
		FKeyData.Add(Key.X, new GameKeyboardData(Key.X, 1, 3, 31));
		FKeyData.Add(Key.C, new GameKeyboardData(Key.C, 2, 3, 32));
		FKeyData.Add(Key.V, new GameKeyboardData(Key.V, 3, 3, 33));
		FKeyData.Add(Key.B, new GameKeyboardData(Key.B, 4, 3, 34));
		FKeyData.Add(Key.N, new GameKeyboardData(Key.N, 5, 3, 35));
		FKeyData.Add(Key.M, new GameKeyboardData(Key.M, 6, 3, 36));
		FKeyData.Add(Key.Comma, new GameKeyboardData(Key.Comma, 7, 3, 37));
		FKeyData.Add(Key.Period, new GameKeyboardData(Key.Period, 8, 3, 38));
		FKeyData.Add(Key.Slash, new GameKeyboardData(Key.Slash, 9, 3, 39));
	}

	public override void _UnhandledInput(InputEvent pEvent)
	{
		if (pEvent is InputEventKey eventKey)
		{
			if (eventKey.IsPressed())
			{
				HandleKeyPress(eventKey.PhysicalKeycode);
			}
		}
	}

	private void HandleKeyPress(Key pKey)
	{
		if (FKeyData.ContainsKey(pKey) == false)
		{
			return;
		}
		
		AnimatedSprite2D node = FKeyboard.GetChild<AnimatedSprite2D>(FKeyData[pKey].GetGlobalIndex());
		node.Frame = 1;
	}
}
