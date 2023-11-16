using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
	[Export] public KeyboardInputHandler FInputHandler;
	[Export] public Node FInteractableKeysParent;

	protected List<InteractableKey> FInteracatableKeys = new();

	public virtual void HandleKeyPress(InteractableKeyData pKeyData) {}
	public virtual void HandleKeyUp(InteractableKeyData pKeyData) {}

	public override void _Ready()
	{
		base._Ready();

		if (FInteracatableKeys.Count <= 0)
		{
			var InteractableKeys = FInteractableKeysParent.GetChildren().ToList();
			
			foreach (InteractableKey InteractableKey in InteractableKeys)
			{
				FInteracatableKeys.Add(InteractableKey);
			}
		}
	}
}
