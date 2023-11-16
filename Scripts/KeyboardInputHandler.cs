using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class KeyboardInputHandler : Node
{
	private Dictionary<Key, InteractableKeyData> FKeyData = new();
	[Export] private GameManager FGameManager;

	public override void _Ready()
	{
		GenerateKeyData();
	}

	public override void _UnhandledInput(InputEvent pEvent)
	{
		if (pEvent is InputEventKey eventKey)
		{
			if (eventKey.IsPressed())
			{
				HandleKeyPress(eventKey.PhysicalKeycode);
			}

			// TODO: Make it so pressed buttons are stored in a list
			// If its !Pressed and part of the list, call a different event
		}
	}

	public InteractableKeyData GetKeyData(Key pKey)
	{
		if (FKeyData.ContainsKey(pKey) == false)
		{
			GD.PushError("Key not found: " + pKey.ToString());
			return null;
		}

		return FKeyData[pKey];
	}

	public InteractableKeyData GetKeyData(int pIndex)
	{
		if (pIndex >= FKeyData.Count)
		{
			GD.PushError("Index out of bounds: " + pIndex);
			return null;
		}

		return FKeyData.ElementAt(pIndex).Value;
	}

	private void HandleKeyPress(Key pKey)
	{
		if (FKeyData.ContainsKey(pKey) == false)
		{
			return;
		}
		
		FGameManager.HandleKeyPress(FKeyData[pKey]);
	}

	private void GenerateKeyData()
	{
		FKeyData.Add(Key.Key1, new InteractableKeyData(Key.Key1, 0, 0, 0));
		FKeyData.Add(Key.Key2, new InteractableKeyData(Key.Key2, 1, 0, 1));
		FKeyData.Add(Key.Key3, new InteractableKeyData(Key.Key3, 2, 0, 2));
		FKeyData.Add(Key.Key4, new InteractableKeyData(Key.Key4, 3, 0, 3));
		FKeyData.Add(Key.Key5, new InteractableKeyData(Key.Key5, 4, 0, 4));
		FKeyData.Add(Key.Key6, new InteractableKeyData(Key.Key6, 5, 0, 5));
		FKeyData.Add(Key.Key7, new InteractableKeyData(Key.Key7, 6, 0, 6));
		FKeyData.Add(Key.Key8, new InteractableKeyData(Key.Key8, 7, 0, 7));
		FKeyData.Add(Key.Key9, new InteractableKeyData(Key.Key9, 8, 0, 8));
		FKeyData.Add(Key.Key0, new InteractableKeyData(Key.Key0, 9, 0, 9));

		FKeyData.Add(Key.Q, new InteractableKeyData(Key.Q, 0, 1, 10));
		FKeyData.Add(Key.W, new InteractableKeyData(Key.W, 1, 1, 11));
		FKeyData.Add(Key.E, new InteractableKeyData(Key.E, 2, 1, 12));
		FKeyData.Add(Key.R, new InteractableKeyData(Key.R, 3, 1, 13));
		FKeyData.Add(Key.T, new InteractableKeyData(Key.T, 4, 1, 14));
		FKeyData.Add(Key.Y, new InteractableKeyData(Key.Y, 5, 1, 15));
		FKeyData.Add(Key.U, new InteractableKeyData(Key.U, 6, 1, 16));
		FKeyData.Add(Key.I, new InteractableKeyData(Key.I, 7, 1, 17));
		FKeyData.Add(Key.O, new InteractableKeyData(Key.O, 8, 1, 18));
		FKeyData.Add(Key.P, new InteractableKeyData(Key.P, 9, 1, 19));

		FKeyData.Add(Key.A, new InteractableKeyData(Key.A, 0, 2, 20));
		FKeyData.Add(Key.S, new InteractableKeyData(Key.S, 1, 2, 21));
		FKeyData.Add(Key.D, new InteractableKeyData(Key.D, 2, 2, 22));
		FKeyData.Add(Key.F, new InteractableKeyData(Key.F, 3, 2, 23));
		FKeyData.Add(Key.G, new InteractableKeyData(Key.G, 4, 2, 24));
		FKeyData.Add(Key.H, new InteractableKeyData(Key.H, 5, 2, 25));
		FKeyData.Add(Key.J, new InteractableKeyData(Key.J, 6, 2, 26));
		FKeyData.Add(Key.K, new InteractableKeyData(Key.K, 7, 2, 27));
		FKeyData.Add(Key.L, new InteractableKeyData(Key.L, 8, 2, 28));
		FKeyData.Add(Key.Semicolon, new InteractableKeyData(Key.Semicolon, 9, 2, 29));

		FKeyData.Add(Key.Z, new InteractableKeyData(Key.Z, 0, 3, 30));
		FKeyData.Add(Key.X, new InteractableKeyData(Key.X, 1, 3, 31));
		FKeyData.Add(Key.C, new InteractableKeyData(Key.C, 2, 3, 32));
		FKeyData.Add(Key.V, new InteractableKeyData(Key.V, 3, 3, 33));
		FKeyData.Add(Key.B, new InteractableKeyData(Key.B, 4, 3, 34));
		FKeyData.Add(Key.N, new InteractableKeyData(Key.N, 5, 3, 35));
		FKeyData.Add(Key.M, new InteractableKeyData(Key.M, 6, 3, 36));
		FKeyData.Add(Key.Comma, new InteractableKeyData(Key.Comma, 7, 3, 37));
		FKeyData.Add(Key.Period, new InteractableKeyData(Key.Period, 8, 3, 38));
		FKeyData.Add(Key.Slash, new InteractableKeyData(Key.Slash, 9, 3, 39));
	}
}
