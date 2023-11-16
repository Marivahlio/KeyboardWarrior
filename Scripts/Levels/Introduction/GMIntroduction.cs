using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class GMIntroduction : GameManager
{
	private enum LevelState {ColoringKeyboard, EnteringPasscode}

	[Export] public Godot.Collections.Array<Color> FPressedColors;
	[Export] public Godot.Collections.Array<Color> FRestColors;
	[Export] public Control FColorCodeParent;
	[Export] public Texture2D FColorCodeFullImage;

	private Dictionary<int, int> FKeysToPress = new();
	private LevelState FLevelState = LevelState.ColoringKeyboard;

	public override void _Ready()
	{
		base._Ready();

		// 40 interactable keys in total, might make this a static const value later on
		for (int i = 0; i < 40; i++)
		{
			FKeysToPress.Add(i, 1);
		}

		for (int i = 0; i < FGameKeyboard.GetInteractableKeys().Count; i++)
		{
			IKIntroduction InteractableKey = (IKIntroduction)FGameKeyboard.GetInteractableKeys()[i];
			InteractableKey.Setup();
			InteractableKey.SetColors(FPressedColors[FInputHandler.GetKeyData(i).GetRowIndex()], FRestColors[FInputHandler.GetKeyData(i).GetRowIndex()]);
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

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Enter))
		{
			for (int i = 1; i < FGameKeyboard.GetInteractableKeys().Count; i++)
			{
				((IKIntroduction)FGameKeyboard.GetInteractableKeys()[i]).DoAction();
				FKeysToPress[i] = 0;
			}

			HandleKeyPress(FInputHandler.GetKeyData(0));
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
				for (int i = 0; i < FColorCodeParent.GetChildren().Count; i++)
				{
					TextureRect Item = FColorCodeParent.GetChild<TextureRect>(i);
					Tween tween = GetTree().CreateTween();
					tween.TweenProperty(Item, "scale", new Vector2(0, 1), 0.5f).SetDelay(0.5f * i).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
					tween.TweenCallback(Callable.From(() => Item.Texture = FColorCodeFullImage));
					tween.TweenProperty(Item, "scale", Vector2.One, 0.5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
					tween.TweenCallback(Callable.From(() => SwitchLevelState(LevelState.EnteringPasscode)));
				}
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
