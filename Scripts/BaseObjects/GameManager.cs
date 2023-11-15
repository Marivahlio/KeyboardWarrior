using Godot;
using System;

public partial class GameManager : Node
{
	[Export] public GameKeyboard FGameKeyboard;

	public virtual void HandleKeyPress(InteractableKeyData pKeyData) {}
}
