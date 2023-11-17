using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class GMIntroduction : GameManager
{
	private enum LevelState {Transitioning, ColoringKeyboard, EnteringPasscode, LevelComplete}

	[Export] public Godot.Collections.Array<Color> FPressedColors;
	[Export] public Godot.Collections.Array<Color> FRestColors;
	[Export] public Control FColorCodeParent;
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
		
		// Loop through all keys, set colors and their scale to 0
		for (int i = 0; i < FInteracatableKeys.Count; i++)
		{
			IKIntroduction InteractableKey = (IKIntroduction)FInteracatableKeys[i];
			InteractableKey.Setup();
			InteractableKey.SetColors(FPressedColors[FInputHandler.GetKeyData(i).GetRowIndex()], FRestColors[FInputHandler.GetKeyData(i).GetRowIndex()]);
			InteractableKey.PlayScaleTween(Vector2.Zero, 0, 0);
		}

		// Loop through all keys, set their scale to one with a delay
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
		// Quick and easy debug cheat.
		// TODO: REMOVE WHEN DONE WITH THIS LEVEL
		if (Input.IsKeyPressed(Key.Enter))
		{
			for (int i = 1; i < FInteracatableKeys.Count; i++)
			{
				((IKIntroduction)FInteracatableKeys[i]).OnKeyPressed();
				((IKIntroduction)FInteracatableKeys[i]).OnKeyUp();
				FKeysToPress[i] = 0;
			}

			HandleKeyPress(FInputHandler.GetKeyData(0));
			((IKIntroduction)FInteracatableKeys[0]).OnKeyUp();
		}

		if (FLevelState == LevelState.EnteringPasscode)
		{
			Key BlueKey = FInputHandler.GetKeyData(new Vector2I(FColorCodeParent.GetChild<ColorCodeObject>(0).GetCode(), 2)).GetKey();
			Key GreenKey = FInputHandler.GetKeyData(new Vector2I(FColorCodeParent.GetChild<ColorCodeObject>(1).GetCode(), 1)).GetKey();
			Key PinkKey = FInputHandler.GetKeyData(new Vector2I(FColorCodeParent.GetChild<ColorCodeObject>(2).GetCode(), 3)).GetKey();
			Key OrangeKey = FInputHandler.GetKeyData(new Vector2I(FColorCodeParent.GetChild<ColorCodeObject>(3).GetCode(), 0)).GetKey();

			if (FInputHandler.IsKeyBeingHeld(OrangeKey) &&
					FInputHandler.IsKeyBeingHeld(GreenKey) &&
					FInputHandler.IsKeyBeingHeld(BlueKey) &&
					FInputHandler.IsKeyBeingHeld(PinkKey))
			{
				GD.Print("Code works!");
				SwitchLevelState(LevelState.LevelComplete);

				for (int i = 0; i < FInteracatableKeys.Count; i++)
				{
					((IKIntroduction)FInteracatableKeys[i]).OnKeyPressed();
				}
			}
		}
	}

	public override void HandleKeyPress(InteractableKeyData pKeyData)
	{
		base.HandleKeyPress(pKeyData);

		if (FLevelState == LevelState.LevelComplete)
		{
			return;
		}

		// Remove the key from the required keys dict
		int PressedKeyGlobalIndex = pKeyData.GetGlobalIndex();
		((IKIntroduction)FInteracatableKeys[PressedKeyGlobalIndex]).OnKeyPressed();
		FKeysToPress[PressedKeyGlobalIndex] = 0;

		TryStopTween(FBackgroundtween);
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
				TransitionToPasscodeState();
				return;
			}

			DoBackgroundParticleEffect();
		}
	}

	public override void HandleKeyUp(InteractableKeyData pKeyData)
	{
		if (FLevelState == LevelState.LevelComplete)
		{
			return;
		}

		int PressedKeyGlobalIndex = pKeyData.GetGlobalIndex();
		((IKIntroduction)FInteracatableKeys[PressedKeyGlobalIndex]).OnKeyUp();

		TryStopTween(FBackgroundtween);
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

	private void TryStopTween(Tween pTween)
	{
		if (pTween != null && pTween.IsRunning())
		{
			pTween.Stop();
		}
	}

	private void TransitionToPasscodeState()
	{
		SwitchLevelState(LevelState.Transitioning);
		DoBackgroundParticleEffect(300f);

		// Make all particles permanently colored instead of black
		FBackgroundParticlesTween = GetTree().CreateTween().SetParallel(true);
		FBackgroundParticlesTween.TweenProperty(FBackgroundParticles.ProcessMaterial, "color", new Color(1, 1, 1, 1), 3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
		FBackgroundParticlesTween.TweenProperty(FBackgroundParticles, "speed_scale", 0.8f, 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);

		// Reveal the passcode
		for (int i = 0; i < FColorCodeParent.GetChildren().Count; i++)
		{
			ColorCodeObject Item = FColorCodeParent.GetChild<ColorCodeObject>(i);
			Item.SetCode(i);
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(Item, "scale", new Vector2(0, 1), 0.5f).SetDelay(0.5f * i).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
			tween.TweenCallback(Callable.From(() => Item.ChangeTexture()));
			tween.TweenProperty(Item, "scale", Vector2.One, 0.5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
			tween.TweenCallback(Callable.From(() => SwitchLevelState(LevelState.EnteringPasscode)));
		}
	}

	private void DoBackgroundParticleEffect(double pSpeedScaleOverride = 0)
	{
			double SpeedScale = pSpeedScaleOverride;
			
			// Increase the speed of the background particles
			if (pSpeedScaleOverride == 0)
			{
				SpeedScale = FBackgroundParticles.SpeedScale;
				SpeedScale += 1.5f;
				Mathf.Clamp(SpeedScale, 1, 15); 
			}

			// Make the particles appear colored and fade to black again, also tween speed back to normal
			((ParticleProcessMaterial)FBackgroundParticles.ProcessMaterial).Color = new Color(1, 1, 1, 1);
			TryStopTween(FBackgroundParticlesTween);
			FBackgroundParticlesTween = GetTree().CreateTween();
			FBackgroundParticlesTween.TweenProperty(FBackgroundParticles, "speed_scale", SpeedScale, 0.1f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo);
			FBackgroundParticlesTween.TweenProperty(FBackgroundParticles.ProcessMaterial, "color", new Color(0, 0, 0, 1), 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
			FBackgroundParticlesTween.Parallel().TweenProperty(FBackgroundParticles, "speed_scale", 1f, 2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
	}
}
