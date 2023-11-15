using Godot;
using System;
using System.Collections.Generic;

public partial class GMIntroduction : GameManager
{
	private enum LevelState {ColoringKeyboard, EnteringPasscode}

	[Export] public Godot.Collections.Array<Color> FLockColors;

	private Dictionary<int, int> FKeysToPress = new();
	private LevelState FLevelState = LevelState.ColoringKeyboard;

	public override void _Ready()
	{
		// 40 interactable keys in total, might make this a static const value later on
		for (int i = 0; i < 40; i++)
		{
			FKeysToPress.Add(i, 1);
		}

		for (int i = 0; i < FGameKeyboard.GetInteractableKeys().Count; i++)
		{
			IKIntroduction InteractableKey = (IKIntroduction)FGameKeyboard.GetInteractableKeys()[i];
			InteractableKey.PlayScaleTween(Vector2.Zero, 0, 0);
		}

		for (int i = 0; i < 10; i++)
		{
			float Duration = 0.1f;
			float BaseDelay = 0.1f * i;
			IKIntroduction InteractableKey1 = (IKIntroduction)FGameKeyboard.GetInteractableKeys()[i];
			InteractableKey1.PlayScaleTween(Vector2.One, Duration, BaseDelay);

			IKIntroduction InteractableKey2 = (IKIntroduction)FGameKeyboard.GetInteractableKeys()[i+10];
			InteractableKey2.PlayScaleTween(Vector2.One, Duration, BaseDelay + 0.1f);

			IKIntroduction InteractableKey3 = (IKIntroduction)FGameKeyboard.GetInteractableKeys()[i+20];
			InteractableKey3.PlayScaleTween(Vector2.One, Duration, BaseDelay + 0.2f);

			IKIntroduction InteractableKey4 = (IKIntroduction)FGameKeyboard.GetInteractableKeys()[i+30];
			InteractableKey4.PlayScaleTween(Vector2.One, Duration, BaseDelay + 0.3f);
		}
	}

	public override void HandleKeyPress(InteractableKeyData pKeyData)
	{
		base.HandleKeyPress(pKeyData);

		int PressedKeyGlobalIndex = pKeyData.GetGlobalIndex();
		FGameKeyboard.GetInteractableKeys()[PressedKeyGlobalIndex].DoAction();
		FKeysToPress[PressedKeyGlobalIndex] = 0;

		if (FLevelState == LevelState.ColoringKeyboard)
		{
			if (HaveAllKeysBeenPressed())
			{
				// Uncover passcode
				SwitchLevelState(LevelState.EnteringPasscode);
				GD.Print("All keys pressed");
			}
		}
	}

	private bool HaveAllKeysBeenPressed()
	{
		for (int i = 0; i < FKeysToPress.Count; i++)
		{
			if (FKeysToPress[i] == 1)
			{
				return false;
			}
		}

		return true;
	}

	private void SwitchLevelState(LevelState pNewState)
	{
		FLevelState = pNewState;
	}
}
