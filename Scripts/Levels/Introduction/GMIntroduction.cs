using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class GMIntroduction : GameManager
{
	private enum LevelState {Transitioning, ColoringKeyboard, EnteringPasscode}

	[Export] public Godot.Collections.Array<Color> FPressedColors;
	[Export] public Godot.Collections.Array<Color> FRestColors;
	[Export] public Control FColorCodeParent;
	[Export] public Texture2D FColorCodeFullImage;
	[Export] public ColorRect FBackground;
	[Export] public GpuParticles2D FBackgroundParticles;

	private Dictionary<int, int> FKeysToPress = new();
	private LevelState FLevelState = LevelState.ColoringKeyboard;

	private Tween FBackgroundtween;
	private Tween FBackgroundParticlesTween;

	public override void _Ready()
	{
		base._Ready();

		// 40 interactable keys in total, might make this a static const value later on
		for (int i = 0; i < 40; i++)
		{
			FKeysToPress.Add(i, 1);
		}

		for (int i = 0; i < FInteracatableKeys.Count; i++)
		{
			IKIntroduction InteractableKey = (IKIntroduction)FInteracatableKeys[i];
			InteractableKey.Setup();
			InteractableKey.SetColors(FPressedColors[FInputHandler.GetKeyData(i).GetRowIndex()], FRestColors[FInputHandler.GetKeyData(i).GetRowIndex()]);
			InteractableKey.PlayScaleTween(Vector2.Zero, 0, 0);
		}

		for (int i = 0; i < 10; i++)
		{
			float Duration = 0.1f;
			float BaseDelay = 0.1f * i;
			IKIntroduction InteractableKey1 = (IKIntroduction)FInteracatableKeys[i];
			InteractableKey1.PlayScaleTween(Vector2.One, Duration, BaseDelay);

			IKIntroduction InteractableKey2 = (IKIntroduction)FInteracatableKeys[i+10];
			InteractableKey2.PlayScaleTween(Vector2.One, Duration, BaseDelay + 0.1f);

			IKIntroduction InteractableKey3 = (IKIntroduction)FInteracatableKeys[i+20];
			InteractableKey3.PlayScaleTween(Vector2.One, Duration, BaseDelay + 0.2f);

			IKIntroduction InteractableKey4 = (IKIntroduction)FInteracatableKeys[i+30];
			InteractableKey4.PlayScaleTween(Vector2.One, Duration, BaseDelay + 0.3f);
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Enter))
		{
			for (int i = 1; i < FInteracatableKeys.Count; i++)
			{
				((IKIntroduction)FInteracatableKeys[i]).DoAction();
				FKeysToPress[i] = 0;
			}

			HandleKeyPress(FInputHandler.GetKeyData(0));
		}
	}

	public override void HandleKeyPress(InteractableKeyData pKeyData)
	{
		base.HandleKeyPress(pKeyData);

		int PressedKeyGlobalIndex = pKeyData.GetGlobalIndex();
		((IKIntroduction)FInteracatableKeys[PressedKeyGlobalIndex]).OnKeyPressed();
		FKeysToPress[PressedKeyGlobalIndex] = 0;

		if (FBackgroundtween != null && FBackgroundtween.IsRunning())
		{
			FBackgroundtween.Stop();
		}

		FBackgroundtween = GetTree().CreateTween();
		FBackgroundtween.TweenProperty(FBackground, "color", FPressedColors[pKeyData.GetRowIndex()], 0.5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);

		if (FLevelState == LevelState.ColoringKeyboard)
		{
			if (FBackgroundParticlesTween != null && FBackgroundParticlesTween.IsRunning())
			{
				FBackgroundParticlesTween.Stop();
			}

			if (HaveAllKeysBeenPressed())
			{
				SwitchLevelState(LevelState.Transitioning);
				FBackgroundParticlesTween = GetTree().CreateTween().SetParallel(true);
				FBackgroundParticlesTween.TweenProperty(FBackgroundParticles.ProcessMaterial, "color", new Color(1, 1, 1, 1), 3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
				FBackgroundParticlesTween.TweenProperty(FBackgroundParticles, "speed_scale", 0.8f, 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

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
			else
			{
				double SpeedScale = FBackgroundParticles.SpeedScale;
				SpeedScale += 1.5f;
				Mathf.Clamp(SpeedScale, 1, 15); 
				FBackgroundParticles.SpeedScale = SpeedScale;

				((ParticleProcessMaterial)FBackgroundParticles.ProcessMaterial).Color = new Color(1, 1, 1, 1);
				FBackgroundParticlesTween = GetTree().CreateTween();
				FBackgroundParticlesTween.TweenProperty(FBackgroundParticles, "speed_scale", 1f, 0.2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
				FBackgroundParticlesTween.SetParallel(true).TweenProperty(FBackgroundParticles.ProcessMaterial, "color", new Color(0, 0, 0, 1), 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
				FBackgroundParticlesTween.TweenProperty(FBackgroundParticles, "speed_scale", 1f, 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
			}
		}
	}

	public override void HandleKeyUp(InteractableKeyData pKeyData)
	{
		int PressedKeyGlobalIndex = pKeyData.GetGlobalIndex();
		((IKIntroduction)FInteracatableKeys[PressedKeyGlobalIndex]).OnKeyUp();

		if (FBackgroundtween != null && FBackgroundtween.IsRunning())
		{
			FBackgroundtween.Stop();
		}

		FBackgroundtween = GetTree().CreateTween();
		FBackgroundtween.TweenProperty(FBackground, "color", new Color(1, 1, 1, 1), 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
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
