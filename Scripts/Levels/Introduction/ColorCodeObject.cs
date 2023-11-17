using Godot;
using System;

public partial class ColorCodeObject : TextureRect
{
	[Export] private Label FCodeLabel;
	[Export] public Texture2D FColorCodeFullImage;
	
	private int FCodeValue;

	public void ChangeTexture()
	{
		Texture = FColorCodeFullImage;
		FCodeLabel.Visible = true;
	}

	// I want the code to be semi-random, so I handle the randomization in here depending on the index
	public void SetCode(int pIndexInCode)
	{
		switch (pIndexInCode)
		{
			case 0:
				FCodeValue = GD.RandRange(1, 10);
				break;

			case 1:
				FCodeValue = GD.RandRange(1, 5);
				break;

			case 2:
				FCodeValue = GD.RandRange(1, 10);
				break;

			case 3:
				FCodeValue = GD.RandRange(7, 10);
				break;
		}

		FCodeLabel.Text = FCodeValue.ToString();
	}

	// The code we display is 1-10 inclusive, but we use 0-9 in code 
	public int GetCode()
	{
		return FCodeValue - 1;
	}
}
