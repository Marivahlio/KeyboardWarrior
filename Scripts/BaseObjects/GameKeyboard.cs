using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameKeyboard : MarginContainer
{
	[Export] private Node FInteractableKeyParent;

	private List<InteractableKey> FInteracatableKeys = new();

	public List<InteractableKey> GetInteractableKeys()
	{
		if (FInteracatableKeys.Count <= 0)
		{
			var InteractableKeys = FInteractableKeyParent.GetChildren().ToList();
			
			foreach (InteractableKey InteractableKey in InteractableKeys)
			{
				FInteracatableKeys.Add(InteractableKey);
			}
		}

		return FInteracatableKeys;
	}
}
