using Godot;
using System;

public partial class GameManager : Node
{
	[Export] public KeyboardInputHandler FInputHandler;
	[Export] public GameKeyboard FGameKeyboard;

	public virtual void HandleKeyPress(InteractableKeyData pKeyData) {}
	public virtual void HandleKeyUp(InteractableKeyData pKeyData) {}
}
