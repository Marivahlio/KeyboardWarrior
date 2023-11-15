using Godot;
using System;
using System.Collections.Generic;

public partial class IKIntroduction : InteractableKey
{
	public override void DoAction() 
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(GetChild(0), "modulate", new Color(1, 1, 1, 1), 0.5f);
	}

	public void PlayScaleTween(Vector2 pScale, float pDuration, float delay)
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", pScale, pDuration).SetDelay(delay).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
	}
}
