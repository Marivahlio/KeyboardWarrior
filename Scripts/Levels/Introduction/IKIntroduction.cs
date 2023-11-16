using Godot;
using System;
using System.Collections.Generic;

public partial class IKIntroduction : InteractableKey
{
	private Color FPressedColor;
	private Color FRestColor;
	private Tween FActiveColorTween;
	private TextureRect FFilledImage;
	private GpuParticles2D FPressedParticles;

	public void Setup()
	{
		FFilledImage = GetChild<TextureRect>(0);
		FPressedParticles = GetChild<GpuParticles2D>(1);
	}

	public void OnKeyPressed()
	{
		if (FActiveColorTween != null && FActiveColorTween.IsRunning())
		{
			FActiveColorTween.Stop();
		}

		FPressedParticles.Restart();
		FPressedParticles.Emitting = true;

		FFilledImage.Modulate = FPressedColor;
		FActiveColorTween = GetTree().CreateTween();
		FActiveColorTween.TweenProperty(FFilledImage, "scale", new Vector2(1.1f, 1.1f), 0.05f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
	}

	public void OnKeyUp()
	{
		if (FActiveColorTween != null && FActiveColorTween.IsRunning())
		{
			FActiveColorTween.Stop();
		}

		FActiveColorTween = GetTree().CreateTween();
		FActiveColorTween.TweenProperty(FFilledImage, "scale", new Vector2(1f, 1f), 0.5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad);
		FActiveColorTween.SetParallel().TweenProperty(FFilledImage, "modulate", FRestColor, 0.2f);
	}

	public void SetColors(Color pPressedColor, Color pRestColor)
	{
		FPressedColor = pPressedColor;
		FRestColor = pRestColor;
		((ParticleProcessMaterial)FPressedParticles.ProcessMaterial).Color = FRestColor;
	}

	public void PlayScaleTween(Vector2 pScale, float pDuration, float delay)
	{
		if (FActiveColorTween != null && FActiveColorTween.IsRunning())
		{
			FActiveColorTween.Stop();
		}

		FActiveColorTween = GetTree().CreateTween();
		FActiveColorTween.TweenProperty(this, "scale", pScale, pDuration).SetDelay(delay).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
	}
}
